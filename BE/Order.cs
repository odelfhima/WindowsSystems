using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this class: 'order' required the details about the order that the owner of 
    /// the hosting unit did about one of the guest request and what hapenning now with the order.
    /// </summary>
    [Serializable]//in order to use a deep copy
    public class Order
    {
        public int OrderKey { get; set; }
        public int HostingUnitKey { get; set; }
        public int GuestRequestKey { get; set; }
        public ORDERSTATUS Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// this function describing the order
        /// </summary>
        /// <returns>
        /// returns a string with the details
        /// </returns>
        public override string ToString()
        {
            return string.Format("Hosting Unit Key: {0}\nGuest Request Key: {1}\nOrder Key: {2}\nStatus: {3}\nCreate Date: {4}\nOrder Date: {5}\n",
                HostingUnitKey, GuestRequestKey, OrderKey, Status, (CreateDate.ToString()).Remove(10), (OrderDate.ToString()).Remove(10));
        }
    }
}
