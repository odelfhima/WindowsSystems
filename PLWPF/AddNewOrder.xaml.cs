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
    /// Interaction logic for AddNewOrder.xaml
    /// </summary>
    public partial class AddNewOrder : Window
    {
        BL.IBL bl;
        BE.HostingUnit hostU;
        private Calendar MyCalendar;
        public AddNewOrder(BE.HostingUnit H, Calendar c)
        {
            InitializeComponent();
            bl = BL.FactoryBL.GetBL();
            hostU = H;
            MyCalendar = c;
            vbCalendar.Height = MyCalendar.Height;
            vbCalendar.Width = MyCalendar.Width;
            vbCalendar.Child = MyCalendar;
            SetBlackOutDates();
            var GuestRequestList = bl.GuestRequestByCondition(hostU);
            SubGrid.Height = 0;
            for (int i = 0; i < GuestRequestList.Count; i++)
            {
               SubGrid.Height += 300;
                SubGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < GuestRequestList.Count; i++)   // For row 0 data, create UserControl by the number of hosting units of the selected host,
            {
                GuestRequestUserControl a = new GuestRequestUserControl(GuestRequestList[i], hostU.HostingUnitKey);
                SubGrid.Children.Add(a);  // Add them to the display organizer
               Grid.SetRow(a, i);  // and put them in the appropriate row
            }
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

        private void Window_Closed(object sender, EventArgs e)
        {
            new HostingUnit(hostU).ShowDialog();
            Close();
        }
    }
}
