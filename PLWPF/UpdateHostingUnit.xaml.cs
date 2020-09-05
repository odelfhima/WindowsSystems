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
    /// Interaction logic for UpdateHostingUnit.xaml
    /// </summary>
    public partial class UpdateHostingUnit : Window
    {

        BE.HostingUnit hostUnit;
        BL.IBL bl;
        public UpdateHostingUnit(BE.HostingUnit H)
        {
            InitializeComponent();
            bl = BL.FactoryBL.GetBL();
            hostUnit = H;
            this.DataContext = hostUnit;
            this.BankBranchComboBox.ItemsSource = bl.bankBranchesList();
            UpdateGrid.Height = this.Height;
            UpdateGrid.Width = this.Width;
            this.AreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.AREA));
            this.TypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.TYPE));
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)  //when we click on the button UPDATE to update the hosting unit 
        {
            try
            {
                MessageBox.Show(bl.UpdateHostingUnit(hostUnit), "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hosting Unit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            new HostingUnit(hostUnit).ShowDialog();
        }
    }
}
