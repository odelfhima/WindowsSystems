using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// this file has all of the enums of the project
    /// </summary>
    public enum GUESTSTATUS { Active, CloseBySite, CloseExpired }//the status of guest request
    public enum AREA { All, North, South, Center, Jerusalem }//all of the areas of Israel
    public enum TYPE { Zimmer, Hotel, Camping, Tent }//the types of hosting unit
    public enum ORDERSTATUS { NotYetTreated, MailSent, CloseByLackAnswer, CloseByAnswerYes, CloseByAnswerNo }//the status of order
    public enum CHOICE { Possible, Necessary, Uninterested }//3 kinds of choices
}
