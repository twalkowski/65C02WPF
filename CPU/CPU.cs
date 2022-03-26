
namespace _65C02WPF
{
    /// <summary>
    /// This represents the 65C02 CPU model along with an Address bus and a DATA bus
    /// </summary>
    public class CPU
    {
        public int A;
        public byte X;
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
        public byte DataBus;

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
            PC = 0x0000;
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

        public int Step(Memory mem)
        {
            A += 1;
            X += 1;
            Y += 1;
            SP += 1;
            PC += 1;
            SR += 1;
            AddressBus += 1;
            DataBus += 1;

            WriteData(AddressBus, DataBus, mem);

            ConvertSRToFlags(SR);

            return ExecuteInstruction(mem);


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

        private byte ReadData(int address, Memory mem)
        {
            return mem.Read(address);
        }

        private void WriteData(int address, byte data, Memory mem)
        {
            mem.Write(address, data);
        }

        private int ExecuteInstruction(Memory mem)
        {
            // Fetch the next instruction
            int opcode = ReadData(PC, mem);
            PC++;

            return Instructions[opcode].Cycles;
        }

        private Instruction[] Instructions =
        {
            new Instruction (0x00, "brk", AddressMode.Imp      ,1,  7,  false , Operation.BRK),
            new Instruction (0x01, "ora", AddressMode.ZpIxIndX ,2,  6,  false , Operation.ORA),
            new Instruction (0x02, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0x03, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x04, "tsb", AddressMode.Zp       ,2,  5,  false , Operation.TSB),
            new Instruction (0x05, "ora", AddressMode.Zp       ,2,  3,  false , Operation.ORA),
            new Instruction (0x06, "asl", AddressMode.Zp       ,2,  5,  false , Operation.ASL),
            new Instruction (0x07, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x08, "php", AddressMode.Stack    ,1,  3,  false , Operation.PHP),
            new Instruction (0x09, "ora", AddressMode.Imm      ,2,  2,  false , Operation.ORA),
            new Instruction (0x0A, "asl", AddressMode.Acc      ,1,  2,  false , Operation.ASL),
            new Instruction (0x0B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x0C, "tsb", AddressMode.Abs      ,3,  6,  false , Operation.TSB),
            new Instruction (0x0D, "ora", AddressMode.Abs      ,3,  4,  false , Operation.ORA),
            new Instruction (0x0E, "asl", AddressMode.Abs      ,3,  6,  false , Operation.ASL),
            new Instruction (0x0F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x10, "bpl", AddressMode.Rel      ,2,  2,  true  , Operation.BPL),
            new Instruction (0x11, "ora", AddressMode.ZpIndIxY ,2,  5,  true  , Operation.ORA),
            new Instruction (0x12, "ora", AddressMode.ZpInd    ,2,  5,  false , Operation.ORA),
            new Instruction (0x13, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x14, "trb", AddressMode.Zp       ,2,  5,  false , Operation.TRB),
            new Instruction (0x15, "ora", AddressMode.ZpX      ,2,  4,  false , Operation.ORA),
            new Instruction (0x16, "asl", AddressMode.ZpX      ,2,  6,  false , Operation.ASL),
            new Instruction (0x17, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x18, "clc", AddressMode.Imp      ,1,  2,  false , Operation.CLC),
            new Instruction (0x19, "ora", AddressMode.AbsY     ,3,  4,  true  , Operation.ORA),
            new Instruction (0x1A, "ina", AddressMode.Imp      ,1,  2,  false , Operation.INC),
            new Instruction (0x1B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x1C, "trb", AddressMode.Abs      ,3,  6,  false , Operation.TRB),
            new Instruction (0x1D, "ora", AddressMode.AbsX     ,3,  4,  true  , Operation.ORA),
            new Instruction (0x1E, "asl", AddressMode.AbsX     ,3,  6,  true  , Operation.ASL),
            new Instruction (0x1F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x20, "jsr", AddressMode.Abs      ,3,  6,  false , Operation.JSR),
            new Instruction (0x21, "and", AddressMode.ZpIxIndX ,2,  6,  false , Operation.AND),
            new Instruction (0x22, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0x23, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x24, "bit", AddressMode.Zp       ,2,  3,  false , Operation.BIT),
            new Instruction (0x25, "and", AddressMode.Zp       ,2,  3,  false , Operation.AND),
            new Instruction (0x26, "rol", AddressMode.Zp       ,2,  5,  false , Operation.ROL),
            new Instruction (0x27, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x28, "plp", AddressMode.Stack    ,1,  4,  false , Operation.PLP),
            new Instruction (0x29, "and", AddressMode.Imm      ,2,  2,  false , Operation.AND),
            new Instruction (0x2A, "rol", AddressMode.Acc      ,1,  2,  false , Operation.ROL),
            new Instruction (0x2B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x2C, "bit", AddressMode.Abs      ,3,  4,  false , Operation.BIT),
            new Instruction (0x2D, "and", AddressMode.Abs      ,3,  4,  false , Operation.AND),
            new Instruction (0x2E, "rol", AddressMode.Abs      ,3,  6,  false , Operation.NOP),
            new Instruction (0x2F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x30, "bmi", AddressMode.Rel      ,2,  2,  true  , Operation.BMI),
            new Instruction (0x31, "and", AddressMode.ZpIndIxY ,2,  5,  true  , Operation.AND),
            new Instruction (0x32, "and", AddressMode.ZpInd    ,2,  5,  false , Operation.AND),
            new Instruction (0x33, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x34, "bit", AddressMode.ZpX      ,2,  4,  false , Operation.BIT),
            new Instruction (0x35, "and", AddressMode.ZpX      ,2,  4,  false , Operation.AND),
            new Instruction (0x36, "rol", AddressMode.ZpX      ,2,  6,  false , Operation.ROL),
            new Instruction (0x37, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x38, "sec", AddressMode.Imp      ,1,  2,  false , Operation.SEC),
            new Instruction (0x39, "and", AddressMode.AbsY     ,3,  4,  true  , Operation.AND),
            new Instruction (0x3A, "dea", AddressMode.Imp      ,1,  2,  false , Operation.DEC),
            new Instruction (0x3B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x3C, "bit", AddressMode.AbsX     ,3,  4,  true  , Operation.BIT),
            new Instruction (0x3D, "and", AddressMode.AbsX     ,3,  4,  true  , Operation.AND),
            new Instruction (0x3E, "rol", AddressMode.AbsX     ,3,  6,  true  , Operation.ROL),
            new Instruction (0x3F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x40, "rti", AddressMode.Stack    ,1,  6,  false , Operation.RTI),
            new Instruction (0x41, "eor", AddressMode.ZpIxIndX ,2,  6,  false , Operation.EOR),
            new Instruction (0x42, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0x43, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x44, "???", AddressMode.Imp      ,2,  3,  false , Operation.NOP),
            new Instruction (0x45, "eor", AddressMode.Zp       ,2,  3,  false , Operation.EOR),
            new Instruction (0x46, "lsr", AddressMode.Zp       ,2,  5,  false , Operation.LSR),
            new Instruction (0x47, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
/*            new Instruction (0x48, "pha", AddressMode.Stack    ,1,  3,  false ),
            new Instruction (0x49, "eor", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0x4A, "lsr", AddressMode.Acc      ,1,  2,  false ),
            new Instruction (0x4B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x4C, "jmp", AddressMode.Abs      ,3,  3,  false ),
            new Instruction (0x4D, "eor", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x4E, "lsr", AddressMode.Abs      ,3,  6,  false ),
            new Instruction (0x4F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x50, "bvc", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0x51, "eor", AddressMode.ZpIndIxY ,2,  5,  true  ),
            new Instruction (0x52, "eor", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0x53, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x54, "???", AddressMode.Imp      ,2,  4,  false , Operation.NOP),
            new Instruction (0x55, "eor", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0x56, "lsr", AddressMode.ZpX      ,2,  6,  false ),
            new Instruction (0x57, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x58, "cli", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x59, "eor", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0x5A, "phy", AddressMode.Stack    ,1,  3,  false ),
            new Instruction (0x5B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x5C, "???", AddressMode.Imp      ,3,  8,  false , Operation.NOP),
            new Instruction (0x5D, "eor", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0x5E, "lsr", AddressMode.AbsX     ,3,  6,  true  ),
            new Instruction (0x5F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x60, "rts", AddressMode.Stack    ,1,  6,  false ),
            new Instruction (0x61, "adc", AddressMode.ZpIxIndX ,2,  6,  false ),
            new Instruction (0x62, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0x63, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x64, "stz", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0x65, "adc", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0x66, "ror", AddressMode.Zp       ,2,  5,  false ),
            new Instruction (0x67, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x68, "pla", AddressMode.Stack    ,1,  4,  false ),
            new Instruction (0x69, "adc", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0x6A, "ror", AddressMode.Acc      ,1,  2,  false ),
            new Instruction (0x6B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x6C, "jmp", AddressMode.AbsInd   ,3,  6,  false ),
            new Instruction (0x6D, "adc", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x6E, "ror", AddressMode.Abs      ,3,  6,  false ),
            new Instruction (0x6F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x70, "bvs", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0x71, "adc", AddressMode.ZpIndIxY ,2,  5,  true  ),
            new Instruction (0x72, "adc", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0x73, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x74, "stz", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0x75, "adc", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0x76, "ror", AddressMode.ZpX      ,2,  6,  false ),
            new Instruction (0x77, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x78, "sei", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x79, "adc", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0x7A, "ply", AddressMode.Stack    ,1,  4,  false ),
            new Instruction (0x7B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x7C, "jmp", AddressMode.AbsIxInd ,3,  6,  false ),
            new Instruction (0x7D, "adc", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0x7E, "ror", AddressMode.AbsX     ,3,  6,  true  ),
            new Instruction (0x7F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x80, "bra", AddressMode.Rel      ,1,  3,  true  ),
            new Instruction (0x81, "sta", AddressMode.ZpIxIndX ,2,  6,  false ),
            new Instruction (0x82, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0x83, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x84, "sty", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0x85, "sta", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0x86, "stx", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0x87, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x88, "dey", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x89, "bit", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0x8A, "txa", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x8B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x8C, "sty", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x8D, "sta", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x8E, "stx", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x8F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x90, "bcc", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0x91, "sta", AddressMode.ZpIndIxY ,2,  6,  false ),
            new Instruction (0x92, "sta", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0x93, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x94, "sty", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0x95, "sta", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0x96, "stx", AddressMode.ZpY      ,2,  4,  false ),
            new Instruction (0x97, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x98, "tya", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x99, "sta", AddressMode.AbsY     ,3,  5,  false ),
            new Instruction (0x9A, "txs", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0x9B, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0x9C, "stz", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0x9D, "sta", AddressMode.AbsX     ,3,  5,  false ),
            new Instruction (0x9E, "stz", AddressMode.AbsX     ,3,  5,  false ),
            new Instruction (0x9F, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xA0, "ldy", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xA1, "lda", AddressMode.ZpIxIndX ,2,  6,  false ),
            new Instruction (0xA2, "ldx", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xA3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xA4, "ldy", AddressMode.Zp       ,2,  3,  false , Operation.LDY),
            new Instruction (0xA5, "lda", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xA6, "ldx", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xA7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xA8, "tay", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xA9, "lda", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xAA, "tax", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xAB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xAC, "ldy", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xAD, "lda", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xAE, "ldx", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xAF, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xB0, "bcs", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0xB1, "lda", AddressMode.ZpIndIxY ,2,  5,  true  ),
            new Instruction (0xB2, "lda", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0xB3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xB4, "ldy", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0xB5, "lda", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0xB6, "ldx", AddressMode.ZpY      ,2,  4,  false ),
            new Instruction (0xB7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xB8, "clv", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xB9, "lda", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0xBA, "tsx", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xBB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xBC, "ldy", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0xBD, "lda", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0xBE, "ldx", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0xBF, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xC0, "cpy", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xC1, "cmp", AddressMode.ZpIxIndX ,2,  6,  false ),
            new Instruction (0xC2, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0xC3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xC4, "cpy", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xC5, "cmp", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xC6, "dec", AddressMode.Zp       ,2,  5,  false ),
            new Instruction (0xC7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xC8, "iny", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xC9, "cmp", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xCA, "dex", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xCB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xCC, "cpy", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xCD, "cmp", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xCE, "dec", AddressMode.Abs      ,3,  6,  false ),
            new Instruction (0xCF, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xD0, "bne", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0xD1, "cmp", AddressMode.ZpIndIxY ,2,  5,  true  ),
            new Instruction (0xD2, "cmp", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0xD3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xD4, "???", AddressMode.Imp      ,2,  4,  false , Operation.NOP),
            new Instruction (0xD5, "cmp", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0xD6, "dec", AddressMode.ZpX      ,2,  6,  false ),
            new Instruction (0xD7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xD8, "cld", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xD9, "cmp", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0xDA, "phx", AddressMode.Stack    ,1,  3,  false ),
            new Instruction (0xDB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xDC, "???", AddressMode.Imp      ,3,  4,  false , Operation.NOP),
            new Instruction (0xDD, "cmp", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0xDE, "dec", AddressMode.AbsX     ,3,  7,  false ),
            new Instruction (0xDF, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xE0, "cpx", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xE1, "sbc", AddressMode.ZpIxIndX ,2,  6,  false ),
            new Instruction (0xE2, "???", AddressMode.Imp      ,2,  2,  false , Operation.NOP),
            new Instruction (0xE3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xE4, "cpx", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xE5, "sbc", AddressMode.Zp       ,2,  3,  false ),
            new Instruction (0xE6, "inc", AddressMode.Zp       ,2,  5,  false ),
            new Instruction (0xE7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xE8, "inx", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xE9, "sbc", AddressMode.Imm      ,2,  2,  false ),
            new Instruction (0xEA, "nop", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xEB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xEC, "cpx", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xED, "sbc", AddressMode.Abs      ,3,  4,  false ),
            new Instruction (0xEE, "inc", AddressMode.Abs      ,3,  6,  false ),
            new Instruction (0xEF, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xF0, "beq", AddressMode.Rel      ,2,  2,  true  ),
            new Instruction (0xF1, "sbc", AddressMode.ZpIndIxY ,2,  5,  true  ),
            new Instruction (0xF2, "sbc", AddressMode.ZpInd    ,2,  5,  false ),
            new Instruction (0xF3, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xF4, "???", AddressMode.Imp      ,2,  4,  false , Operation.NOP),
            new Instruction (0xF5, "sbc", AddressMode.ZpX      ,2,  4,  false ),
            new Instruction (0xF6, "inc", AddressMode.ZpX      ,2,  6,  false ),
            new Instruction (0xF7, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xF8, "sed", AddressMode.Imp      ,1,  2,  false ),
            new Instruction (0xF9, "sbc", AddressMode.AbsY     ,3,  4,  true  ),
            new Instruction (0xFA, "plx", AddressMode.Stack    ,1,  4,  false ),
            new Instruction (0xFB, "???", AddressMode.Imp      ,1,  1,  false , Operation.NOP),
            new Instruction (0xFC, "???", AddressMode.Imp      ,3,  4,  false , Operation.NOP),
            new Instruction (0xFD, "sbc", AddressMode.AbsX     ,3,  4,  true  ),
            new Instruction (0xFE, "inc", AddressMode.AbsX     ,3,  7,  false ),
*/
            new Instruction (0xFF, "???", AddressMode.Imp      ,1,  1,  false ,Operation.NOP)
        };
    }
}
