using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this classs is presenting a Bank Branch- with all of the details.
    /// </summary>
    [Serializable]//in order to use a deep copy
    public class BankBranch
    {
        public int BankNumber { get; set; }
        public string BankName { get; set; }
        public int BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        /// <summary>
        /// this function describing the Bank Branch
        /// </summary>
        /// <returns>
        /// returns a string with the details
        /// </returns>
        public override string ToString()
        {
            return string.Format("Bank Number: {0}\nBank Name: {1}\nBranch Number : {2}\nBranch Address: {3}\nBranch City: {4}\n",
                BankNumber, BankName, BranchNumber, BranchAddress, BranchCity);
        }
    }
}
