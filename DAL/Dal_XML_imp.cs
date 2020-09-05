using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using System.Net;
using BE;
using System.Xml.Serialization;

namespace DAL
{
    /// <summary>
    /// implementation of IDAL by XML files
    /// </summary>
    public class Dal_XML_imp : IDAL
    {
        static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName;//path of xml files
        XElement configRoot;
        XElement BankBranchesRoot;
        private readonly string configPath = ProjectPath + "/Data/config.xml";
        XElement OrdersRoot;
        private readonly string OrdersPath = ProjectPath + "/Data/Orders.xml";
        private readonly string HostingUnitsPath = ProjectPath + "/Data/HostingUnits.xml";
        private readonly string GuestRequestsPath = ProjectPath + "/Data/GuestRequests.xml";
        string xmlLocalPathBB = ProjectPath + "/Data/atm.xml";
        public static List<HostingUnit> HostingUnitsList = new List<BE.HostingUnit>();
        public static List<Order> OrdersList = new List<BE.Order>();
        public static List<GuestRequest> GuestRequestsList = new List<BE.GuestRequest>();
        public System.ComponentModel.BackgroundWorker backgroundWorker1;
        /// <summary>
        /// DAL_XML_imp ctor
        /// </summary>
        public Dal_XML_imp()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            backgroundWorker1.WorkerReportsProgress = true;
            if (!File.Exists(configPath))
            {
                SaveConfigToXml();
            }
            else
            {
                configRoot = XElement.Load(configPath);
                BE.Configuration.GuestRequestKeySt = Convert.ToInt32(configRoot.Element("GuestRequestKeySt").Value);
                BE.Configuration.OrderKeySt = Convert.ToInt32(configRoot.Element("OrderKeySt").Value);
                BE.Configuration.HostingUnitKeySt = Convert.ToInt32(configRoot.Element("HostingUnitKeySt").Value);
                BE.Configuration.Commission = Convert.ToDouble(configRoot.Element("Commission").Value);
                BE.Configuration.DaysExpireOrder = Convert.ToInt32(configRoot.Element("DaysExpireOrder").Value);
                BE.Configuration.DaysExpireGuestRequest = Convert.ToInt32(configRoot.Element("DaysExpireGuestRequest").Value);
                BE.Configuration.LinkCenterAttractions = configRoot.Element("LinkCenterAttractions").Value;
                BE.Configuration.LinkJerusalemAttractions = configRoot.Element("LinkJerusalemAttractions").Value;
                BE.Configuration.LinkNorthAttractions = configRoot.Element("LinkNorthAttractions").Value;
                BE.Configuration.LinkSouthAttractions = configRoot.Element("LinkSouthAttractions").Value;
                BE.Configuration.FromMail = configRoot.Element("FromMail").Value;
                BE.Configuration.PasswordMail = configRoot.Element("PasswordMail").Value;
                BE.Configuration.CurrentProfit = Convert.ToInt32(configRoot.Element("CurrentProfit").Value);
                BE.SiteOwner.profit = BE.Configuration.CurrentProfit;

            }
            if (!File.Exists(OrdersPath))
            {
                OrdersRoot = new XElement("Orders");
                OrdersRoot.Save(OrdersPath);
            }
            if (!File.Exists(GuestRequestsPath))
            {
                SaveToXML<List<GuestRequest>>(new List<BE.GuestRequest>(), GuestRequestsPath);
            }
            if (!File.Exists(HostingUnitsPath))
            {
                SaveToXML<List<HostingUnit>>(new List<BE.HostingUnit>(), HostingUnitsPath);
            }
            //DownloadBankBranches();
            backgroundWorker1.RunWorkerAsync();
            OrdersRoot = XElement.Load(OrdersPath);
            BankBranchesRoot = XElement.Load(xmlLocalPathBB);
            OrdersList = (from Order in OrdersRoot.Elements() select new Order()
            {
                OrderKey = Convert.ToInt32(Order.Element("OrderKey").Value),
                GuestRequestKey = Convert.ToInt32(Order.Element("GuestRequestKey").Value),
                HostingUnitKey = Convert.ToInt32(Order.Element("HostingUnitKey").Value),
                Status = (BE.ORDERSTATUS)Enum.Parse(typeof(BE.ORDERSTATUS), Order.Element("Status").Value, true),
                CreateDate = DateTime.Parse(Order.Element("CreateDate").Value),
                OrderDate = DateTime.Parse(Order.Element("OrderDate").Value)
            }).ToList();
            HostingUnitsList = LoadFromXML<List<BE.HostingUnit>>(HostingUnitsPath);
            GuestRequestsList = LoadFromXML<List<BE.GuestRequest>>(GuestRequestsPath);
        }
        /// <summary>
        /// Save Configuration To Xml
        /// </summary>
        private void SaveConfigToXml()
        {
            configRoot = new XElement("config");
            try
            {
                configRoot.Add(new XElement("GuestRequestKeySt", BE.Configuration.GuestRequestKeySt),
                               new XElement("HostingUnitKeySt", BE.Configuration.HostingUnitKeySt),
                               new XElement("OrderKeySt", BE.Configuration.OrderKeySt),
                               new XElement("Commission", BE.Configuration.Commission),
                               new XElement("CurrentProfit", BE.Configuration.CurrentProfit),
                               new XElement("DaysExpireOrder", BE.Configuration.DaysExpireOrder),
                               new XElement("DaysExpireGuestRequest", BE.Configuration.DaysExpireGuestRequest),
                               new XElement("LinkNorthAttractions", BE.Configuration.LinkNorthAttractions),
                               new XElement("LinkSouthAttractions", BE.Configuration.LinkSouthAttractions),
                               new XElement("LinkCenterAttractions", BE.Configuration.LinkCenterAttractions),
                               new XElement("LinkJerusalemAttractions", BE.Configuration.LinkJerusalemAttractions),
                               new XElement("FromMail", BE.Configuration.FromMail),
                               new XElement("PasswordMail", BE.Configuration.PasswordMail));
                configRoot.Save(configPath);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// dtor
        /// </summary>
        ~Dal_XML_imp()
        {
            OrdersRoot.Save(OrdersPath);
            SaveToXML<List<BE.HostingUnit>>(HostingUnitsList, HostingUnitsPath);
            SaveToXML<List<BE.GuestRequest>>(GuestRequestsList, GuestRequestsPath);
            SaveConfigToXml();
        }


        private void DownloadBankBranches()
        {
            if (!File.Exists(xmlLocalPathBB))
            {
                WebClient wc = new WebClient();
                try
                {
                    string xmlServerPath = @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                    wc.DownloadFile(xmlServerPath, xmlLocalPathBB);
                }
                catch (Exception)
                {
                    string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                    wc.DownloadFile(xmlServerPath, xmlLocalPathBB);
                }
                finally
                {
                    wc.Dispose();
                }
            }
        }

        //its not download everytime its strting the program because its not necessary. bank branches are not changing too fast.
        // once in year we will do that.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try {
                DownloadBankBranches();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
            private void backgroundWorker1_ProgressChanged(object sender, DoWorkEventArgs e)
        {
            try
            {
                DownloadBankBranches();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Save To XML tamplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source);
            file.Close();
        }

        /// <summary>
        /// Load From XML tamplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }

        public static void saveListToXML<T>(List<T> list, string path)
        {
            XmlSerializer x = new XmlSerializer(list.GetType());
            FileStream fs = new FileStream(path, FileMode.Create);
            x.Serialize(fs, list);
           fs.Close();
        }

        public static List<T> loadListFromXML<T>(string path)
        {
            List<T> list;
            XmlSerializer x = new XmlSerializer(typeof(List<T>));
            FileStream fs = new FileStream(path, FileMode.Open);
            list = (List<T>)x.Deserialize(fs);
            fs.Close();
            return list;

        }

        /// <summary>
        /// Add Guest Request
        /// </summary>
        /// <param name="G"></param>
        public void AddGuestRequest(BE.GuestRequest G)
        {
            if (G.GuestRequestKey == 0)
            {
                BE.Configuration.GuestRequestKeySt++;
                G.GuestRequestKey = BE.Configuration.GuestRequestKeySt;
            }
            GuestRequestsList.Add(G.Copy());
            SaveToXML<List<BE.GuestRequest>>(GuestRequestsList, GuestRequestsPath);
            SaveConfigToXml();
        }


        /// <summary>
        /// Update Guest Request
        /// </summary>
        /// <param name="G"></param>
        public void UpdateGuestRequest(BE.GuestRequest G)
        {
            try
            {
                int index = GuestRequestsList.FindIndex(t => t.GuestRequestKey == G.GuestRequestKey);
                if (index == -1)
                    throw new MissingException("GuestRequestKey");
                GuestRequestsList[index] = G.Copy();
                SaveToXML<List<BE.GuestRequest>>(GuestRequestsList, GuestRequestsPath);
            }
            catch (MissingException e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Add Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        public void AddHostingUnit(BE.HostingUnit H)
        {
            if (H.HostingUnitKey == 0)
            {
                BE.Configuration.HostingUnitKeySt++;
                H.HostingUnitKey = BE.Configuration.HostingUnitKeySt;
                H.DiaryDto = new bool[31 * 12];
            }
            HostingUnitsList.Add(H.Copy());
            SaveToXML<List<BE.HostingUnit>>(HostingUnitsList, HostingUnitsPath);
            SaveConfigToXml();
        }



        /// <summary>
        /// Remove Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        public void RemoveHostingUnit(BE.HostingUnit H)
        {
            int count=HostingUnitsList.RemoveAll(HostU => HostU.HostingUnitKey == H.HostingUnitKey);
            if(count==0)
                throw new MissingException("HostingUnitKey");
            SaveToXML<List<BE.HostingUnit>>(HostingUnitsList, HostingUnitsPath);
        }



        /// <summary>
        /// Update Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        public void UpdateHostingUnit(BE.HostingUnit H)
        {
            try
            {
                int index = HostingUnitsList.FindIndex(t => t.HostingUnitKey == H.HostingUnitKey);
                if (index == -1)
                    throw new MissingException("HostingUnitKey");
                HostingUnitsList[index] = H.Copy();
                SaveToXML<List<BE.HostingUnit>>(HostingUnitsList, HostingUnitsPath);
            }
            catch (MissingException e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Add Order
        /// </summary>
        /// <param name="Ord"></param>
        public void AddOrder(BE.Order Ord)
        {
            try
            {
                if (Ord.OrderKey == 0)
                {
                    BE.Configuration.OrderKeySt++;
                    Ord.OrderKey = BE.Configuration.OrderKeySt;
                }
                XElement t = new XElement("Order");
                t.Add(new XElement("OrderKey", Ord.OrderKey),
                      new XElement("HostingUnitKey", Ord.HostingUnitKey),
                      new XElement("GuestRequestKey", Ord.GuestRequestKey),
                      new XElement("Status", Ord.Status),
                      new XElement("CreateDate", Ord.CreateDate.ToString()),
                      new XElement("OrderDate", Ord.OrderDate.ToString()));
                OrdersRoot.Add(t);
                OrdersRoot.Save(OrdersPath);
                OrdersList.Add(Ord);
                SaveConfigToXml();
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// Update Status Order
        /// </summary>
        /// <param name="Ord"></param>
        public void UpdateStatusOrder(BE.Order Ord)
        {
            try
            {
                XElement t = (from order in OrdersRoot.Elements()
                              where int.Parse(order.Element("OrderKey").Value) == Ord.OrderKey
                              select order).FirstOrDefault();
                //t.Element("OrderKey").Value = Ord.OrderKey.ToString();
                //t.Element("HostingUnitKey").Value = Ord.HostingUnitKey.ToString();
                //t.Element("GuestRequestKey").Value = Ord.GuestRequestKey.ToString();
                t.Element("Status").Value = Ord.Status.ToString();
                //t.Element("CreateDate").Value = Ord.CreateDate.ToString();
                t.Element("OrderDate").Value = Ord.OrderDate.ToString();
                OrdersRoot.Save(OrdersPath);
                int count = OrdersList.RemoveAll(Ord2 => Ord2.OrderKey == Ord.OrderKey);
                if (count == 0)
                    throw new MissingException("OrderKey");
                OrdersList.Add(Ord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Get Hosting Units List from the data source 
        /// </summary>
        /// <returns>Hosting Units List</returns>
        public List<BE.HostingUnit> GetHostingUnitsList()
        {
            try
            {
                return loadListFromXML<HostingUnit>(HostingUnitsPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Get Orders List from the data source 
        /// </summary>
        /// <returns>Orders List </returns>
        public  List<BE.Order> GetOrdersList()
        {

            return OrdersList;
        }


        /// <summary>
        /// Get Guest Requests List from the data source 
        /// </summary>
        /// <returns>Guest Requests List</returns>
        public List<BE.GuestRequest> GetGuestRequestsList()
        {
            try
            {
                return loadListFromXML<GuestRequest>(GuestRequestsPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Get Bank Branches List from the data source 
        /// </summary>
        /// <returns>Bank Branches List</returns>
        public List<BE.BankBranch> GetBankBranchesList()
        {
            try
            {
                //IEnumerable<BE.BankBranch> BBI = from BB in DS.DataSource.BankBranches select BB.Copy();
                //List<BE.BankBranch> BBL = BBI.ToList();
                //return BBL;
                var it = (from item in BankBranchesRoot.Elements()
                          select new BE.BankBranch()
                          {
                              BankName = item.Element("שם_בנק").Value,
                              BankNumber = Convert.ToInt32(item.Element("קוד_בנק").Value),
                              BranchNumber = Convert.ToInt32(item.Element("קוד_סניף").Value),
                              BranchAddress = item.Element("כתובת_ה-ATM").Value,
                              BranchCity = item.Element("ישוב").Value
                          }).ToList();
                var x = it.GroupBy(s => s.BankName).Select(grp => grp.FirstOrDefault()).OrderBy(s => s.BankName).ToList();
                return x;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        ///Get Hosting Units List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Hosting Units List</returns>
        public List<BE.HostingUnit> GetHostingUnitsListByCond(Predicate<BE.HostingUnit> cond)
        {
            var v = loadListFromXML<BE.HostingUnit>(HostingUnitsPath);
            return (from HostU in v where cond(HostU) select HostU.Copy()).ToList();
        }



        /// <summary>
        ///Get Orders List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Orders List</returns>
        public List<BE.Order> GetOrdersListByCond(Predicate<BE.Order> cond)
        {
            return (from Order in GetOrdersList() where cond(Order)
                   select Order).ToList();
        }



        /// <summary>
        /// Get Guest Requests List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Guest Requests List</returns>
        public List<BE.GuestRequest> GetGuestRequestsListByCond(Predicate<BE.GuestRequest> cond)
        {
            var v = loadListFromXML<BE.GuestRequest>(GuestRequestsPath);
            return (from GR in v where cond(GR) select GR.Copy()).ToList();
        }
    }
}
