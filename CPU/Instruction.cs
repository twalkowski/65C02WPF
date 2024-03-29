﻿using System;
using System.Collections.Generic;
using System.Text;

namespace _65C02WPF
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The Instruction class. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class Instruction
    {
        /// <summary>
        /// Properties
        /// </summary>
        /// 
        public int OpCode { get; private set; }

        public string Mnem { get; private set; }

        public int Byte1 { get; set; }

        public int Byte2 { get; set; }

        public AddressMode Mode { get; private set; }

        public int Length { get; private set; }

        public int Cycles { get; private set; }

        public bool XtraCycle { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        public Instruction(int opcode,
                            string mnem,
                            AddressMode mode,
                            int length,
                            int cycles,
                            bool xtraCycle,
                            Action func)
        {
            OpCode = opcode;
            Mnem = mnem;
            Byte1 = 0;
            Byte2 = 0;
            Mode = mode;
            Length = length;
            Cycles = cycles;
            XtraCycle = xtraCycle;
            //            DoWork = func;
        }


        /// <summary>
        /// Methods
        /// </summary>
        /// 
        public override string ToString()
        {
            return Mnem;
        }

        public Action DoWork;
        public Action GetAddress;

        ///----------
    }


    ///-------------------------------------------------------------------------------------------------
    /// <summary>    Values that represent address modes. </summary>
    ///
    /// <remarks>    Twalk, 5/20/2017. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum AddressMode
    {
        /// <summary>
        Abs,

        /// <summary> An enum constant representing the abs Index ind option. </summary>
        AbsIxInd,

        /// <summary> An enum constant representing the abs X coordinate option. </summary>
        AbsX,

        /// <summary> An enum constant representing the abs Y coordinate option. </summary>
        AbsY,

        /// <summary> An enum constant representing the abs ind option. </summary>
        AbsInd,

        /// <summary> An enum constant representing the Accumulate option. </summary>
        Acc,

        /// <summary> An enum constant representing the imm option. </summary>
        Imm,

        /// <summary> An enum constant representing the imp option. </summary>
        Imp,

        /// <summary> An enum constant representing the Relative option. </summary>
        Rel,

        /// <summary> An enum constant representing the stack option. </summary>
        Stack,

        /// <summary> An enum constant representing the zp option. </summary>
        Zp,

        /// <summary> An enum constant representing the zp Index ind X coordinate option. </summary>
        ZpIxIndX,

        /// <summary> An enum constant representing the zp X coordinate option. </summary>
        ZpX,

        /// <summary> An enum constant representing the zp Y coordinate option. </summary>
        ZpY,

        /// <summary> An enum constant representing the zp ind option. </summary>
        ZpInd,

        /// <summary> An enum constant representing the zp ind Index Y coordinate option. </summary>
        ZpIndIxY
    }
}
