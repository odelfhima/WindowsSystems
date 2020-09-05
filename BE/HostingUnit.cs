using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace BE
{
    /// <summary>
    /// this class: 'hosting unit' required all of the details about this hosting unit and her owner.
    /// this details about the hosting unit have match to the detail about the guest request 
    /// in order to do the comparation between them
    /// </summary>
    [Serializable]//in order to use a deep copy
    public class HostingUnit
    {
        public int HostingUnitKey { get; set; }
        public Host Owner { get; set; }
        public AREA Area { get; set; }
        public string HostingUnitName { get; set; }
        [XmlIgnore]
        public bool[,] Diary { get; private set; }
        public TYPE TypeOfHostingUnit { get; set; }
        public bool Pool { get; set; }
        public bool Jacuzzi { get; set; }
        public bool Garden { get; set; }
        [XmlArray("Diary")]
        public bool[] DiaryDto  // mimaarah lematricia 
        {
            get { return Diary.Flatten(); }
            set { Diary = value.Expand(31); } // 31 is the number of roes in the matrix
        }
        /// <summary>
        /// indexer by DateTime
        /// </summary>
        /// <param name="t"></param>
        bool this[DateTime t]
        {
            get { return Diary[t.Day - 1, t.Month - 1]; }
            set { Diary[t.Day - 1, t.Month - 1] = value; }
        }
        /// <summary>
        /// this function describing the hosting unit
        /// </summary>
        /// <returns>
        /// returns a string with the details
        /// </returns>
        public override string ToString()
        {
            return string.Format("Hosting Unit Key: {0}\nOwner:\n {1}\n\nArea: {2}\nHosting Unit Name: {3}\nDiary: {4}\nPool: {5}\nJacuzzi : {6}\nGarden: {7}\n", HostingUnitKey, Owner.ToString(), Area,
                HostingUnitName, PrintDiary(Diary), Pool, Jacuzzi, Garden);
        }

        /// <summary>
        /// this function pass on a diary and enter to a string the bussy days of the diary.
        /// </summary>
        /// <param name="Diary"></param>
        /// <returns>
        /// parameter t - a string that describe all of the bussy days
        /// </returns>
        private string PrintDiary(bool[,] Diary) // return string of the Diary will all the reservations
        {
            string t = "";
            for (DateTime date = new DateTime(2020, 1, 1); date != new DateTime(2020, 12, 31); date = date.AddDays(1))
            {
                if (this[date] == true)
                {
                    if (date.Day == 1 && date.Month == 1)  // the customer enter the first day of the year 
                        t += string.Format("entry date :{0} / {1}    ", date.Day, date.Month);
                    else if (this[date.AddDays(-1)] == false)
                        t += string.Format("entry date :{0} / {1}    ", date.Day, date.Month);
                    if (date.Day == 31 && date.Month == 12) // the customer leaves the last day of the year
                        t += string.Format("exit date :{0} / {1}    ", date.Day, date.Month);
                    else if (this[date.AddDays(1)] == false)
                        t += string.Format("exit date :{0} / {1}    ", date.Day, date.Month) + '\n';
                }
            }
            return t;
        }
    }
}
