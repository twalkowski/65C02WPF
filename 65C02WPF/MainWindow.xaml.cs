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

namespace _65C02WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CPU myCpu = new CPU();
        Memory myMemory = new Memory(256 * 256);

        public MainWindow()
        {
            this.InitializeComponent();

            // Set up the Viewmodel to display data deom the CPU and memory
            // then set the data contexct for the XAML to link to the ViewModel
            this.ViewModel = new CpuViewModel();
            this.DataContext = ViewModel;
        }

        CpuViewModel ViewModel { get; set; }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            myCpu.Reset();
            myMemory.Initialize();
            ViewModel.UpdateView(myCpu);
            ViewModel.HexDump = ViewModel.DumpMemAsHex(myMemory, ViewModel.Page);
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            myCpu.Step();
            ViewModel.UpdateView(myCpu);
        }
  
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Page = (ViewModel.Page + 1) & 0xff;
            ViewModel.HexDump = ViewModel.DumpMemAsHex(myMemory, ViewModel.Page);
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Page == 0x00)
            {
                ViewModel.Page = 0xff;
            }
            else
            {
                ViewModel.Page--;
            }

            ViewModel.HexDump = ViewModel.DumpMemAsHex(myMemory, ViewModel.Page);
        }

    }
}
