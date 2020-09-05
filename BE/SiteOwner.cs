using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this class: 'Site owner' has the commission that the site owner has earned until now
    /// and his password to the site. the Site owner can see the lists of the data of the site.
    /// like list of order, the list of the guest request etc.
    /// </summary>
    static public class SiteOwner
    {
        static private double profitP=0;

        static public double profit
        {
            get { return profitP; }
            set { profitP= value; }
        }

        static public string password { get; set; } // password of the owner of the site 
    }
}
