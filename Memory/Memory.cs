using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _65C02WPF
{
    public class Memory
    {
        private byte[] memory;
        public int Length 
        { 
            get { return memory.Length; }
        }

        public Memory(int size)
        {
            memory = new byte[size];
        }

        public byte Read(int address)
        {
            if ((address < 0) || (address > memory.Length))
            {
                throw new IndexOutOfRangeException();
            }

            return memory[address];
        }

        public void Write(int address, byte value)
        {
            if ((address < 0) || (address > memory.Length))
            {
                throw new IndexOutOfRangeException();
            }

            memory[address] = value;
        }

        public void Initialize()
        {
            for (int i = 0; i < memory.Length; i++)
            {
//                memory[i] = (byte)i;
                memory[i] = (byte)((i & 0xff00) >> 8);
            }
        }

    }
}
