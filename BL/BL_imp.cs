using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace BL
{
    public class BL_imp : IBL
    {
        DAL.IDAL dal = DAL.FactoryDal.GetInstance();
        #region Threads
        static public Thread closeOrder =new Thread( new ThreadStart(CloseOrders));
        static public Thread closeGuestRequest = new Thread(new ThreadStart(closeGuestRequests));
        static public Thread UpdatesDiary = new Thread(new ThreadStart(UpdateDiaryFun));
        static void CloseOrders()   // A process (tahalikhone thread ) that runs once in the day and updates the status of all expired orders has been over a month since the email was sent from the system 
        {
            BL.IBL bl = BL.FactoryBL.GetBL();
            while (true)
            {
                var orders = bl.ListOrdersByDaysPassed(BE.Configuration.DaysExpireOrder); // list of orders according to the days passed
                foreach (BE.Order ord in orders)
                {
                    if (ord.Status == BE.ORDERSTATUS.NotYetTreated || ord.Status == BE.ORDERSTATUS.MailSent)
                        bl.UpdateOrderStatus(ord, BE.ORDERSTATUS.CloseByLackAnswer);
                }
                Thread.Sleep(86400000);// to active the process once a day
            }

        }

        static void UpdateDiaryFun()
        {
            BL.IBL bl = BL.FactoryBL.GetBL();
            while (true)
            {
                var hostingUnits = bl.HostingUnitByCondition(H => { return true; });
                DateTime date = DateTime.Now.AddDays(-1);
                foreach (BE.HostingUnit H in hostingUnits)
                {
                    H.Diary[date.Day, date.Month] = false;
                    bl.UpdateHostingUnit(H);
                }
                Thread.Sleep(86400000);// to active the process once a day
            }
        }
        static void closeGuestRequests()
        {
            BL.IBL bl = BL.FactoryBL.GetBL();
            while (true)
            {
                var guestRs = bl.GuestRequestByCondition(GR => { return bl.CalculateDate(GR.RegistrationDate) > BE.Configuration.DaysExpireGuestRequest; }); // list of orders according to the days passed
                foreach (BE.GuestRequest GR in guestRs)
                {
                    if (GR.Status == BE.GUESTSTATUS.Active)
                        bl.ChangeGuestRequestStatus(GR.GuestRequestKey);
                }
                Thread.Sleep(86400000);// to active the process once a day
            }
        }
        #endregion

        public bool CheckDates(DateTime D1, DateTime D2) // check if this is in year 2020 and if the Date enter is before the Date exit at least one day 
        {
            TimeSpan timeS = D2 - D1;
            if (int.Parse(timeS.Days.ToString()) >= 1)
                return true;
            return false;
        }
        public bool CheckInDairy(DateTime Denter, DateTime Dexit, bool[,] Diary) // // When creating a customer order, make sure that the requested dates are available in the hosting unit offered to them.
        {
            for (DateTime t = Denter; t != Dexit; t = t.AddDays(1))
            {
                if (Diary[t.Day - 1, t.Month - 1])
                    return false;
            }
            return true;
        }
        public string CollectCommission(DateTime Denter, DateTime Dexit, BE.Host host) // // When booking status changes due to closing of transaction - a taxe of 10 NIS  per accommodation must be calculated.
        {
            double price = CalculateDate(Denter, Dexit) * BE.Configuration.Commission;
            BE.SiteOwner.profit += price;
            BE.Configuration.CurrentProfit = BE.SiteOwner.profit;
            return string.Format("{0} NIS collected from your bank account", price);
        }
       

        public void MarkInDiary(bool[,] Diary, DateTime Denter, DateTime Dexit) // // When order status changes due to transaction closing - the relevant dates must be marked in the matrix ( diary)
        {
            if (!CheckDates(Denter, Dexit))
                throw new ArgumentOutOfRangeException("Denter or Dexit", "ERROR: incorect dates");
            if (!CheckInDairy(Denter, Dexit, Diary))
                throw new ArgumentException("ERROR: this dates are bussy", "Denter and Dexit");
            for (DateTime t = Denter; t != Dexit; t = t.AddDays(1))
                Diary[t.Day - 1, t.Month - 1] = true;
        }
        public void ChangeGuestRequestStatus(int GuestRequestKey) // When order status changes due to transaction closing - the status of guest request must be changed accordingly 
        {
            IEnumerable<BE.GuestRequest> GRL = dal.GetGuestRequestsList();
            var v = (from GR in GRL where GR.GuestRequestKey == GuestRequestKey select GR).FirstOrDefault();
            v.Status = BE.GUESTSTATUS.CloseBySite;
            dal.UpdateGuestRequest(v);
        }
        public string DeleteHostingUnit(BE.HostingUnit hu) //  // You can not delete a hosting unit as long as there is an offer associated with it in open mode
        {
            string huName = hu.HostingUnitName;
            IEnumerable<BE.Order> ords = dal.GetOrdersListByCond(t => { return t.HostingUnitKey == hu.HostingUnitKey; });
            foreach (var item in ords)
            {
                if (item.Status == BE.ORDERSTATUS.NotYetTreated || item.Status == BE.ORDERSTATUS.MailSent)
                    throw new ArgumentException("You Can't delete this Hosting Unit as long as there is an offer associated with it in the open.");
            }
            dal.RemoveHostingUnit(hu);
            return "Removing the Hosting Unit " + huName + " accomplish with success";
        }

        // The owner of a hosting unit will be able to send an order to the customer (change the status to "Sent mail"), only if you have signed a bank account debit authorization.
        //When the order status changes to "Sent Mail" the system will automatically send a mail to the customer with the order details ( here we have just to print a message on the screen)
        public void SendMail(BE.Order ord)
        {

            List<BE.HostingUnit> HostUnitList = dal.GetHostingUnitsListByCond(hu => { return hu.HostingUnitKey == ord.HostingUnitKey; });
            if (HostUnitList.FirstOrDefault().Owner.CollectionClearance == false)
                throw new ArgumentException("The owner of a hosting unit can send an invitation only to the customer that have signed for a bank account debit authorization");
            var guestR = dal.GetGuestRequestsListByCond(G => { return G.GuestRequestKey == ord.GuestRequestKey; }).FirstOrDefault();
            var HostingU = dal.GetHostingUnitsListByCond(H => { return H.HostingUnitKey == ord.HostingUnitKey; }).FirstOrDefault();
            MailMessage mail = new MailMessage();
            mail.To.Add(guestR.MailAddress);
            mail.From = new MailAddress(BE.Configuration.FromMail);
            mail.Subject = "Order To Your Future Vacation";
            string pool, jacuzzi, garden;
            if (HostingU.Pool)
                pool = "yes";
            else pool = "no";
            if (HostingU.Garden)
                garden = "yes";
            else garden = "no";
            if (HostingU.Jacuzzi)
                jacuzzi = "yes";
            else jacuzzi = "no";
            mail.Body = String.Format("Hello {0} {1}!\n{2} {3} invite you to his {4}: {5} on the dates that you asked for\nThe details of the hosting unit are:\nPool: {6}\nGarden: {7}\nJacuzzi: {8} \nArea: {9}\n If you are still interested , contact us, we are available at this number {10}",
                guestR.PrivateName, guestR.FamilyName, HostingU.Owner.PrivateName, HostingU.Owner.FamilyName, HostingU.TypeOfHostingUnit, HostingU.HostingUnitName, pool, garden, jacuzzi, HostingU.Area, HostingU.Owner.PhoneNumber);
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(BE.Configuration.FromMail, BE.Configuration.PasswordMail);
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {
                throw new ArgumentException("send Mail Failed... try again later");
                //SendMail(ord);
            }
        }
        public List<BE.HostingUnit> ListofHostingUnitNotOccupied(DateTime Denter, int duration)  // A function that accepts a date and number of vacation days and returns the list of all available hosting units on that date.
        {
            DateTime Dexit = Denter.AddDays(duration);
            List<BE.HostingUnit> notOccupiedHU = dal.GetHostingUnitsListByCond(Hu => { return CheckInDairy(Denter, Dexit, Hu.Diary); });
            return notOccupiedHU;
        }
        public int CalculateDate(params DateTime[] Dates)  // this function receives one or two dates and returns the number of days that have passed from the first date to the second, or if she receives only one date - from the first date to the present day
        {
            DateTime D;  // if the fonction receive only one date 
            if (Dates.Length == 1)
                D = DateTime.Now; // the date of today
            else
                D = Dates[1];
            TimeSpan timeS = D - Dates[0];
            return timeS.Days;
        }
        public List<BE.Order> ListOrdersByDaysPassed(int days)// list of orders according to the days passed
        {
            List<BE.Order> OrdList = dal.GetOrdersListByCond(ord => { return CalculateDate(ord.OrderDate) >= days; });
            return OrdList;
        }

        public List<BE.GuestRequest> GuestRequestByCondition(Predicate<BE.GuestRequest> condition) //  function that can return all customer requirements that matching to a particular condition
        {
            List<BE.GuestRequest> GRL = dal.GetGuestRequestsListByCond(condition);
            return GRL;
        }
        public List<BE.GuestRequest> GuestRequestByCondition(BE.HostingUnit hostU) //  function that can return all customer requirements that matching to a particular condition
        {
            List<BE.GuestRequest> GRL = GuestRequestByCondition(gRequest =>
            {
                return ((CheckInDairy(gRequest.EntryDate,gRequest.ReleaseDate,hostU.Diary))&& ((gRequest.Area == hostU.Area) || gRequest.Area == BE.AREA.All) && (((gRequest.Pool == BE.CHOICE.Necessary || gRequest.Pool == BE.CHOICE.Possible) && hostU.Pool == true) || ((gRequest.Pool == BE.CHOICE.Possible || gRequest.Pool == BE.CHOICE.Uninterested) && hostU.Pool == false)) &&
                        (((gRequest.Garden == BE.CHOICE.Necessary || gRequest.Garden == BE.CHOICE.Possible) && hostU.Garden == true) || ((gRequest.Garden == BE.CHOICE.Possible || gRequest.Garden == BE.CHOICE.Uninterested) && hostU.Garden == false)) &&
                        (((gRequest.Jacuzzi == BE.CHOICE.Necessary || gRequest.Jacuzzi == BE.CHOICE.Possible) && hostU.Jacuzzi == true) || ((gRequest.Jacuzzi == BE.CHOICE.Possible || gRequest.Jacuzzi == BE.CHOICE.Uninterested) && hostU.Jacuzzi == false)) && gRequest.Type == hostU.TypeOfHostingUnit && gRequest.Status==BE.GUESTSTATUS.Active);
            });
            var orders = OrdersByCondition(ord => { return ord.HostingUnitKey == hostU.HostingUnitKey; });
            var guestrequestKeys = from ord in orders select ord.GuestRequestKey;
            var x = from gr in GRL where !IsInTheList(guestrequestKeys.ToList(), gr.GuestRequestKey) select gr;
            return x.ToList();
        }
        private bool IsInTheList(List<int> list, int GuestRequestKey)//helper function
        {
            foreach (int G in list)
                if (G == GuestRequestKey)
                    return true;
            return false;
        }
        public List<BE.HostingUnit> HostingUnitByCondition(Predicate<BE.HostingUnit> condition)
        {
            List<BE.HostingUnit> Hu = dal.GetHostingUnitsListByCond(condition);
            return Hu;
        }
        public List<BE.Order> OrdersByCondition(Predicate<BE.Order> condition)
        {
            List<BE.Order> orders = dal.GetOrdersListByCond(condition);
            return orders;
        }
        public int NumofOrders(BE.GuestRequest guestr) //  // A function that receive guest request and returns the number of orders sent to it
        {
            List<BE.Order> OL = dal.GetOrdersListByCond(ord => { return guestr.GuestRequestKey == ord.GuestRequestKey; });
            return OL.Count;
        }
        public int NumofOrdersClosebySucces(BE.HostingUnit hostU) //  //A function that accepts a hosting unit and returns the number of successfully closed orders for this unit through the site
        {
            List<BE.Order> OL = dal.GetOrdersListByCond(ord => { return hostU.HostingUnitKey == ord.HostingUnitKey && ord.Status == BE.ORDERSTATUS.CloseByAnswerYes; });
            return OL.Count;
        }

        #region GROUPING
        public IEnumerable<IGrouping<BE.AREA, BE.GuestRequest>> guestRequestsByArea() //List of guest request (grouped) according to the required area.
        {
            IEnumerable<IGrouping<BE.AREA, BE.GuestRequest>> result = from gr in dal.GetGuestRequestsList() group gr by gr.Area into g1 select g1;
            return result;
        }
        public IEnumerable<IGrouping<int, BE.GuestRequest>> guestRequestsByNumofVacationers() // List of guest request (grouped) according to the number of vacationer ( adults + childrens)
        {
            IEnumerable<IGrouping<int, BE.GuestRequest>> result = from gr in dal.GetGuestRequestsList() group gr by gr.Adults + gr.Children into g1 select g1;
            return result;
        }
        public IEnumerable<IGrouping<int, BE.Host>> HostByNumofHostingUnits() // List of hosts (grouped) according to the number of Hosting unit that they hold 
        {
            var temp = dal.GetHostingUnitsList();
            temp=temp.GroupBy(s => s.Owner.HostKey)
                                                 .Select(grp => grp.FirstOrDefault())
                                                 .OrderBy(s => s.Owner.HostKey)
                                                 .ToList();
            IEnumerable<IGrouping<int, BE.Host>> result = from hostingU in temp group hostingU.Owner by NumofHostingUnit(hostingU.Owner) into g1 select g1;
            return result;
        }
        public IEnumerable<IGrouping<BE.AREA, BE.HostingUnit>> HostingUnitbyArea() // List of Hosting units ( grouping) grouped by area required ;
        {
            IEnumerable<IGrouping<BE.AREA, BE.HostingUnit>> result = from hostingU in dal.GetHostingUnitsList() group hostingU by hostingU.Area into g1 select g1;
            return result;
        }

        public IEnumerable<IGrouping<bool, BE.Order>> AllOrders()
        {
            IEnumerable<IGrouping<bool, BE.Order>> result = from order in dal.GetOrdersList() group order by true into g1 select g1;
            return result;
        }
        #endregion

        #region ADD
        public string AddGuestRequest(BE.GuestRequest guestR) // add guest request
        {
            guestR.GuestRequestKey = 0;
            if (!CheckMailAddress(guestR.MailAddress))
                throw new ArgumentException("This email address isn't correct");
            guestR.Status = BE.GUESTSTATUS.Active;
            guestR.RegistrationDate = DateTime.Now;
            if (!CheckDates(guestR.EntryDate, guestR.ReleaseDate))
            {
                throw new ArgumentException("ERROR DATES-Entry date must be before Release Date");
            }
            if (guestR.Vacationers==0)
            {
                throw new ArgumentException("ERROR-You have to add vacationners");
            }
            if (guestR.ReleaseDate>= DateTime.Now.AddYears(1))
            {
                throw new ArgumentException("You cannot register for duration more than year");
            }
            if (Convert.ToInt32(guestR.Area) < 0 || Convert.ToInt32(guestR.Area) > 4)
            {
                throw new ArgumentException("ERROR-This number of Area dosn't exist");
            }
            if (Convert.ToInt32(guestR.Type) < 0 || Convert.ToInt32(guestR.Type) > 3)
            {
                throw new ArgumentException("ERROR-This number of Type dosn't exist");
            }
            if (guestR.Adults < 0 || guestR.Children < 0)
            {
                throw new ArgumentException("ERROR-The number of adults and childrens have to be positive");
            }
            if (Convert.ToInt32(guestR.Pool) < 0 || Convert.ToInt32(guestR.Pool) > 2)
            {
                throw new ArgumentException("ERROR-This number of Choice for the pool dosn't exist");
            }
            if (Convert.ToInt32(guestR.Garden) < 0 || Convert.ToInt32(guestR.Garden) > 2)
            {
                throw new ArgumentException("ERROR-This number of Choice for the garden dosn't exist");
            }
            if (Convert.ToInt32(guestR.Jacuzzi) < 0 || Convert.ToInt32(guestR.Jacuzzi) > 2)
            {
                throw new ArgumentException("ERROR-This number of Choice for the jacuzzi dosn't exist");
            }

            dal.AddGuestRequest(guestR);
            return "Add Guest Request accomplish with success , you will soon receive an email from Hosts\nYour Guest Request key is:\n"+guestR.GuestRequestKey;
        }
        public string AddOrder(int GuestRequestKey, int HostingUnitKey) // add order
        {
            BE.Order newOrd = new BE.Order();
            newOrd.HostingUnitKey = HostingUnitKey;
            var GRkeyList = from GR in dal.GetGuestRequestsList() where GR.GuestRequestKey == GuestRequestKey select GR.GuestRequestKey;
            if (GRkeyList.Count() == 0)
                throw new ArgumentException("ERROR: this Guest Request Key doesn't exist");
            newOrd.GuestRequestKey = GuestRequestKey;
            newOrd.OrderKey = 0;
            newOrd.Status = BE.ORDERSTATUS.NotYetTreated;
            newOrd.CreateDate = DateTime.Now;
            dal.AddOrder(newOrd);
            return " Add Order accomplish with success - Send mail to the client";
        }
        public string AddHostingUnit(BE.HostingUnit hostU) // add hosting unit
        {
            hostU.HostingUnitKey = 0;
            if (!(CheckMailAddress(hostU.Owner.MailAddress)))
                throw new ArgumentException("This email address isn't correct");
            int num;
            if (hostU.Owner.PhoneNumber.Length != 10 || !int.TryParse(hostU.Owner.PhoneNumber,out num))
                throw new ArgumentException("This phone number isn't correct");
            if (hostU.Owner.HostKey.Length != 8 || !int.TryParse(hostU.Owner.HostKey, out num))
            {
                throw new ArgumentException("the ID must be in format of 8 digits");
            }
            if (Convert.ToInt32(hostU.TypeOfHostingUnit) < 0 || Convert.ToInt32(hostU.TypeOfHostingUnit) > 3)
            {
                throw new ArgumentException("ERROR-This number of Type Of Hosting Unit dosn't exist");
            }

            dal.AddHostingUnit(hostU);
            return " Add Hosting Unit accomplish with success \n Your hosting unit key is :" + hostU.HostingUnitKey;
        }
        #endregion

        #region UPDATE
        public string UpdateOrderStatus(BE.Order ord, BE.ORDERSTATUS status) // update order status
        {
            try
            {


                string st = "";
                if (ord.Status == BE.ORDERSTATUS.CloseByAnswerYes || ord.Status == BE.ORDERSTATUS.CloseByAnswerNo || ord.Status == BE.ORDERSTATUS.CloseByLackAnswer)
                {
                    throw new ArgumentException("You can't change the Order Status because it's already close");
                }
                else if (ord.Status == BE.ORDERSTATUS.NotYetTreated && status == BE.ORDERSTATUS.MailSent)
                {
                    Thread thread = new Thread(() => SendMail(ord));
                    thread.Start();
                    //SendMail(ord);
                    st = "mail Sent";
                    ord.OrderDate = DateTime.Now;
                }
                else if (ord.Status == BE.ORDERSTATUS.MailSent && status == BE.ORDERSTATUS.CloseByAnswerYes)
                {
                    ChangeGuestRequestStatus(ord.GuestRequestKey);
                    BE.GuestRequest guestRequest = dal.GetGuestRequestsListByCond(GR => { return GR.GuestRequestKey == ord.GuestRequestKey; }).FirstOrDefault();
                    BE.HostingUnit hostingUnit = dal.GetHostingUnitsListByCond(HU => { return HU.HostingUnitKey == ord.HostingUnitKey; }).FirstOrDefault();
                    st = CollectCommission(guestRequest.EntryDate, guestRequest.ReleaseDate, hostingUnit.Owner);
                    MarkInDiary(hostingUnit.Diary, guestRequest.EntryDate, guestRequest.ReleaseDate);
                    dal.UpdateHostingUnit(hostingUnit);
                    guestRequest.Status = BE.GUESTSTATUS.CloseBySite;
                    dal.UpdateGuestRequest(guestRequest);
                }
                else if (ord.Status == BE.ORDERSTATUS.MailSent && status == BE.ORDERSTATUS.CloseByAnswerNo) { }
                else if (status == BE.ORDERSTATUS.CloseByLackAnswer) { }
                else
                {
                    throw new ArgumentException("You can't change to this Order Status");
                }
                ord.Status = status;
                dal.UpdateStatusOrder(ord);
                return "The update of Order Status succeed\n" + st;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public string UpdateHostingUnit(BE.HostingUnit hu) // update hosting unit
        {
            dal.UpdateHostingUnit(hu);
            return "The update of Hosting Units succeed - You have to update the clients about the changes that you have made";
        }

        public string UpdateGuestRequest(BE.GuestRequest guestRequest) // update guest request 
        {
            dal.UpdateGuestRequest(guestRequest);
            return "The update of Guest Request succeed";
        }
        #endregion

        public bool CheckMailAddress(string mail)  // check if the address mail is correct
        {
            if ((mail.Contains("@") && mail.Contains(".") && mail.IndexOf("@") < mail.LastIndexOf(".")))
                return true;
            return false;
        }
        public BE.HostingUnit hostUnitbyKey(int HostingUnitKey) //return hosting unit according to hosting unit key
        {
            var hostUnitList = dal.GetHostingUnitsListByCond(hostU => { return hostU.HostingUnitKey == HostingUnitKey; });
            if (hostUnitList.Count == 0)
                throw new ArgumentException("This Hosting Unit Key doesn't exist");
            return hostUnitList.FirstOrDefault();
        }
        public List<BE.Order> ListOfOrdersbyHostingUnitKey(int HostingUnitKey) // List of Orders according to the Hosting unit key 
        {
            List<BE.Order> OrdList = dal.GetOrdersListByCond(ord => { return ord.HostingUnitKey == HostingUnitKey; });
            return OrdList;
        }
        public List<BE.BankBranch> bankBranchesList() //// list of bank branches

        {
            return dal.GetBankBranchesList();
        }

        public int NumofHostingUnit(BE.Host owner)
        {
            int i = 0;
            foreach (var v in dal.GetHostingUnitsList())
            {
                if (v.Owner.HostKey == owner.HostKey)
                    i++;
            }
            return i;
        }
        public List<BE.GuestRequest> GuestRequestByHostingUnitOrder(int HostingUnitKey)
        {
            // BE.HostingUnit HostU= dal.GetHostingUnitsListByCond(Hu => { return Hu.HostingUnitKey==HostingUnitKey; }).FirstOrDefault();
            var OrdersGRKeyByHuKey = from Order in ListOfOrdersbyHostingUnitKey(HostingUnitKey) select Order.GuestRequestKey;
            List<int> OrdersGRKeyByHuKeyList = OrdersGRKeyByHuKey.ToList();
            var GuestRequestsByHukey = from GR in dal.GetGuestRequestsList() where OrdersGRKeyByHuKeyList.Find(GRKeyFromList => { return GRKeyFromList == GR.GuestRequestKey; }) != default(int) select GR;
            return GuestRequestsByHukey.ToList();
        }




        /// <summary>
        /// this function check if the num coud be a commision and replace it
        /// </summary>
        /// <param name="num"></param>
        public void Change_Commision(int num)
        {
            try
            {
                if (num <= 0)
                    throw new ArgumentException("Commision must be more than zero");
                BE.Configuration.Commission = num;
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// this function check if the num coud be a Days Expired and replace it
        /// </summary>
        /// <param name="num"></param>
        public void Change_DaysExpiredOrder(int num)
        {
            try
            {
                if (num <= 0)
                    throw new ArgumentException("Days Expire must be more than zero");
                BE.Configuration.DaysExpireOrder = num;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Change_DaysExpiredGuestRequest(int num)
        {
            try
            {
                if (num <= 0)
                    throw new ArgumentException("Days Expire must be more than zero");
                BE.Configuration.DaysExpireOrder = num;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool RemoteFileExists(string url) //check if Link to the internet is exist
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }
}
