using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// this class : FactoryBL creating a BL_imp variable to have an access to the function that in the BL from PL 
    /// </summary>
    public class FactoryBL
    {
        /// <summary>
        /// creating a BL_imp variable
        /// </summary>
        /// <returns>
        /// a BL_imp variable
        /// </returns>
        public static IBL GetBL()
        {
            return new BL_imp();
        }
    }
}
