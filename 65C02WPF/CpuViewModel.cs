using System.ComponentModel;
using System.Text;

namespace _65C02WPF
{
    class CpuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Statitics

        private int instructions;
        public int Instructions
        {
            get { return instructions; }
            set
            {
                if (instructions != value)
                {
                    instructions = value;
                    OnPropertyChanged(nameof(Instructions));
                }
            }
        }

        private ulong cycles;
        public ulong Cycles
        {
            get { return cycles; }
            set
            {
                if (cycles != value)
                {
                    cycles = value;
                    OnPropertyChanged(nameof(Cycles));
                }
            }
        }


        // CPU

        private int accumulator;
        public int Accumulator
        {
            get { return accumulator; }
            set
            {
                if (accumulator != value)
                {
                    accumulator = value;
                    OnPropertyChanged(nameof(Accumulator));
                }
            }
        }

        private int xRegister;
        public int XRegister
        {
            get { return xRegister; }
            set
            {
                if (xRegister != value)
                {
                    xRegister = value;
                    OnPropertyChanged(nameof(XRegister));
                }
            }
        }

        private int yRegister;
        public int YRegister
        {
            get { return yRegister; }
            set
            {
                if (yRegister != value)
                {
                    yRegister = value;
                    OnPropertyChanged(nameof(YRegister));
                }
            }
        }

        private int stackPointer;
        public int StackPointer
        {
            get { return stackPointer; }
            set
            {
                if (stackPointer != value)
                {
                    stackPointer = value;
                    OnPropertyChanged(nameof(StackPointer));
                }
            }
        }

        private int programCounter;
        public int ProgramCounter
        {
            get { return programCounter; }
            set
            {
                if (programCounter != value)
                {
                    programCounter = value;
                    OnPropertyChanged(nameof(ProgramCounter));
                }
            }
        }

        private int statusRegister;
        public int StatusRegister
        {
            get { return statusRegister; }
            set
            {
                if (statusRegister != value)
                {
                    statusRegister = value;
                    OnPropertyChanged(nameof(StatusRegister));
                }
            }
        }

        private int data;
        public int Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    OnPropertyChanged(nameof(Data));
                }
            }
        }

        private int address;
        public int Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        private bool nFlag;
        public bool NFlag
        {
            get { return nFlag; }
            set
            {
                if (nFlag != value)
                {
                    nFlag = value;
                    OnPropertyChanged(nameof(NFlag));
                }
            }
        }

        private bool vFlag;
        public bool VFlag
        {
            get { return vFlag; }
            set
            {
                if (vFlag != value)
                {
                    vFlag = value;
                    OnPropertyChanged(nameof(VFlag));
                }
            }
        }

        private bool bFlag;
        public bool BFlag
        {
            get { return bFlag; }
            set
            {
                if (bFlag != value)
                {
                    bFlag = value;
                    OnPropertyChanged(nameof(BFlag));
                }
            }
        }

        private bool dFlag;
        public bool DFlag
        {
            get { return dFlag; }
            set
            {
                if (dFlag != value)
                {
                    dFlag = value;
                    OnPropertyChanged(nameof(DFlag));
                }
            }
        }

        private bool iFlag;
        public bool IFlag
        {
            get { return iFlag; }
            set
            {
                if (iFlag != value)
                {
                    iFlag = value;
                    OnPropertyChanged(nameof(IFlag));
                }
            }
        }

        private bool zFlag;
        public bool ZFlag
        {
            get { return zFlag; }
            set
            {
                if (zFlag != value)
                {
                    zFlag = value;
                    OnPropertyChanged(nameof(ZFlag));
                }
            }
        }

        private bool cFlag;
        public bool CFlag
        {
            get { return cFlag; }
            set
            {
                if (cFlag != value)
                {
                    cFlag = value;
                    OnPropertyChanged(nameof(CFlag));
                }
            }
        }

        // Memory

        private int page = 0;
        public int Page
        {
            get { return page; }
            set
            {
                if (page != value)
                {
                    page = value;
                    OnPropertyChanged(nameof(Page));
                }
            }
        }

        private string hexDump;
        public string HexDump
        {
            get { return hexDump; }
            set
            {
                if (hexDump != value)
                {
                    hexDump = value;
                    OnPropertyChanged(nameof(HexDump));
                }
            }
        }

        // Public Methods

        public void UpdateView(CPU cpu)
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

        public string DumpMemAsHex(Memory mem, int page)
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
