using System;
using System.Collections.Generic;
using System.Text;

namespace _65C02WPF
{
    public class Instruction
    {
        public int OpCode;
        public string Mnemonic;
        public int Length;
        public int Cycles;
        public int Low;
        public int Hi;
        public Action<Instruction> DoIt;
        public AddressMode Mode;


        /// <summary>
        ///  The available 65C02 addressing modes
        /// </summary>
        public enum AddressMode
        {
            Immediate,
            Absolute
        }


    }
}
