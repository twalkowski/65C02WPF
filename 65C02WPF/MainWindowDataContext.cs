using System.Text;

namespace _65C02WPF
{
    class MainWindowDataContext : ObservableObject
    {
        // Statitics

        private int _instructions;
        public int Instructions         /// The number of CPU instructions executed by the simulator
        {
            get => _instructions;
            set => Set(ref _instructions, value);
        }


        private ulong _cycles;
        public ulong Cycles             /// The number of machine cycles executed by the simulator

        {
            get => _cycles;
            set => Set(ref _cycles, value);
        }


        // CPU

        private int _accumulator;
        public int Accumulator
        {
            get => _accumulator;
            set => Set(ref _accumulator, value);
        }

        private int _xRegister;
        public int XRegister
        {
            get => _xRegister;
            set => Set(ref _xRegister, value);
        }

        private int _yRegister;
        public int YRegister
        {
            get => _yRegister;
            set => Set(ref _yRegister, value);
        }

        private int _stackPointer;
        public int StackPointer
        {
            get => _stackPointer;
            set => Set(ref _stackPointer, value);
        }

        private int _programCounter;
        public int ProgramCounter
        {
            get => _programCounter;
            set => Set(ref _programCounter, value);
        }

        private int _statusRegister;
        public int StatusRegister
        {
            get => _statusRegister;
            set => Set(ref _statusRegister, value);
        }

        private int _data;
        public int Data
        {
            get { return _data; }
            set => Set(ref _data, value);
        }

        private int _address;
        public int Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        private bool _nFlag;
        public bool NFlag
        {
            get => _nFlag;
            set => Set(ref _nFlag, value);
        }

        private bool _vFlag;
        public bool VFlag
        {
            get => _vFlag;
            set => Set(ref _vFlag, value);
        }

        private bool _bFlag;
        public bool BFlag
        {
            get => _bFlag;
            set => Set(ref _bFlag, value);
        }

        private bool _dFlag;
        public bool DFlag
        {
            get => _dFlag;
            set => Set(ref _dFlag, value);
        }

        private bool _iFlag;
        public bool IFlag
        {
            get => _iFlag;
            set => Set(ref _iFlag, value);
        }

        private bool _zFlag;
        public bool ZFlag
        {
            get => _zFlag;
            set => Set(ref _zFlag, value);
        }

        private bool _cFlag;
        public bool CFlag
        {
            get => _cFlag;
            set => Set(ref _cFlag, value);
        }

        // Memory

        private int _page = 0;
        public int Page                     /// The 256-byte page of memory to display
        {
            get => _page;
            set => Set(ref _page, value);
        }

        private string _hexDump;
        public string HexDump               /// the memory page as a string of hex digits and ascii characters
        {
            get => _hexDump;
            set => Set(ref _hexDump, value);
        }

        // Public Methods

            /// <summary>
            /// Populate the data context for the simulator with the CPU registers and flags
            /// </summary>
            /// <param name="cpu">The instance of CPU to display</param>
        public void DisplayCpuData(CPU cpu)
        {
            Accumulator = cpu.A;
            XRegister = cpu.X;
            YRegister = cpu.Y;
            StackPointer = cpu.SP;
            ProgramCounter = cpu.PC;
            StatusRegister = cpu.SR;
            Address = cpu.AddressBus;
            Data = cpu.DataBus;

            NFlag = cpu.N;
            VFlag = cpu.V;
            BFlag = cpu.B;
            DFlag = cpu.D;
            IFlag = cpu.I;
            ZFlag = cpu.Z;
            CFlag = cpu.C;
        }

        /// <summary>
        /// Display the the memory bytes in a page as both hexadecimal digits and ascii characters
        /// </summary>
        /// <param name="mem">The Memory instance display</param>
        /// <param name="page">the 256-byte page to display</param>
        /// <returns>a string repersentation of the memory page</returns>
        public string DisplayMemoryPageAsHexDump(Memory mem, int page)
        {
            StringBuilder hexString = new StringBuilder("");
            StringBuilder charString = new StringBuilder("    ");
            int i;
            int v;

            for (int row = 0; row < 16; row++)
            {
                hexString.Append(string.Format("{0:x4}:  ", (page << 8 | row << 4)));

                charString.Clear();
                charString.Append("    ");

                for (int col = 0; col < 16; col++)
                {
                    if (col == 8)
                    {
                        hexString.Append(" ");
                        charString.Append(" ");
                    }

                    i = page << 8 | row << 4 | col;
                    hexString.Append(string.Format(" {0:x2}", mem.Read(i)));

                    // Interpret byte as a printable ascii character , '.' otherwise
                    // -- ignore the high bit
                    v = mem.Read(i) & 0x7f;
                    if (v < 32 || v > 126)
                        charString.Append(".");
                    else
                        charString.Append((char)v);
                } // end col loop

                // append the ascii string to the row bytes
                hexString.Append(charString.ToString() + "\r\n");
            } //end row loop

            return hexString.ToString();
        }
    }
}
