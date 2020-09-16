namespace _65C02WPF
{
    /// <summary>
    /// This represents the 65C02 CPU model along with an Address bus and a DATA bus
    /// </summary>
    public class CPU
    {
        public int A;
        public int X;
        public int Y;

        private int sp;
        public int SP
        {
            get { return sp; }
            set { sp = 0x0100 + (value & 0x0ff); }
        }

        private int pc;
        public int PC
        {
            get { return pc; }
            set { pc = (value & 0xffff); }
        }

        public byte SR;
        public int AddressBus;
        public int DataBus;

        public bool N = false;
        public bool V = false;
        public bool B = false;
        public bool D = false;
        public bool I = false;
        public bool Z = false;
        public bool C = false;

        public void Reset()
        {
            A = 0;
            X = 0;
            Y = 0;
            SP = 0x1ff;
            PC = 0xfffc;
            AddressBus = 0;
            DataBus = 0;

            N = false;
            V = false;
            B = false;
            D = false;
            I = false;
            Z = false;
            C = false;

            SR = ConvertFlagsToSR();
        }

        public void Step()
        {
            A += 1;
            X += 1;
            Y += 1;
            SP += 1;
            PC += 1;
            SR += 1;
            AddressBus += 1;
            DataBus += 1;

            ConvertSRToFlags(SR);


            /// 
            /// TODO  code to execute a single instruction
            /// 
        }

        private byte ConvertFlagsToSR()
        {
            var v = ((N ? 0x80 : 0)
                  + (V ? 0x40 : 0)
                  + 0x20  // unused bit in SR
                  + (B ? 0x10 : 0)
                  + (D ? 0x08 : 0)
                  + (I ? 0x04 : 0)
                  + (Z ? 0x02 : 0)
                  + (C ? 0x01 : 0)
                  );
            return (byte)v;
        }

        private void ConvertSRToFlags(byte value)
        {
            C = (value & 0x01) != 0;
            Z = (value & 0x02) != 0;
            I = (value & 0x04) != 0;
            D = (value & 0x08) != 0;
            B = (value & 0x10) != 0;
            V = (value & 0x40) != 0;
            N = (value & 0x80) != 0;
        }


    }
}
