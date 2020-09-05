using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this class: 'host' has the details about this person in addition
    /// if he agree to Collection Clearance
    /// </summary>
    [Serializable]//in order to use a deep copy
    public class Host
    {
        public string HostKey { get; set; } // Id of Host
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public BankBranch BankBranchH { get; set; }
        public bool CollectionClearance { get; set; }
        public int BankAccountNumber { get; set; }
        /// <summary>
        /// this function describing the host
        /// </summary>
        /// <returns>
        /// returns a string with the details
        /// </returns>
        public override string ToString()
        {
            return string.Format("Host Key: {0}\nName: {1} {2}\nPhone Number: {3}\nMail Address : {4}\nBank Branch: {5}Collection Clearance: {6}\n",
                HostKey, PrivateName, FamilyName, PhoneNumber, MailAddress, BankBranchH, CollectionClearance);
        }

    }
}
