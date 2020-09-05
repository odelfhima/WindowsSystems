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

namespace PLWPF
{
    /// <summary>
    /// The actually password of the WEB SITE MANAGER is 11111
    /// </summary>
    public class ChangePassword: DependencyObject   // if the web site manager wants to change the password 
    {
        public string New
        {
            get { return (string)GetValue(NewpassProperty); }
            set { SetValue(NewpassProperty, value); }
        }
        public string Old
        {
            get { return (string)GetValue(OldpassProperty); }
            set { SetValue(OldpassProperty, value); }
        }
        public static readonly DependencyProperty NewpassProperty =
        DependencyProperty.Register("New", typeof(string), typeof(ChangePassword), new PropertyMetadata(default(ChangePassword)));
        public static readonly DependencyProperty OldpassProperty =
        DependencyProperty.Register("Old", typeof(string), typeof(ChangePassword), new PropertyMetadata(default(ChangePassword)));
    }

    /// <summary>
    /// Interaction logic for WebSiteManager.xaml
    /// </summary>
    public partial class WebSiteManager : Window
    {
        BL.IBL bl;
        CollectionView view;
        PropertyGroupDescription groupDescription;
        ChangePassword Password;
        public WebSiteManager()
        {
            InitializeComponent();
            BE.SiteOwner.password = "11111";
            WebSiteManagerAfterPassword.Visibility = Visibility.Collapsed;
            PasswordBorder.Visibility = Visibility.Visible;
            QClients.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            bl = BL.FactoryBL.GetBL();
            lvUsers.ItemsSource = bl.GuestRequestByCondition(GR => { return true; });
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
            groupDescription = new PropertyGroupDescription("Area");
            view.GroupDescriptions.Add(groupDescription);
            Password = new ChangePassword();
            Password.Old = "";
            Password.New = "";
        }

        private void BEnter_Click(object sender, RoutedEventArgs e) //when we click on the button ENTER we have to check if the password is correct and then the manager can enter on his account
        {
            try
            {
                if (ManagerPass.Password != BE.SiteOwner.password)
                {
                    throw new ArgumentException("The Manager Web Site Password is not correct");
                }
                WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
                PasswordBorder.Visibility = Visibility.Collapsed;
                QClients.Visibility = Visibility.Collapsed;
                Setting_pass.Visibility = Visibility.Collapsed;
                HostingUnitQ.Visibility = Visibility.Collapsed;
                GroupingByHosts.Visibility = Visibility.Collapsed;
                OrderQ.Visibility = Visibility.Collapsed;
                GroupingByFunctions.Visibility = Visibility.Collapsed;
                ordGrouping.Visibility = Visibility.Collapsed;
                ConstantData.Visibility = Visibility.Collapsed;
                Change_Links.Visibility = Visibility.Collapsed;
                DataBorder.Visibility = Visibility.Collapsed;
                InfTxtB.Text += "Data:\nthe current profit is: " + BE.Configuration.CurrentProfit.ToString() + "\nthe number of order is: " + bl.OrdersByCondition(ord => { return true; }).Count.ToString() +
                    "\nthe number of hosting units is: " + bl.HostingUnitByCondition(H => { return true; }).Count.ToString() + "\nthe number of guest requests is: " + bl.GuestRequestByCondition(GR => { return true; }).Count.ToString();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Access Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                BEnter_Click(null, null);
        }

        private void MenuItemClients_Click(object sender, RoutedEventArgs e) //when we click on the button Menu Item : Clients Querie
        {
            WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Visible;
            Setting_pass.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            ClientQcmb.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //fonction selection changed of the combo box 
        {
            ComboBox cb = sender as ComboBox;
            lvUsers.ItemsSource = null;
            lvUsers.ItemsSource = bl.GuestRequestByCondition(GR => { return true; });
            view = null;
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
            if (cb.SelectedIndex==0)  // if we want to see Clients Querie grouped by area
            {
                groupDescription = new PropertyGroupDescription("Area");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 1) //  if we want to see Clients Querie grouped by num of vacationners
            {
                groupDescription = new PropertyGroupDescription("Vacationers");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 2) // if we want to see Clients Querie grouped by type of hosting unit that they have choice
            {
                groupDescription = new PropertyGroupDescription("Type");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 3) // if we want to see Clients Querie grouped by Status 
            {
                groupDescription = new PropertyGroupDescription("Status");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void MenuItemHostingUnit_Click(object sender, RoutedEventArgs e) //when we click on the button Menu Item : Hosting Units Querie
        {
            WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Visible;
            Setting_pass.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Visible;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            view.GroupDescriptions.Remove(groupDescription);
            view = null;
            lvHostingUnits.ItemsSource = bl.HostingUnitbyArea();
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvHostingUnits.ItemsSource);
            groupDescription = new PropertyGroupDescription("Area"); // if we want to see Hosting Units Querie grouped by area
            view.GroupDescriptions.Add(groupDescription);
            Hucmb.SelectedIndex = 0;
        }

        private void MenuItemOrders_Click(object sender, RoutedEventArgs e) //when we click on the button Menu Item : Orders Querie
        {
            WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Visible;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Visible;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            view.GroupDescriptions.Remove(groupDescription);
            view = null;
            lvOrders.ItemsSource = bl.OrdersByCondition(ord=> { return true; });
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvOrders.ItemsSource);
            groupDescription = new PropertyGroupDescription("Status"); // if we want to see Orders Querie grouped by orders status 
            view.GroupDescriptions.Add(groupDescription);
            ordcmb.SelectedIndex = 1;
            priffitTxtB.Text = "the total profit is: " + BE.Configuration.CurrentProfit.ToString(); 
        }

        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Visible;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                HUByAreaQ.Visibility = Visibility.Visible;
                view.GroupDescriptions.Remove(groupDescription);
                view = null;
                lvHostingUnits.ItemsSource = bl.HostingUnitByCondition(H=> { return true; });
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvHostingUnits.ItemsSource);
                groupDescription = new PropertyGroupDescription("Area");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 1)
            {
                HUByAreaQ.Visibility = Visibility.Visible;
                view.GroupDescriptions.Remove(groupDescription);
                view = null;
                lvHostingUnits.ItemsSource = bl.HostingUnitByCondition(H => { return true; });
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvHostingUnits.ItemsSource);
                groupDescription = new PropertyGroupDescription("TypeOfHostingUnit");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 2)
            {
                ICGByHosts.ItemsSource = bl.HostByNumofHostingUnits();
                GroupingByHosts.Visibility = Visibility.Visible;
            }
        }

        private void ComboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebSiteManagerAfterPassword.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Visible;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            ordGrouping.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex == 0)
            {
                GroupingByFunctions.Visibility = Visibility.Visible;
                IOrdG.ItemsSource = bl.AllOrders();
            }
            if (cb.SelectedIndex == 1)
            {
                ordGrouping.Visibility = Visibility.Visible;
                view.GroupDescriptions.Remove(groupDescription);
                view = null;
                lvOrders.ItemsSource = bl.OrdersByCondition(ord => { return true; });
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvOrders.ItemsSource);
                groupDescription = new PropertyGroupDescription("Status");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 2)
            {
                ordGrouping.Visibility = Visibility.Visible;
                view.GroupDescriptions.Remove(groupDescription);
                view = null;
                lvOrders.ItemsSource = bl.OrdersByCondition(ord => { return true; });
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvOrders.ItemsSource);
                groupDescription = new PropertyGroupDescription("CreateDate.Month");
                view.GroupDescriptions.Add(groupDescription);
            }
            if (cb.SelectedIndex == 3)
            {
                ordGrouping.Visibility = Visibility.Visible;
                view.GroupDescriptions.Remove(groupDescription);
                view = null;
                lvOrders.ItemsSource = bl.OrdersByCondition(ord => { return true; });
                view = (CollectionView)CollectionViewSource.GetDefaultView(lvOrders.ItemsSource);
                groupDescription = new PropertyGroupDescription("HostingUnitKey");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e) // fonction to change the password of the web site manager 
        {
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Visible;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            this.DataContext = Password;

        }

        private void information_Click(object sender, RoutedEventArgs e) // when you click on Settings : Constant Data
        {
            ConstantData.Visibility = Visibility.Visible;
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            DaysExTxtB.Text = BE.Configuration.DaysExpireOrder.ToString(); // if you want to change the num of days expired for an order
            DaysEx2TxtB.Text = BE.Configuration.DaysExpireGuestRequest.ToString(); // if you want to change the num of days expired for an order
            CommisionTxtB.Text = BE.Configuration.Commission.ToString(); // if you want to change the tax of each day of vacation
        }

        private void Links_Click(object sender, RoutedEventArgs e)
        {
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            PasswordBorder.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Visible;
            NorthLTextBox.Text = BE.Configuration.LinkNorthAttractions;
            CenterLTextBox.Text = BE.Configuration.LinkCenterAttractions;
            JerusalemLTextBox.Text = BE.Configuration.LinkJerusalemAttractions;
            SouthLTextBox.Text = BE.Configuration.LinkSouthAttractions;
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Old==BE.SiteOwner.password)
            {
                BE.SiteOwner.password = Password.New;
                MessageBox.Show("Password Changed", "Password", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
            }
            else
            {
                MessageBox.Show("the Current password is incorrect, please try again...", "Password", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
            Password.Old = "";
            Password.New = "";
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            PasswordBorder.Visibility = Visibility.Visible;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Collapsed;
            ManagerPass.Password = "";
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

        private void DaysExB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num;
                if (!int.TryParse(DaysExTxtB.Text, out num))
                {
                    throw new ArgumentException("it must be a number");
                }
                bl.Change_DaysExpiredOrder(int.Parse(DaysExTxtB.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Manager Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CommisionB_Click(object sender, RoutedEventArgs e) // 
        {
            try
            {
                int num;
                if (!int.TryParse(CommisionTxtB.Text, out num))
                {
                    throw new ArgumentException("it must be a number");
                }
                bl.Change_Commision(int.Parse(CommisionTxtB.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Manager Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Change_linksB_Click(object sender, RoutedEventArgs e) //if we want to change the internet links and put others instead- the new link have to be correct
        {
            try
            {
                if (!bl.RemoteFileExists(NorthLTextBox.Text))
                    throw new ArgumentException("North Link doesn't exist");
                if (!bl.RemoteFileExists(CenterLTextBox.Text))
                    throw new ArgumentException("Center Link doesn't exist");
                if (!bl.RemoteFileExists(JerusalemLTextBox.Text))
                    throw new ArgumentException("Jerusalem Link doesn't exist");
                if (!bl.RemoteFileExists(SouthLTextBox.Text))
                    throw new ArgumentException("South Link doesn't exist");
                BE.Configuration.LinkNorthAttractions = NorthLTextBox.Text;
                BE.Configuration.LinkSouthAttractions = SouthLTextBox.Text;
                BE.Configuration.LinkJerusalemAttractions = JerusalemLTextBox.Text;
                BE.Configuration.LinkCenterAttractions = CenterLTextBox.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Manager Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_linksB_Click(object sender, RoutedEventArgs e) // click to cancel the configuration of the links and to make sure that the links are correct- if the new link it's not correct and we click on cancel ,it puts back the old link 
        {
            NorthLTextBox.Text = BE.Configuration.LinkNorthAttractions;
            CenterLTextBox.Text = BE.Configuration.LinkCenterAttractions;
            JerusalemLTextBox.Text = BE.Configuration.LinkJerusalemAttractions;
            SouthLTextBox.Text = BE.Configuration.LinkSouthAttractions;
        }

        private void DaysEx2B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num;
                if (!int.TryParse(DaysExTxtB.Text, out num))
                {
                    throw new ArgumentException("it must be a number");
                }
                bl.Change_DaysExpiredGuestRequest(int.Parse(DaysExTxtB.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Manager Data", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PasswordBorder.Visibility = Visibility.Collapsed;
            QClients.Visibility = Visibility.Collapsed;
            HUByAreaQ.Visibility = Visibility.Collapsed;
            HostingUnitQ.Visibility = Visibility.Collapsed;
            GroupingByHosts.Visibility = Visibility.Collapsed;
            OrderQ.Visibility = Visibility.Collapsed;
            GroupingByFunctions.Visibility = Visibility.Collapsed;
            Setting_pass.Visibility = Visibility.Collapsed;
            PasswordBorder.Visibility = Visibility.Collapsed;
            ConstantData.Visibility = Visibility.Collapsed;
            Change_Links.Visibility = Visibility.Collapsed;
            DataBorder.Visibility = Visibility.Visible;

        }
    }
}
