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
    /// Interaction logic for NumericUpDownControl.xaml
    /// A NumericUpDown control contains a single numeric value that can be incremented or decremented by clicking the up or down buttons of the control.
    /// </summary>
    public partial class NumericUpDownControl : UserControl
    {
        public int CurrentNumeric { get; set; }
        public NumericUpDownControl()
        {
            InitializeComponent();
            Value = 0;
            MinValue = 0;
        }
        public static readonly DependencyProperty ValueProperty =
               DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDownControl), new PropertyMetadata(0));
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }

            set
            {
                if (value < MinValue)
                    SetValue(ValueProperty, MinValue);
                else
                    SetValue(ValueProperty, value);

                txtNum.Text = Value.ToString();
            }
        }
        public int MinValue { get; set; }
        public void cmdUp_Click(object sender, RoutedEventArgs e) { Value++; }
        public void cmdDown_Click(object sender, RoutedEventArgs e) { Value--; }
        public void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null || txtNum.Text == "" || txtNum.Text == "-")
            {
                Value = 0;
                return;
            }
            int val;
            if (!int.TryParse(txtNum.Text, out val))
                txtNum.Text = Value.ToString();
            else Value = val;
        }
    }
}
