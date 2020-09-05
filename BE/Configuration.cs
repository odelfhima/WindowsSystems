using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
 /// <summary>
 /// this class: 'configuration' has all of the static variable and the running seerial number
 /// </summary>
    public static class Configuration
    {

        static private int GuestRequestKeyStP=10000000;

        static public int GuestRequestKeySt
        {
            get
            {
                return GuestRequestKeyStP;
            }
            set
            {
                GuestRequestKeyStP = value;
            }
        }
        static private int HostingUnitKeyStP=10000000;

        static public int HostingUnitKeySt
        {
            get
            { 
                return HostingUnitKeyStP;
            }
            set { HostingUnitKeyStP = value; }
        }
        static private int OrderKeyStP=10000000;

        static public int OrderKeySt
        {
            get
            {
                
                return OrderKeyStP;
            }
            set { OrderKeyStP = value; }
        }

        static public double Commission=10;
        static public double CurrentProfit=0;
        static public int DaysExpireGuestRequest=60;
        static public int DaysExpireOrder=60;
        #region Link for Attractions
        static public string LinkNorthAttractions= "https://www.aquakef.com/";
        static public string LinkSouthAttractions= "http://www.parktimna.co.il/";
        static public string LinkCenterAttractions= "http://www.lunapark.co.il/";
        static public string LinkJerusalemAttractions= "https://www.jerusalemzoo.org/";
        #endregion
        static public string FromMail="adi.giladi1@gmail.com";
        static public string PasswordMail="maroon 5";

    }
}
