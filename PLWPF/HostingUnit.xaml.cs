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
using System.Windows.Shapes;
using System.Globalization;

namespace PLWPF
{


    #region converter
    /// <summary>
    /// converter for add button and Collection Clearnace
    /// </summary>
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            if (boolValue)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    /// <summary>
    /// class with order that comftible to view on the window
    /// </summary>
    public class OrderView
    {
        public BE.Order order { get; set; }
        public string PrivateNameGuest { get; set; }
        public string FamilyNameGuest { get; set; }
        public DateTime EntryDateGuest { get; set; }
        public DateTime ReleaseDateGuest { get; set; }
        public DateTime CreateDateOrder { get; set; }
        public int ChildrenGuest { get; set; }
        public int AdultsGuest { get; set; }
    }

    /// <summary>
    /// Interaction logic for HostingUnit.xaml
    /// </summary>
    public partial class HostingUnit : Window
    {
        private System.Windows.Controls.Calendar MyCalendar;
        BE.HostingUnit hostU;
        BL.IBL bl;
        public HostingUnit()
        {
            InitializeComponent();
            hostU = new BE.HostingUnit();
            hostU.Owner = new BE.Host();
            hostU.Owner.BankBranchH = new BE.BankBranch();
            this.DataContext = hostU;
            bl = BL.FactoryBL.GetBL();
            this.BankBranchComboBox.ItemsSource = bl.bankBranchesList();
            mainGrid.Height = this.Height;
            mainGrid.Width = this.Width;
            TabChoice.Width = this.Width;
            TabChoice.Height = this.Height;
            AddTab.Width = this.Width;
            AddTab.Height = this.Height;
            infAdd.Text = "  you have to agree to collect comission\n  in order to add you hosting unit!\n  it cost " + BE.Configuration.Commission.ToString() + " per day\n  when order close successfuly";
            this.AreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.AREA));
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.TYPE));
        }
        public HostingUnit(BE.HostingUnit H)
        {
            InitializeComponent();
            bl = BL.FactoryBL.GetBL();
            hostU = H;
            this.DataContext = hostU;
            this.BankBranchComboBox.ItemsSource = bl.bankBranchesList();
            mainGrid.Height = this.Height;
            mainGrid.Width = this.Width;
            TabChoice.Width = this.Width;
            TabChoice.Height = this.Height;
            AddTab.Width = this.Width;
            AddTab.Height = this.Height;
            HostingUkey.Password = hostU.HostingUnitKey.ToString();
            this.DataContext = hostU;
            PrivateAreaPasswordBorder.Visibility = Visibility.Collapsed;
            PrivateAreaBorder.Visibility = Visibility.Visible;
            WelcomeHost.Text = "welcome " + hostU.Owner.PrivateName + " " + hostU.Owner.FamilyName + "!";
            WelcomeHost.FontSize = 20;
            WelcomeHost.FontWeight = FontWeights.Bold;
            InitializeDiaryandOrders();
        }
        private System.Windows.Controls.Calendar CreateCalendar() // Create a calendar, show monthly, select one sequence, mark the current day.
        {
            System.Windows.Controls.Calendar MonthlyCalendar = new System.Windows.Controls.Calendar();
            MonthlyCalendar.Name = "MonthlyCalendar";
            MonthlyCalendar.DisplayMode = CalendarMode.Month;
            MonthlyCalendar.SelectionMode = CalendarSelectionMode.SingleRange;
            MonthlyCalendar.IsTodayHighlighted = true;
            MonthlyCalendar.Height = vbCalendar.Height;
            MonthlyCalendar.Width = vbCalendar.Width;
            return MonthlyCalendar;
        }

        private void SetBlackOutDates()  // Go over all the days on the updated list and mark them in the calendar.
        {
            for (DateTime date = new DateTime(2020, 01, 01); date != new DateTime(2020, 12, 31); date = date.AddDays(1))
            {
                if (hostU.Diary[date.Day - 1, date.Month - 1] == true)
                {
                    MyCalendar.BlackoutDates.Add(new CalendarDateRange(date));
                }
            }
        }
        private void addButton_Click(object sender, RoutedEventArgs e) //when we click on the button ADD to add hosting unit
        {
            try
            {
                if (hostU.Owner.BankBranchH.BankNumber == 0 || hostU.HostingUnitName == null || hostU.Owner.HostKey == null || hostU.Owner.PrivateName == null || hostU.Owner.FamilyName == null || hostU.Owner.MailAddress == null || hostU.Owner.PhoneNumber == null || hostU.Owner.BankAccountNumber == 0)
                    throw new ArgumentException("Please fill out all required fields");


                MessageBox.Show(bl.AddHostingUnit(hostU), "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Information);
                HostingUkey.Password = hostU.HostingUnitKey.ToString();
                hostU = null;
                hostU = new BE.HostingUnit();
                hostU.Owner = new BE.Host();
                hostU.Owner.BankBranchH = new BE.BankBranch();
                this.DataContext = hostU;
                BEnter_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BEnter_Click(object sender, RoutedEventArgs e) // when we click on the button ENTER we have to check if the hosting unit key exist and that it is in the right format
        {
            try
            {
                int num;
                if (HostingUkey.Password.Length != 8 || !int.TryParse(HostingUkey.Password, out num))
                {
                    throw new ArgumentException("the hosting unit key must be in format of 8 digits");
                }
                hostU = bl.hostUnitbyKey(int.Parse(HostingUkey.Password));
                this.DataContext = hostU;
                PrivateAreaPasswordBorder.Visibility = Visibility.Collapsed;
                PrivateAreaBorder.Visibility = Visibility.Visible; 
                WelcomeHost.Text = "Welcome " + hostU.Owner.PrivateName + " " + hostU.Owner.FamilyName + "!";
                numOrders.Text += bl.NumofOrdersClosebySucces(hostU).ToString();
                WelcomeHost.FontSize=19;
                InitializeDiaryandOrders();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Access Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void InitializeDiaryandOrders() //  Initialize the Diary and Orders
        {
            List<BE.Order> OrdersList = bl.ListOfOrdersbyHostingUnitKey(hostU.HostingUnitKey);
            var CurrentGuestRequests = bl.GuestRequestByHostingUnitOrder(hostU.HostingUnitKey);
            List<OrderView> OrdersDetais = (from Order in OrdersList
                               select (from GR in CurrentGuestRequests
                                       where GR.GuestRequestKey == Order.GuestRequestKey
                                       select new OrderView
                                       {
                                           order = Order,
                                           PrivateNameGuest = GR.PrivateName,
                                           FamilyNameGuest = GR.FamilyName,
                                           EntryDateGuest = GR.EntryDate,
                                           ReleaseDateGuest = GR.ReleaseDate,
                                           CreateDateOrder = Order.CreateDate,
                                           ChildrenGuest = GR.Children,
                                           AdultsGuest = GR.Adults
                                       }).FirstOrDefault()).ToList();
            OrdersDataGrid.ItemsSource = null ;
            OrdersDataGrid.ItemsSource = OrdersDetais;
            MyCalendar = null;
            MyCalendar = CreateCalendar();
            vbCalendar.Child = null;
            vbCalendar.Child = MyCalendar;
            SetBlackOutDates();
        }


        private void Pass_KeyDown(object sender, KeyEventArgs e) // check if the password its correct
        {
            if (e.Key == Key.Return)
                BEnter_Click(null, null);
        }

        private void TabChoice_SelectionChanged(object sender, SelectionChangedEventArgs e) // fonction selection changed for tab control
        {
            try
            {
                TabControl tabControl = sender as TabControl;
                if (tabControl.SelectedIndex == 1)
                {
                    hostU = new BE.HostingUnit();
                    hostU.Owner = new BE.Host();
                    hostU.Owner.BankBranchH = new BE.BankBranch();
                    this.DataContext = hostU;
                }
                if (tabControl.SelectedIndex == 0)
                {
                    HostingUkey.Password = "";
                    PrivateAreaBorder.Visibility = Visibility.Collapsed;
                    PrivateAreaPasswordBorder.Visibility = Visibility.Visible;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Information Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
        }

        private void BLogOut_Click(object sender, RoutedEventArgs e) //when we click on the button LOG OUT
        {
            hostU = null;
            hostU = new BE.HostingUnit();
            hostU.Owner = new BE.Host();
            hostU.Owner.BankBranchH = new BE.BankBranch();
            this.DataContext = hostU;
            PrivateAreaBorder.Visibility = Visibility.Collapsed;
            PrivateAreaPasswordBorder.Visibility = Visibility.Visible;
            HostingUkey.Password = "";
        }

        private void BDelete_Click(object sender, RoutedEventArgs e) // when we click on the button Delete to delete Hosting Unit 
        {
            try
            {
                var result = MessageBox.Show("are you Sure you want to delete the hosting unit?", "Hosting Unit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.None);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show(bl.DeleteHostingUnit(hostU), "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                    hostU = null;
                    hostU = new BE.HostingUnit();
                    hostU.Owner = new BE.Host();
                    hostU.Owner.BankBranchH = new BE.BankBranch();
                    this.DataContext = hostU;
                    PrivateAreaBorder.Visibility = Visibility.Collapsed;
                    PrivateAreaPasswordBorder.Visibility = Visibility.Visible;
                    HostingUkey.Password = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed Removing Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
        }

        private void BUpdate_Click(object sender, RoutedEventArgs e) //when we click on the button UPDATE to update hosting unit
        {
            new UpdateHostingUnit(hostU).ShowDialog();
            Close();
        }

        private void BAddNewOrder_Click(object sender, RoutedEventArgs e) //when we click on the button Add Orders to ADD new orders
        {
            vbCalendar.Child = null;
            new AddNewOrder(hostU, MyCalendar).ShowDialog();
            Close();
        }
        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e) // fonction selection changed of the combo box who proposes all the orders status 
        {
            try
            {
                ComboBox myComboBox = sender as ComboBox;
                var cbi = myComboBox.SelectedIndex;
                var valuesAsArray = Enum.GetValues(typeof(BE.ORDERSTATUS));
                if (OrdersDataGrid.SelectedItem is OrderView)
                {
                    OrderView orderview = (OrderView)OrdersDataGrid.SelectedItem;
                    MessageBox.Show(bl.UpdateOrderStatus(orderview.order, (BE.ORDERSTATUS)valuesAsArray.GetValue(cbi)), "Order", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                    InitializeDiaryandOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed Update Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
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

        private void IsCollectionClearance_Click(object sender, RoutedEventArgs e)
        {
            addButton.IsEnabled = IsCollectionClearance.IsChecked.Value;
        }
    }
}




