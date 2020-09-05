using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Threading;

namespace BL
{
    public interface IBL
    {
        /// <summary>
        ///  check if this is in year 2020 and if the Date enter is before the Date exit at least one day
        /// </summary>
        /// <param name="D1"></param>
        /// <param name="D2"></param>
        /// <returns>bool - true or false</returns>
        bool CheckDates(DateTime D1, DateTime D2);



        /// <summary>
        /// When creating a customer order, make sure that the requested dates are available in the hosting unit offered to them.
        /// </summary>
        /// <param name="Denter"></param>
        /// <param name="Dexit"></param>
        /// <param name="Diary"></param>
        /// <returns>true or false </returns>
        bool CheckInDairy(DateTime Denter, DateTime Dexit, bool[,] Diary);


        /// <summary>
        /// When booking status changes due to closing of transaction - a taxe of 10 NIS  per accommodation must be calculated.
        /// </summary>
        /// <param name="Denter"></param>
        /// <param name="Dexit"></param>
        /// <param name="host"></param>
        /// <returns>return how much it costs</returns>
        string CollectCommission(DateTime Denter, DateTime Dexit, BE.Host host);



        /// <summary>
        ///  When order status changes due to transaction closing - the relevant dates must be marked in the matrix ( diary)
        /// </summary>
        /// <param name="Dairy"></param>
        /// <param name="Denter"></param>
        /// <param name="Dexit"></param>
        void MarkInDiary(bool[,] Dairy, DateTime Denter, DateTime Dexit);


        /// <summary>
        /// When order status changes due to transaction closing - the status of guest request must be changed accordingly 
        /// </summary>
        /// <param name="GuestRequestKey"></param>
        void ChangeGuestRequestStatus(int GuestRequestKey);



        /// <summary>
        /// You can not delete a hosting unit as long as there is an offer associated with it in open mode
        /// </summary>
        /// <param name="hu"></param>
        /// <returns> return string if the action was done with succes or not </returns>
        string DeleteHostingUnit(BE.HostingUnit hu);


        /// <summary>
        /// When the order status changes to "Sent Mail" the system will automatically send a mail to the customer with the order details
        /// </summary>
        /// <param name="ord"></param>
        void SendMail(BE.Order ord);


        /// <summary>
        /// A function that accepts a date and number of vacation days and returns the list of all available hosting units on that date.
        /// </summary>
        /// <param name="Denter"></param>
        /// <param name="duration"></param>
        /// <returns>return the list of all available hosting units on that date</returns>
        List<BE.HostingUnit> ListofHostingUnitNotOccupied(DateTime Denter, int duration);




        /// <summary>
        /// This function receives one or two dates
        /// </summary>
        /// <param name="Dates"></param>
        /// <returns> return the number of days that have passed from the first date to the second,
        /// or if she receives only one date - from the first date to the present day</returns>
        int CalculateDate(params DateTime[] Dates);



        /// <summary>
        ///  list of orders according to the days passed
        /// </summary>
        /// <param name="days"></param>
        /// <returns>OrdList</returns>
        List<BE.Order> ListOrdersByDaysPassed(int days);


        /// <summary>
        /// function that can return all customer requirements that matching to a particular condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>GRL</returns>
        List<BE.GuestRequest> GuestRequestByCondition(Predicate<BE.GuestRequest> condition);


        /// <summary>
        /// function that can return all Hosting Units that matching to a particular condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>list of Hosting unit</returns>
        List<BE.HostingUnit> HostingUnitByCondition(Predicate<BE.HostingUnit> condition);


        /// <summary>
        /// function that can return all Orders that matching to a particular condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>list of orders</returns>
        List<BE.Order> OrdersByCondition(Predicate<BE.Order> condition);


        /// <summary>
        /// A function that receive guest request and returns the number of orders sent to it
        /// </summary>
        /// <param name="guestr"></param>
        /// <returns> return the number of orders sent to it</returns>
        int NumofOrders(BE.GuestRequest guestr);



        /// <summary>
        /// A function that accepts a hosting unit and returns the number of successfully closed orders for this unit through the site
        /// </summary>
        /// <param name="hostU"></param>
        /// <returns> return the number of successfully closed orders for this unit through the site</returns>
        int NumofOrdersClosebySucces(BE.HostingUnit hostU);

        #region Grouping
        /// <summary>
        /// List of guest request (grouped) according to the required area.
        /// </summary>
        /// <returns>result (grouping)</returns>
        IEnumerable<IGrouping<BE.AREA, BE.GuestRequest>> guestRequestsByArea();


        
        /// <summary>
        /// One group with all orders(grouping)
        /// </summary>
        /// <returns>result</returns>
        IEnumerable<IGrouping<bool, BE.Order>> AllOrders();


        /// <summary>
        /// List of guest request (grouped) according to the number of vacationer
        /// </summary>
        /// <returns>result (grouping)</returns>
        IEnumerable<IGrouping<int, BE.GuestRequest>> guestRequestsByNumofVacationers();


        /// <summary>
        /// List of hosts (grouped) according to the number of Hosting unit that they hold 
        /// </summary>
        /// <returns>result (grouping)</returns>
        IEnumerable<IGrouping<int, BE.Host>> HostByNumofHostingUnits();



        /// <summary>
        /// List of Hosting units ( grouping) grouped by area required 
        /// </summary>
        /// <returns>result (grouping)</returns>
        IEnumerable<IGrouping<BE.AREA, BE.HostingUnit>> HostingUnitbyArea();


        #endregion

        #region ADD
        /// <summary>
        /// Add Guest Request
        /// </summary>
        /// <param name="guestR"></param>
        /// <returns>string if this action accomplish with success</returns>
        string AddGuestRequest(BE.GuestRequest guestR);


        /// <summary>
        /// Add Order
        /// </summary>
        /// <param name="GuestRequestKey"></param>
        /// <param name="HostingUnitKey"></param>
        /// <returns>string if this action accomplish with success</returns>
        string AddOrder(int GuestRequestKey, int HostingUnitKey);


        /// <summary>
        /// Add Hosting Unit
        /// </summary>
        /// <param name="hostU"></param>
        /// <returns>string if this action accomplish with success</returns>
        string AddHostingUnit(BE.HostingUnit hostU);


        #endregion


        #region Update
        /// <summary>
        /// Update Order Status
        /// </summary>
        /// <param name="ord"></param>
        /// <param name="status"></param>
        /// <returns>string if this action accomplish with success</returns>
        string UpdateOrderStatus(BE.Order ord, BE.ORDERSTATUS status);


        /// <summary>
        /// Update Hosting Unit
        /// </summary>
        /// <param name="hu"></param>
        /// <returns>string if this action accomplish with success</returns>
        string UpdateHostingUnit(BE.HostingUnit hu);




        /// <summary>
        /// Update Guest Request
        /// </summary>
        /// <param name="guestRequest"></param>
        /// <returns>string if this action accomplish with success</returns>
        string UpdateGuestRequest(BE.GuestRequest guestRequest);

        #endregion


        /// <summary>
        /// check if the mail address contains @ and at least one point after
        /// </summary>
        /// <param name="mail"></param>
        /// <returns>true or false</returns>
        bool CheckMailAddress(string mail);


        /// <summary>
        ///  return hosting unit according to hosting unit key
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns> The Hosting Unit accordingto the hosting unit key</returns>
        BE.HostingUnit hostUnitbyKey(int HostingUnitKey);


        /// <summary>
        /// List of Orders according to the Hosting unit key 
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns>list of Orders</returns>
        List<BE.Order> ListOfOrdersbyHostingUnitKey(int HostingUnitKey);


        /// <summary>
        /// list of bank branches
        /// </summary>
        /// <returns>bank Branches List</returns>
        List<BE.BankBranch> bankBranchesList();



        /// <summary>
        ///  help fonction for hostbyhostingUnit
        /// </summary>
        /// <param name="owner"></param>
        /// <returns>Num of Hosting Unit</returns>
        int NumofHostingUnit(BE.Host owner);



        /// <summary>
        ///GuestRequest By Hosting Unit Orders
        /// </summary>
        /// <param name="HostingUnitKey"></param>
        /// <returns> list of guest request that are in hosting unit orders </returns>
        List<BE.GuestRequest> GuestRequestByHostingUnitOrder(int HostingUnitKey);



        /// <summary>
        /// Guest Request By Condition
        /// </summary>
        /// <param name="hostU"></param>
        /// <returns> list of guest request according to the condition </returns>
        List<BE.GuestRequest> GuestRequestByCondition(BE.HostingUnit hostU);


        /// <summary>
        /// this function check if the num coud be a commision and replace it
        /// </summary>
        /// <param name="num"></param>
        void Change_Commision(int num);


        /// <summary>
        /// this function check if the num coud be a Days Expired and replace it
        /// </summary>
        /// <param name="num"></param>
        void Change_DaysExpiredOrder(int num);


        /// <summary>
        /// this function check if the num coud be a Days Expired and replace it
        /// </summary>
        /// <param name="num"></param>
        void Change_DaysExpiredGuestRequest(int num);


        /// <summary>
        /// check if Link to the internet is exist
        /// </summary>
        /// <param name="url"></param>
        /// <returns>True : If the file exits, False if file not exists</returns>
        bool RemoteFileExists(string url);
  
    }
}