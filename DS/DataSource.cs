using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DS
{
    public static class DataSource
    {

        //public static List<GuestRequest> GuestRequestsList = new List<GuestRequest>()
        //{
        //    new GuestRequest()
        //    {

        //         GuestRequestKey= 99999999,
        //         PrivateName= "efrat",
        //         FamilyName="cohen",
        //         MailAddress="efrat.cohen@gmail.com",
        //         Status = BE.GUESTSTATUS.Active,
        //         RegistrationDate= new DateTime(2020,08,06),
        //         EntryDate=new DateTime (2020,09,06),
        //         ReleaseDate= new DateTime(2020,09,10),
        //         Area=BE.AREA.Center ,
        //         Type= BE.TYPE.Hotel,
        //         Adults=2,
        //         Children=7,
        //         Pool=BE.CHOICE.Necessary,
        //         Jacuzzi=BE.CHOICE.Possible,
        //         Garden=BE.CHOICE.Possible,
        //         ChildrenAttractions=BE.CHOICE.Necessary
        //    },

        //    new GuestRequest()
        //    {

        //        GuestRequestKey= 99999998,
        //         PrivateName= "adi",
        //         FamilyName="giladi",
        //         MailAddress="adi.giladi1@gmail.com",
        //         Status = BE.GUESTSTATUS.Active,
        //         RegistrationDate= new DateTime(2020,04,06),
        //         EntryDate=new DateTime (2020,08,06),
        //         ReleaseDate= new DateTime(2020,08,10),
        //         Area=BE.AREA.North ,
        //         Type= BE.TYPE.Zimmer,
        //         Adults=2,
        //         Children=10,
        //         Pool=BE.CHOICE.Necessary,
        //         Jacuzzi=BE.CHOICE.Necessary,
        //         Garden=BE.CHOICE.Necessary,
        //         ChildrenAttractions=BE.CHOICE.Possible
        //    },

        //    new GuestRequest()
        //    {

        //        GuestRequestKey= 99999997,
        //         PrivateName= "odel",
        //         FamilyName="fhima",
        //         MailAddress="odel.fhima2@gmail.com",
        //         Status = BE.GUESTSTATUS.Active,
        //         RegistrationDate= new DateTime(2020,10,06),
        //         EntryDate=new DateTime (2020,12,06),
        //         ReleaseDate= new DateTime(2020,12,10),
        //         Area=BE.AREA.South ,
        //         Type= BE.TYPE.Hotel,
        //         Adults=2,
        //         Children=7,
        //         Pool=BE.CHOICE.Necessary,
        //         Jacuzzi=BE.CHOICE.Necessary,
        //         Garden=BE.CHOICE.Uninterested,
        //         ChildrenAttractions=BE.CHOICE.Possible
        //    },

        //    new GuestRequest()
        //    {

        //        GuestRequestKey= 99999996,
        //         PrivateName= "oshri",
        //         FamilyName="cohen",
        //         MailAddress="oshri.cohen@gmail.com",
        //         Status = BE.GUESTSTATUS.CloseExpired,
        //         RegistrationDate= new DateTime(2020,02,06),
        //         EntryDate=new DateTime (2020,06,06),
        //         ReleaseDate= new DateTime(2020,06,10),
        //         Area=BE.AREA.Jerusalem ,
        //         Type= BE.TYPE.Hotel,
        //         Adults=2,
        //         Children=9,
        //         Pool=BE.CHOICE.Uninterested,
        //         Jacuzzi=BE.CHOICE.Possible,
        //         Garden=BE.CHOICE.Necessary,
        //         ChildrenAttractions=BE.CHOICE.Necessary
        //    },

        //    new GuestRequest()
        //    {

        //         GuestRequestKey= 99999995,
        //         PrivateName= "bibi",
        //         FamilyName="Ben-Gurion",
        //         MailAddress="bibi.bengurion.1@gmail.com",
        //         Status = BE.GUESTSTATUS.Active,
        //         RegistrationDate= new DateTime(2020,05,06),
        //         EntryDate=new DateTime (2020,07,06),
        //         ReleaseDate= new DateTime(2020,07,10),
        //         Area=BE.AREA.All ,
        //         Type= BE.TYPE.Tent,
        //         Adults=2,
        //         Children=3,
        //         Pool=BE.CHOICE.Uninterested,
        //         Jacuzzi=BE.CHOICE.Uninterested,
        //         Garden=BE.CHOICE.Necessary,
        //         ChildrenAttractions=BE.CHOICE.Uninterested
        //    }
        //};
        //public static List<Order> OrdersList = new List<Order>()
        //{
        //    new Order()
        //    {
        //        HostingUnitKey=HostingUnitsList[0].HostingUnitKey,
        //        GuestRequestKey=GuestRequestsList[2].GuestRequestKey,
        //        OrderKey=9999999,
        //        Status=BE.ORDERSTATUS.MailSent,
        //        CreateDate= new DateTime(2020,10,08),
        //        OrderDate=new DateTime(2020,10,09)
        //    },

        //    new Order()
        //    {
        //        HostingUnitKey=HostingUnitsList[1].HostingUnitKey,
        //        GuestRequestKey=GuestRequestsList[1].GuestRequestKey,
        //        OrderKey=99999998,
        //        Status=BE.ORDERSTATUS.CloseByLackAnswer,
        //        CreateDate= new DateTime(2020,04,08),
        //        OrderDate=new DateTime(2020,04,09)
        //    },
        //    new Order()
        //    {
        //        HostingUnitKey=HostingUnitsList[2].HostingUnitKey,
        //        GuestRequestKey=GuestRequestsList[4].GuestRequestKey,
        //        OrderKey=99999997,
        //        Status=BE.ORDERSTATUS.NotYetTreated,
        //        CreateDate= new DateTime(2020,05,07),
        //        OrderDate=new DateTime(2020,05,08)
        //    }
        //};


        //public static List<HostingUnit> HostingUnitsList = new List<HostingUnit>()
        //{
        //   new HostingUnit()
        //   {
        //       HostingUnitKey=99999999,
        //       Owner= new Host()
        //       {
        //           HostKey= 123456789 ,
        //           PrivateName="hanna",
        //           FamilyName="levy",
        //           PhoneNumber=0587418529,
        //           MailAddress="hanna.levy@gmail.com",
        //           BankBranchH= BankBranches[0],
        //           CollectionClearance= true,
        //           BankAccountNumber= 166685
        //       },
        //       HostingUnitName="נווה מחמד ",
        //       Area=BE.AREA.South ,
        //       Jacuzzi= true,
        //       Pool=true,
        //       ChildrenAttractions=false,
        //       Garden=true
        //       },

        //   new HostingUnit()
        //   {
        //       HostingUnitKey=99999998,
        //       Owner= new Host()
        //       {
        //           HostKey= 123456788 ,
        //           PrivateName="rina",
        //           FamilyName="bendavid",
        //           PhoneNumber=0503851996,
        //           MailAddress="rina.benda@gmail.com",
        //           BankBranchH=BankBranches[1],
        //           CollectionClearance= true,
        //            BankAccountNumber= 166688
        //       },
        //       HostingUnitName="חמד הגליל",
        //       Area=BE.AREA.North,
        //       Jacuzzi= true,
        //       Pool=false,
        //       ChildrenAttractions=false,
        //       Garden=true
        //   },

        //       new HostingUnit()
        //   {
        //       HostingUnitKey=99999997,
        //       Owner= new Host()
        //       {
        //           HostKey= 123456787 ,
        //           PrivateName="israel",
        //           FamilyName="israeli",
        //           PhoneNumber=0526647886,
        //           MailAddress="israeli12@gmail.com",
        //           BankBranchH= BankBranches[2],
        //           CollectionClearance= true,
        //           BankAccountNumber= 165555
        //       },
        //       HostingUnitName=" צימר ישראלי",
        //       Area=BE.AREA.North ,
        //       Jacuzzi= false,
        //       Pool=false,
        //       ChildrenAttractions=true,
        //       Garden=true
        //       },

        //       new HostingUnit()
        //   {
        //       HostingUnitKey=99999996,
        //       Owner= new Host()
        //       {
        //           HostKey= 123456786 ,
        //           PrivateName="refael",
        //           FamilyName="refaeli",
        //           PhoneNumber=0526647899,
        //           MailAddress="refael22@gmail.com",
        //           BankBranchH= BankBranches[2],
        //           CollectionClearance= true,
        //           BankAccountNumber= 167777
        //       },
        //       HostingUnitName=" מלון רפאלי",
        //       Area=BE.AREA.Center,
        //       Jacuzzi= true,
        //       Pool=true,
        //       ChildrenAttractions=false,
        //       Garden=true
        //       }
        //};
        public static List<BankBranch> BankBranches = new List<BankBranch>()
        {
            new BankBranch()
                   {
                       BankNumber=11,
                       BankName="discount",
                       BranchNumber= 41,
                       BranchAddress=" yafo 220",
                       BranchCity="jerusalem"
                   },
            new BankBranch()
            {
                       BankNumber=12,
                       BankName="mizrahi",
                       BranchNumber= 45,
                       BranchAddress="kanfei nesharim 7",
                       BranchCity="jerusalem"
            },
            new BankBranch()
                   {
                       BankNumber=13,
                       BankName="bank hapoalim",
                       BranchNumber= 48,
                       BranchAddress="Allenby 25",
                       BranchCity=" Tel Aviv"
                   },
            new BankBranch()
            {
                       BankNumber=13,
                       BankName="bank hapoalim",
                       BranchNumber= 44,
                       BranchAddress="yerushlaim 12",
                       BranchCity=" Afula"
            },
            new BankBranch()
                   {
                       BankNumber=17,
                       BankName="ben leumi",
                       BranchNumber= 47,
                       BranchAddress="haganenet 34",
                       BranchCity="Jerusalem"
                   }
        };
        public static List<HostingUnit> HostingUnitsList = new List<BE.HostingUnit>();
        public static List<Order> OrdersList = new List<BE.Order>();
        public static List<GuestRequest> GuestRequestsList = new List<BE.GuestRequest>();
    }
}
