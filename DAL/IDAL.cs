using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IDAL
    {
        /// <summary>
        /// Add Guest Request
        /// </summary>
        /// <param name="G"></param>
        void AddGuestRequest(BE.GuestRequest G);


        /// <summary>
        /// Update Guest Request
        /// </summary>
        /// <param name="G"></param>
        void UpdateGuestRequest(BE.GuestRequest G);


        /// <summary>
        /// Add Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        void AddHostingUnit(BE.HostingUnit H);



        /// <summary>
        /// Remove Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        void RemoveHostingUnit(BE.HostingUnit H);



        /// <summary>
        /// Update Hosting Unit
        /// </summary>
        /// <param name="H"></param>
        void UpdateHostingUnit(BE.HostingUnit H);


        /// <summary>
        /// Add Order
        /// </summary>
        /// <param name="Ord"></param>
        void AddOrder(BE.Order Ord);



        /// <summary>
        /// Update Status Order
        /// </summary>
        /// <param name="Ord"></param>
        void UpdateStatusOrder(BE.Order Ord);


        /// <summary>
        /// Get Hosting Units List from the data source 
        /// </summary>
        /// <returns>Hosting Units List</returns>
        List<BE.HostingUnit> GetHostingUnitsList();


        /// <summary>
        /// Get Orders List from the data source 
        /// </summary>
        /// <returns>Orders List </returns>
        List<BE.Order> GetOrdersList();


        /// <summary>
        /// Get Guest Requests List from the data source 
        /// </summary>
        /// <returns>Guest Requests List</returns>
        List<BE.GuestRequest> GetGuestRequestsList();


        /// <summary>
        /// Get Bank Branches List from the data source 
        /// </summary>
        /// <returns>Bank Branches List</returns>
        List<BE.BankBranch> GetBankBranchesList();


        /// <summary>
        ///Get Hosting Units List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Hosting Units List</returns>
        List<BE.HostingUnit> GetHostingUnitsListByCond(Predicate<BE.HostingUnit> cond);



        /// <summary>
        ///Get Orders List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Orders List</returns>
        List<BE.Order> GetOrdersListByCond(Predicate<BE.Order> cond);



        /// <summary>
        /// Get Guest Requests List from the data source by condition
        /// </summary>
        /// <param name="cond"></param>
        /// <returns>Guest Requests List</returns>
        List<BE.GuestRequest> GetGuestRequestsListByCond(Predicate<BE.GuestRequest> cond);
    }
}
