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
    /// Interaction logic for GuestRequest.xaml
    /// </summary>
    public partial class GuestRequest : Window
    {
        BE.GuestRequest guestRequest;
        BL.IBL bl;
        public GuestRequest()
        {
            InitializeComponent();
            guestRequest = new BE.GuestRequest();
            this.DataContext = guestRequest;
            bl = BL.FactoryBL.GetBL();
            this.AreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.AREA));
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.TYPE));
            this.jacuzziChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.gardenChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.poolChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.UAreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.AREA));
            this.UTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.TYPE));
            this.UjacuzziChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.UgardenChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.UpoolChoice.ItemsSource = Enum.GetValues(typeof(BE.CHOICE));
            this.entryDateDatePicker.DisplayDateStart = DateTime.Now;
            this.ReleaseDateDatePicker.DisplayDateStart = DateTime.Now;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)  //when we click on the button ADD 
        {
            try
            {
                if (guestRequest.PrivateName== null || guestRequest.FamilyName == null) // we have to fill out all the required fields
                    throw new ArgumentException("Please fill out all required fields");
                guestRequest.Adults = NumAdults.Value;
                guestRequest.Children = NumChildren.Value;
                new MessageSuccess(this, bl.AddGuestRequest(guestRequest)).ShowDialog();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Guest Request", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e) //when we click on the button LOG OUT 
        {
            var result = MessageBox.Show("Do you want to save the changes that you made?", "Log Out", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.None);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show(bl.UpdateGuestRequest(guestRequest), "Guest Request", MessageBoxButton.OK, MessageBoxImage.Information);
                guestRequest = new BE.GuestRequest();
                DataContext = guestRequest;
                UNumAdults.Value = 0;
                UNumChildren.Value = 0;
                GRkey.Password = "";
                UpdateBeforePassword.Visibility = Visibility.Visible;
                UpdateAfterPassword.Visibility = Visibility.Collapsed;
            }
            if (result == MessageBoxResult.No)
            {
                guestRequest = new BE.GuestRequest();
                UNumAdults.Value = 0;
                UNumChildren.Value = 0;
                GRkey.Password = "";
                UpdateBeforePassword.Visibility = Visibility.Visible;
                UpdateAfterPassword.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e) //when we click on the button UPDATE
        {
            try
            {
                guestRequest.Adults = UNumAdults.Value;
                guestRequest.Children = UNumChildren.Value;
                MessageBox.Show(bl.UpdateGuestRequest(guestRequest), "Guest Request", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Guest Request", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BEnter_Click(object sender, RoutedEventArgs e) // when we click on the button ENTER we have to check if the guest request key exist and that it is in the right format
        {
            try
            {
                int num;
                if (GRkey.Password.Length != 8 || !int.TryParse(GRkey.Password, out num))
                {
                    throw new ArgumentException("the Guest Request key must be in format of 8 digits");
                }
                var GRList = bl.GuestRequestByCondition(GR => { return GR.GuestRequestKey == int.Parse(GRkey.Password); });
                if (GRList.Count == 0)
                {
                    throw new ArgumentException("This Guest Request Key doesn't exist");
                }
                guestRequest = GRList.FirstOrDefault();
                DataContext = guestRequest;
                UNumAdults.Value= guestRequest.Adults;
                UNumChildren.Value=guestRequest.Children;
                UpdateBeforePassword.Visibility = Visibility.Collapsed;
                UpdateAfterPassword.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Access Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                BEnter_Click(null, null);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) // fonction selection changed for the tab control
        {
            guestRequest = null;
            GRkey.Password = "";
            try
            {
                TabControl tabControl = sender as TabControl;
                if (tabControl.SelectedIndex == 0)
                {
                    guestRequest = new BE.GuestRequest();
                }
                if (tabControl.SelectedIndex == 1)
                {
                    UpdateBeforePassword.Visibility = Visibility.Visible;
                    UpdateAfterPassword.Visibility = Visibility.Collapsed;
                    guestRequest = new BE.GuestRequest();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
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
    }
}
