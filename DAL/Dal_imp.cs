using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Dal_imp : IDAL
    {
        public void AddGuestRequest(BE.GuestRequest G)
        {
            if (G.GuestRequestKey == 0)
                G.GuestRequestKey = BE.Configuration.GuestRequestKeySt;
            DS.DataSource.GuestRequestsList.Add(G.Copy());
        }
        public void UpdateGuestRequest(BE.GuestRequest G)
        {
            try
            {
                int count = DS.DataSource.GuestRequestsList.RemoveAll(Gst => G.GuestRequestKey == Gst.GuestRequestKey);
                if (count == 0)
                    throw new MissingException("GuestRequestKey");
                AddGuestRequest(G);
            }
            catch (MissingException e)
            {
                throw e;
            }
        }
        public void AddHostingUnit(BE.HostingUnit H)
        {
            if (H.HostingUnitKey == 0)
            {
                H.HostingUnitKey = BE.Configuration.HostingUnitKeySt;
                H.DiaryDto = new bool[31*12];
               //H.Diary=new bool[31 , 12];
            }
               
            DS.DataSource.HostingUnitsList.Add(H.Copy());
        }
        public void RemoveHostingUnit(BE.HostingUnit H)
        {
            DS.DataSource.HostingUnitsList.RemoveAll(HostU => HostU.HostingUnitKey == H.HostingUnitKey);
        }
        public void UpdateHostingUnit(BE.HostingUnit H)
        {
            try
            {
                int count = DS.DataSource.HostingUnitsList.RemoveAll(HostU => HostU.HostingUnitKey == H.HostingUnitKey);
                if (count == 0)
                    throw new MissingException("HostingUnitKey");
                AddHostingUnit(H);
            }
            catch (MissingException e)
            {
                throw e;
            }
        }
        public void AddOrder(BE.Order Ord)
        {
            if (Ord.OrderKey == 0)
                Ord.OrderKey = BE.Configuration.OrderKeySt;
            DS.DataSource.OrdersList.Add(Ord.Copy());
        }
        public void UpdateStatusOrder(BE.Order Ord)
        {
            try
            {
                int count = DS.DataSource.OrdersList.RemoveAll(Ord2 => Ord2.OrderKey == Ord.OrderKey);
                if (count == 0)
                    throw new MissingException("OrderKey");
                AddOrder(Ord);
            }
            catch (MissingException e)
            {
                throw e;
            }
        }
        public List<BE.HostingUnit> GetHostingUnitsList()
        {
            IEnumerable<BE.HostingUnit> HUI = from HostU in DS.DataSource.HostingUnitsList select HostU.Copy();
            List<BE.HostingUnit> HUL = HUI.ToList();
            return HUL;
        }

        public List<BE.Order> GetOrdersList()
        {
            IEnumerable<BE.Order> OI = from ord in DS.DataSource.OrdersList select ord.Copy();
            List<BE.Order> OL = OI.ToList();
            return OL;
        }
        public List<BE.GuestRequest> GetGuestRequestsList()
        {
            IEnumerable<BE.GuestRequest> GRI = from GR in DS.DataSource.GuestRequestsList select GR.Copy();
            List<BE.GuestRequest> GRL = GRI.ToList();
            return GRL;
        }

        public List<BE.BankBranch> GetBankBranchesList()
        {
            IEnumerable<BE.BankBranch> BBI = from BB in DS.DataSource.BankBranches select BB.Copy();
            List<BE.BankBranch> BBL = BBI.ToList();
            return BBL;
        }

        public List<BE.HostingUnit> GetHostingUnitsListByCond(Predicate<BE.HostingUnit> cond)
        {
            IEnumerable<BE.HostingUnit> HUI = from HostU in DS.DataSource.HostingUnitsList where cond(HostU) select HostU.Copy();
            List<BE.HostingUnit> HUL = HUI.ToList();
            return HUL;
        }
        public List<BE.Order> GetOrdersListByCond(Predicate<BE.Order> cond)
        {
            IEnumerable<BE.Order> OI = from ord in DS.DataSource.OrdersList where cond(ord) select ord.Copy();
            List<BE.Order> OL = OI.ToList();
            return OL;
        }
        public List<BE.GuestRequest> GetGuestRequestsListByCond(Predicate<BE.GuestRequest> cond)
        {
            IEnumerable<BE.GuestRequest> GRI = from GR in DS.DataSource.GuestRequestsList where cond(GR) select GR.Copy();
            List<BE.GuestRequest> GRL = GRI.ToList();
            return GRL;
        }
    }
}
