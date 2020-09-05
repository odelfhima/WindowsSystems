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

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestRequestUserControl.xaml
    /// </summary>
    public partial class GuestRequestUserControl : UserControl
    {
        BE.GuestRequest GuestR;
        int this_HostingUnitKey;
        BL.IBL bl;
        public GuestRequestUserControl(BE.GuestRequest GR, int HostingUnitKey)
        {
            InitializeComponent();
            bl = BL.FactoryBL.GetBL();
            GuestR = GR;
            this.DataContext = GuestR;
            this_HostingUnitKey = HostingUnitKey;
            NameTextBox.Text = GuestR.PrivateName + " " + GuestR.FamilyName;
        }

        private void Button_Click(object sender, RoutedEventArgs e) //when we click on the button ORDER to add order
        {
            try
            {
                MessageBox.Show(bl.AddOrder(GuestR.GuestRequestKey, this_HostingUnitKey), "Order", MessageBoxButton.OK, MessageBoxImage.Information);
                this.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Order", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
