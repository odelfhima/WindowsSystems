using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this class has all of the detail about guest request-details about the client 
    /// and details about the vication.
    /// </summary>
    [Serializable]//in order to use a deep copy
    public class GuestRequest
    {
        public int GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public GUESTSTATUS Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public AREA Area { get; set; }
        public TYPE Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public CHOICE Pool { get; set; }
        public CHOICE Jacuzzi { get; set; }
        public CHOICE Garden { get; set; }
        public int Vacationers
        {
            get { return Adults+Children; }
        }
        /// <summary>
        /// this function describing the guest request
        /// </summary>
        /// <returns>
        /// returns a string with the details
        /// </returns>
        public override string ToString()
        {
            //string result = " מספר הבקשה לאירוח:  " + GuestRequestKey + '\n' + "שם הלקוח: " + PrivateName + '\n' + ' ' + FamilyName + '\n' + "מייל: " + MailAddress + '\n' + " סטטוס הבקשה: " + Status + '\n' +
            //    "תאריך רישום למערכת: " + (RegistrationDate.ToString()).Remove(10) + '\n' + "תאריך רצוי לתחילת הנופש: " + (EntryDate.ToString()).Remove(10) + '\n' + "תאריך רצוי לסיום הנופש: " + (ReleaseDate.ToString()).Remove(10) + '\n' +
            //    "אזור הנופש הרצוי בארץ: " + Area + '\n' + "סוג יחידת האירוח הרצוי: " + Type + '\n' + "מספר המבוגרים: " + Adults + '\n' + "מספר ילדים: " + Children + '\n' +
            //    "בריכה: " + Pool + '\n' + "ג'קוזי: " + Jacuzzi + '\n' + " גינה: " + Garden + '\n';
            string result = "Guest Request Key:  " + GuestRequestKey + '\n' + "Name: " + PrivateName + ' ' + FamilyName + '\n' + "Mail Address: " + MailAddress + '\n' + " Status: " + Status + '\n' +
                "Registration Date: " + (RegistrationDate.ToString()).Remove(10) + '\n' + "Entry Date: " + (EntryDate.ToString()).Remove(10) + '\n' + "Release Date: " + (ReleaseDate.ToString()).Remove(10) + '\n' +
                "Area: " + Area + '\n' + "Type of Hosting Unit: " + Type + '\n' + "Adults: " + Adults + '\n' + " Children: " + Children + '\n' +
                " Pool: " + Pool + '\n' + "Jacuzzi: " + Jacuzzi + '\n' + "Garden: " + Garden + '\n';
            return result;
        }
    }
}
