using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region data source list for Initialize xml 
        //List<BE.GuestRequest> GuestRequestsList = new List<BE.GuestRequest>()
        //{
        //    new BE.GuestRequest()
        //    {

        //         GuestRequestKey= 0,
        //         PrivateName= "efrat",
        //         FamilyName="cohen",
        //         MailAddress="odel.fhima@gmail.com",
        //         Status = BE.GUESTSTATUS.Active,
        //         RegistrationDate= new DateTime(2020,08,06),
        //         EntryDate=new DateTime (2020,09,06),
        //         ReleaseDate= new DateTime(2020,09,10),
        //         Area=BE.AREA.All ,
        //         Type= BE.TYPE.Zimmer,
        //         Adults=2,
        //         Children=7,
        //         Pool=BE.CHOICE.Necessary,
        //         Jacuzzi=BE.CHOICE.Possible,
        //         Garden=BE.CHOICE.Possible
        //    },

        //    new BE.GuestRequest()
        //    {
        //         GuestRequestKey= 0,
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
        //         Garden=BE.CHOICE.Necessary
        //    },

        //    new BE.GuestRequest()
        //    {

        //        GuestRequestKey= 0,
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
        //         Garden=BE.CHOICE.Uninterested
        //    },

        //    new BE.GuestRequest()
        //    {

        //        GuestRequestKey= 0,
        //         PrivateName= "oshri",
        //         FamilyName="cohen",
        //         MailAddress="odel.fhima@gmail.com",
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
        //         Garden=BE.CHOICE.Necessary
        //    },

        //    new BE.GuestRequest()
        //    {

        //         GuestRequestKey= 0,
        //         PrivateName= "bibi",
        //         FamilyName="Ben-Gurion",
        //         MailAddress="odel.fhima@gmail.com",
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
        //         Garden=BE.CHOICE.Necessary
        //    }
        //};
        //List<BE.HostingUnit> HostingUnitsList = new List<BE.HostingUnit>()
        //        {
        //           new BE.HostingUnit()
        //           {
        //               HostingUnitKey=0,
        //               Owner= new BE.Host()
        //               {
        //                   HostKey= "12345678" ,
        //                   PrivateName="hanna",
        //                   FamilyName="levy",
        //                   PhoneNumber="0587418529",
        //                   MailAddress="hanna.levy@gmail.com",
        //                   BankBranchH= new BE.BankBranch()
        //           {
        //               BankNumber=11,
        //               BankName="discount",
        //               BranchNumber= 41,
        //               BranchAddress=" yafo 220",
        //               BranchCity="jerusalem"
        //           },
        //                   CollectionClearance= true,
        //                   BankAccountNumber= 166685
        //               },
        //               HostingUnitName="Neve Hemed",
        //               TypeOfHostingUnit=BE.TYPE.Zimmer,
        //               Area=BE.AREA.All ,
        //               Jacuzzi= true,
        //               Pool=true,
        //               Garden=true
        //               },

        //   new BE.HostingUnit()
        //   {
        //       HostingUnitKey=0,
        //       Owner= new BE.Host()
        //       {
        //           HostKey= "12345677" ,
        //           PrivateName="rina",
        //           FamilyName="bendavid",
        //           PhoneNumber="0503851996",
        //           MailAddress="rina.benda@gmail.com",
        //           BankBranchH=new BE.BankBranch()
        //           {
        //               BankNumber=11,
        //               BankName="discount",
        //               BranchNumber= 41,
        //               BranchAddress=" yafo 220",
        //               BranchCity="jerusalem"
        //           },
        //           CollectionClearance= true,
        //            BankAccountNumber= 166688
        //       },
        //       HostingUnitName="Hemed Hagalil",
        //       TypeOfHostingUnit=BE.TYPE.Hotel,
        //       Area=BE.AREA.North,
        //       Jacuzzi= true,
        //       Pool=false,
        //       Garden=true
        //   },

        //       new BE.HostingUnit()
        //       {
        //       HostingUnitKey=0,
        //       Owner= new BE.Host()
        //       {
        //           HostKey= "12345676" ,
        //           PrivateName="israel",
        //           FamilyName="israeli",
        //           PhoneNumber="0526647886",
        //           MailAddress="israeli12@gmail.com",
        //           BankBranchH= new BE.BankBranch()
        //           {
        //               BankNumber=11,
        //               BankName="discount",
        //               BranchNumber= 41,
        //               BranchAddress=" yafo 220",
        //               BranchCity="jerusalem"
        //           },
        //           CollectionClearance= true,
        //           BankAccountNumber= 165555
        //       },
        //       HostingUnitName="Israely Zimmer",
        //       TypeOfHostingUnit=BE.TYPE.Zimmer,
        //       Area=BE.AREA.North ,
        //       Jacuzzi= false,
        //       Pool=false,
        //       Garden=true
        //       },

        //       new BE.HostingUnit()
        //       {
        //       HostingUnitKey=0,
        //       Owner= new BE.Host()
        //       {
        //           HostKey= "12345675" ,
        //           PrivateName="refael",
        //           FamilyName="refaeli",
        //           PhoneNumber="0526647899",
        //           MailAddress="refael22@gmail.com",
        //           BankBranchH= new BE.BankBranch()
        //           {
        //               BankNumber=11,
        //               BankName="discount",
        //               BranchNumber= 41,
        //               BranchAddress=" yafo 220",
        //               BranchCity="jerusalem"
        //           },
        //           CollectionClearance= true,
        //           BankAccountNumber= 167777
        //       },
        //       HostingUnitName="Refaeli Hotel",
        //       TypeOfHostingUnit=BE.TYPE.Hotel,
        //       Area=BE.AREA.Center,
        //       Jacuzzi= true,
        //       Pool=true,
        //       Garden=true
        //       }
        //};
        #endregion
        BL.IBL bl;
        public MainWindow()
        {
            InitializeComponent();
            string ProjectPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName;//path of xml files
            Uri imageAttraction = new Uri(ProjectPath + "/Data/AttractionIcon.png", UriKind.Absolute);            bl = BL.FactoryBL.GetBL();
            NI.Source = new BitmapImage(imageAttraction) ;
            CI.Source = new BitmapImage(imageAttraction) ;
            JI.Source = new BitmapImage(imageAttraction) ;
            SI.Source = new BitmapImage(imageAttraction) ;
            //--- start the threads ---
            //BL.BL_imp.closeGuestRequest.Start();
            //BL.BL_imp.closeOrder.Start();
            //BL.BL_imp.UpdatesDiary.Start();
            AboutTxt.Text += "\nThis site started run in the\nbeggining of 2020, in order to help\npeople " +
                "find a vacation in easy way\nand in order to help Hosts to find clients...\n\nSo How It's Working?\n -Do you want to order vacation?\n" +
                "  press on 'Guest Request', fill the\n  details and add yourself to the\n  site. You can always update your\n  details if you change your mind.\n" +
                " -Do you have a hosting unit and\n  you want more clients?\n  press 'Hosting Unit', go to 'Add\n  Hosting Unit' tab, fill the " +
                "details\n  and add your\n hosting unit on the site. You can\n  also update your details.\n  You'll see optional guest requests\n  and you have the choice to order them.\n  " +
                "*when you closing order successfuly\n  the site will collect commision\n  from your bank account.\n\n       © Odel Fhima and Adi Giladi";
            #region add the lists           
            //foreach (BE.GuestRequest GR in GuestRequestsList)
            //{
            //    bl.AddGuestRequest(GR);
            //}
            //foreach (BE.HostingUnit HU in HostingUnitsList)
            //{
            //    bl.AddHostingUnit(HU);
            //}
            #endregion
        }

        private void websitemanagerButton_Click(object sender, RoutedEventArgs e) //when we click on the button WEB SITE MANAGER we are directed to his window
        {
            Window WebSiteManagerWindow = new WebSiteManager();
            WebSiteManagerWindow.ShowDialog();

        }

        private void hostingunitButton_Click(object sender, RoutedEventArgs e) //when we click on the button Hosting Unit we are directed to his window
        {
            new HostingUnit().ShowDialog();

        }

        private void guestrequestButton_Click(object sender, RoutedEventArgs e) //when we click on the button Guest Request we are directed to his window
        {
            new GuestRequest().ShowDialog();

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) // when you touch the button with the mouse it changes his size and his writing
        {
            ((Button)sender).Width *= 1.1;
            ((Button)sender).Height *= 1.1;
            ((Button)sender).FontWeight = FontWeights.Bold;
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e) // when you remove the mouse from the button it changes his size and his writing
        {
            ((Button)sender).Width /= 1.1;
            ((Button)sender).Height /= 1.1;
            ((Button)sender).FontWeight = FontWeights.DemiBold;
        }

        private void NI_MouseUp(object sender, MouseButtonEventArgs e) // 
        {
            try
            {
                openLink(BE.Configuration.LinkNorthAttractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void openLink(string link) // open link of attractions all around ISRAEL

        {
            try
            {
                System.Diagnostics.Process.Start(link);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void MyMouseEnter(object sender, MouseEventArgs e) // when you touch the button with the mouse it changes the image size
        {
            ((Image)sender).Width *= 1.2;
            ((Image)sender).Height *= 1.2;
        }

        private void MyMouseLeave(object sender, MouseEventArgs e) // when you remove the mouse from the button it changes the image size
        {
            ((Image)sender).Width /= 1.2;
            ((Image)sender).Height /= 1.2;
        }

        private void SI_MouseUp(object sender, MouseButtonEventArgs e) // to open the link of Attraction in South of Israel
        {
            try
            {
                openLink(BE.Configuration.LinkSouthAttractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void JI_MouseUp(object sender, MouseButtonEventArgs e) //// to open the link of Attraction in Jerusalem 
        {
            try
            {
                openLink(BE.Configuration.LinkJerusalemAttractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CI_MouseUp(object sender, MouseButtonEventArgs e) // // to open the link of Attraction in Center of Israel
        {
            try
            {
                openLink(BE.Configuration.LinkCenterAttractions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
