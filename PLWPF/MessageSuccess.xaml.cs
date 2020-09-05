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
    /// Interaction logic for PrivateAreaConnect.xaml
    /// </summary>
    public partial class MessageSuccess : Window
    {
        Window previousWindow;
        public MessageSuccess(Window w, string message) // reveal a message succeess to the operator
        {
            InitializeComponent();
            TextBoxSuccess.Text = message;
            previousWindow = w;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
