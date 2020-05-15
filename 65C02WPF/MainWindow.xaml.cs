using System.Windows;

namespace _65C02WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CPU myCpu = new CPU();
        public Memory myMemory = new Memory(256 * 256);
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
            DC.DisplayCpuData(myCpu);
            DC.HexDump = DC.DisplayMemoryPageAsHexDump(myMemory, DC.Page);
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            myCpu.Step();
            DC.Instructions += 1;
            DC.Cycles += 3;
            DC.DisplayCpuData(myCpu);
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            DC.Page = (DC.Page + 1) & 0xff;
            DC.HexDump = DC.DisplayMemoryPageAsHexDump(myMemory, DC.Page);

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

            DC.HexDump = DC.DisplayMemoryPageAsHexDump(myMemory, DC.Page);

        }
    }
}
