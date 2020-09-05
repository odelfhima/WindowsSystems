using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// we use a factory method and at the same time a singleton method to be sure that we do not create a new instance of the DAL every time.
    /// </summary>
    static public class FactoryDal
    {
        static IDAL instance = null;
        public static IDAL GetInstance()
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }
    }
}
