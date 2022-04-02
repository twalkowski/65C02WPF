using System.Windows;

namespace _65C02WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CPU myCpu = new CPU();
        Memory myMemory = new Memory(0x10000);
        private MainWindowDataContext DC => (MainWindowDataContext)DataContext;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            myCpu.Reset();
            myMemory.Initialize();
            DC.Instructions = 0;
            DC.Cycles = 0;
            DC.ShowCpuData(myCpu);
            RefreshMemoryDisplay();
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            DC.Cycles += myCpu.Step(ref myMemory);
            DC.Instructions += 1;
            DC.Cycles += 3;
            DC.ShowCpuData(myCpu);
            RefreshMemoryDisplay();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            DC.Page = (DC.Page + 1) & 0xff;
            RefreshMemoryDisplay();

        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (DC.Page == 0x00)
            {
                DC.Page = 0xff;
            }
            else
            {
                DC.Page--;
            }

            RefreshMemoryDisplay();
        }

        private void RefreshMemoryDisplay()
        {
            DC.HexDump = DC.ShowMemoryPageAsHexDump(myMemory, DC.Page);
        }

        private void Page_Click(object sender, RoutedEventArgs e)
        {
            RefreshMemoryDisplay();
        }
    }
}
