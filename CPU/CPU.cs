using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _65C02WPF
{
    public class CPU
    {
        public int A;
        public byte X;
        public int Y;
        public int SP;
        public int PC;
        public byte SR
        {
            get => ConvertFlagsToSR();
            set { }
        }
        public int AddressBus;
        public int DataBus;

        public byte testFlags;

        public bool N;
        public bool V;
        public bool B;
        public bool D;
        public bool I;
        public bool Z;
        public bool C;

        public void Reset()
        {
            A = 32;
            X = 0;
            Y = 0;
            SP = 0xff;
            PC = 0xfffc;
            AddressBus = 0;
            DataBus = 0;
            testFlags = 0;

            N = true;
            V = true;
            B = true;
            D = true;
            I = true;
            Z = true;
            C = true;
        }

        public void Step()
        {
            A += 1;
            X += 2;
            Y += 3;
            SP += 4;
            PC += 5;
            AddressBus += 7;
            DataBus += 8;
            testFlags += 1;
            ConvertSRToFlags(testFlags);

            // TODO  code to execute a single instruction
        }
        
        private byte ConvertFlagsToSR()
        {
            int v = ((N ? 0x80 : 0)
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
            N = (value & 0x80) != 0;
            V = (value & 0x40) != 0;
            B = (value & 0x10) != 0;     
            D = (value & 0x08) != 0;
            I = (value & 0x04) != 0;
            Z = (value & 0x02) != 0;
            C = (value & 0x01) != 0;
        }


    }
}
