
using System.Text;

namespace _65C02WPF
{
    /// <summary>
    /// This represents the 65C02 CPU model along with an Address bus and a DATA bus
    /// </summary>
    /// 

    public class CPU
    {
        public byte A;
        public byte X;
        public byte Y;

        private byte sp;
        public short SP
        {
            get { return (short)(sp + 0x0100); }
            set { sp = (byte)value; }
        }

        public short PC;

        public byte SR
        {
            get { return FlagsToSR(); }
            set { SRToFlags((byte)value); }
        }

        public short AddressBus;
        public byte DataBus;

        public bool N = false;
        public bool V = false;
        public bool B = false;
        public bool D = false;
        public bool I = false;
        public bool Z = false;
        public bool C = false;

        public string distext = " ";

        public void Reset()
        {
            A = 0;
            X = 0;
            Y = 0;
            sp = 0xff;
            PC = 0x0000;
            AddressBus = 0;
            DataBus = 0;

            N = false;
            V = false;
            B = true;
            D = false;
            I = true;
            Z = false;
            C = false;

            SR = FlagsToSR();
        }

        public int Step(ref Memory mem)
        {
            int cycles = ExecuteInstruction(
                ref mem);
            mem.Write(AddressBus, DataBus);
            SRToFlags(SR);

            A += 1;
            X += 1;
            Y += 1;
            SP += 1;
            PC += 1;
            SR += 1;
            AddressBus += 1;
            DataBus += 1;
            /// 
            /// TODO  code to execute a single instruction
            /// 
            return cycles;
        }

        private byte FlagsToSR()
        {
            var v = ((N ? 0x80 : 0)
                  + (V ? 0x40 : 0)
                  + 0x20  // unused bit in SR always true
                  + (B ? 0x10 : 0)
                  + (D ? 0x08 : 0)
                  + (I ? 0x04 : 0)
                  + (Z ? 0x02 : 0)
                  + (C ? 0x01 : 0)
                  );
            return (byte)v;
        }

        private void SRToFlags(byte value)
        {
            C = (value & 0x01) != 0;
            Z = (value & 0x02) != 0;
            I = (value & 0x04) != 0;
            D = (value & 0x08) != 0;
            B = (value & 0x10) != 0;
            // unused flag at 0x20
            V = (value & 0x40) != 0;
            N = (value & 0x80) != 0;
        }

        private int ExecuteInstruction(ref Memory mem)
        {
            StringBuilder myString = new StringBuilder();

            // Fetch the next instruction
            int opcode = mem.Read(PC);

            distext = Instructions[opcode].ToString();

            PC++;

            return Instructions[opcode].Cycles;
        }

        private Instruction[] Instructions =
        {
            // Opcodes $00 - $0f
            new Instruction (0x00, "brk  #${0:x2}", AddrMode.Imp      ,2,  7,  false , Oper.BRK),
            new Instruction (0x01, "ora", AddrMode.ZpIxIndX ,2,  6,  false , Oper.ORA),
            new Instruction (0x02, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0x03, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x04, "tsb", AddrMode.Zp       ,2,  5,  false , Oper.TSB),
            new Instruction (0x05, "ora", AddrMode.Zp       ,2,  3,  false , Oper.ORA),
            new Instruction (0x06, "asl", AddrMode.Zp       ,2,  5,  false , Oper.ASL),
            new Instruction (0x07, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x08, "php", AddrMode.Stack    ,1,  3,  false , Oper.PHP),
            new Instruction (0x09, "ora", AddrMode.Imm      ,2,  2,  false , Oper.ORA),
            new Instruction (0x0A, "asl", AddrMode.Acc      ,1,  2,  false , Oper.ASL),
            new Instruction (0x0B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x0C, "tsb", AddrMode.Abs      ,3,  6,  false , Oper.TSB),
            new Instruction (0x0D, "ora", AddrMode.Abs      ,3,  4,  false , Oper.ORA),
            new Instruction (0x0E, "asl", AddrMode.Abs      ,3,  6,  false , Oper.ASL),
            new Instruction (0x0F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $10-$1f
            new Instruction (0x10, "bpl", AddrMode.Rel      ,2,  2,  true  , Oper.BPL),
            new Instruction (0x11, "ora", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.ORA),
            new Instruction (0x12, "ora", AddrMode.ZpInd    ,2,  5,  false , Oper.ORA),
            new Instruction (0x13, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x14, "trb", AddrMode.Zp       ,2,  5,  false , Oper.TRB),
            new Instruction (0x15, "ora", AddrMode.ZpX      ,2,  4,  false , Oper.ORA),
            new Instruction (0x16, "asl", AddrMode.ZpX      ,2,  6,  false , Oper.ASL),
            new Instruction (0x17, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x18, "clc", AddrMode.Imp      ,1,  2,  false , Oper.CLC),
            new Instruction (0x19, "ora", AddrMode.AbsY     ,3,  4,  true  , Oper.ORA),
            new Instruction (0x1A, "ina", AddrMode.Imp      ,1,  2,  false , Oper.INC),
            new Instruction (0x1B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x1C, "trb", AddrMode.Abs      ,3,  6,  false , Oper.TRB),
            new Instruction (0x1D, "ora", AddrMode.AbsX     ,3,  4,  true  , Oper.ORA),
            new Instruction (0x1E, "asl", AddrMode.AbsX     ,3,  6,  true  , Oper.ASL),
            new Instruction (0x1F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $20-$2f
            new Instruction (0x20, "jsr", AddrMode.Abs      ,3,  6,  false , Oper.JSR),
            new Instruction (0x21, "and", AddrMode.ZpIxIndX ,2,  6,  false , Oper.AND),
            new Instruction (0x22, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0x23, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x24, "bit", AddrMode.Zp       ,2,  3,  false , Oper.BIT),
            new Instruction (0x25, "and", AddrMode.Zp       ,2,  3,  false , Oper.AND),
            new Instruction (0x26, "rol", AddrMode.Zp       ,2,  5,  false , Oper.ROL),
            new Instruction (0x27, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x28, "plp", AddrMode.Stack    ,1,  4,  false , Oper.PLP),
            new Instruction (0x29, "and", AddrMode.Imm      ,2,  2,  false , Oper.AND),
            new Instruction (0x2A, "rol", AddrMode.Acc      ,1,  2,  false , Oper.ROL),
            new Instruction (0x2B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x2C, "bit", AddrMode.Abs      ,3,  4,  false , Oper.BIT),
            new Instruction (0x2D, "and", AddrMode.Abs      ,3,  4,  false , Oper.AND),
            new Instruction (0x2E, "rol", AddrMode.Abs      ,3,  6,  false , Oper.NOP),
            new Instruction (0x2F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $30-$3f
            new Instruction (0x30, "bmi", AddrMode.Rel      ,2,  2,  true  , Oper.BMI),
            new Instruction (0x31, "and", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.AND),
            new Instruction (0x32, "and", AddrMode.ZpInd    ,2,  5,  false , Oper.AND),
            new Instruction (0x33, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x34, "bit", AddrMode.ZpX      ,2,  4,  false , Oper.BIT),
            new Instruction (0x35, "and", AddrMode.ZpX      ,2,  4,  false , Oper.AND),
            new Instruction (0x36, "rol", AddrMode.ZpX      ,2,  6,  false , Oper.ROL),
            new Instruction (0x37, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x38, "sec", AddrMode.Imp      ,1,  2,  false , Oper.SEC),
            new Instruction (0x39, "and", AddrMode.AbsY     ,3,  4,  true  , Oper.AND),
            new Instruction (0x3A, "dea", AddrMode.Imp      ,1,  2,  false , Oper.DEC),
            new Instruction (0x3B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x3C, "bit", AddrMode.AbsX     ,3,  4,  true  , Oper.BIT),
            new Instruction (0x3D, "and", AddrMode.AbsX     ,3,  4,  true  , Oper.AND),
            new Instruction (0x3E, "rol", AddrMode.AbsX     ,3,  6,  true  , Oper.ROL),
            new Instruction (0x3F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $40-$4f
            new Instruction (0x40, "rti", AddrMode.Stack    ,1,  6,  false , Oper.RTI),
            new Instruction (0x41, "eor", AddrMode.ZpIxIndX ,2,  6,  false , Oper.EOR),
            new Instruction (0x42, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0x43, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x44, "???", AddrMode.Imp      ,2,  3,  false , Oper.NOP),
            new Instruction (0x45, "eor", AddrMode.Zp       ,2,  3,  false , Oper.EOR),
            new Instruction (0x46, "lsr", AddrMode.Zp       ,2,  5,  false , Oper.LSR),
            new Instruction (0x47, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x48, "pha", AddrMode.Stack    ,1,  3,  false , Oper.PHA),
            new Instruction (0x49, "eor", AddrMode.Imm      ,2,  2,  false , Oper.EOR),
            new Instruction (0x4A, "lsr", AddrMode.Acc      ,1,  2,  false , Oper.LSR),
            new Instruction (0x4B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x4C, "jmp", AddrMode.Abs      ,3,  3,  false , Oper.JMP),
            new Instruction (0x4D, "eor", AddrMode.Abs      ,3,  4,  false , Oper.EOR),
            new Instruction (0x4E, "lsr", AddrMode.Abs      ,3,  6,  false , Oper.LSR),
            new Instruction (0x4F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $50-$5f
            new Instruction (0x50, "bvc", AddrMode.Rel      ,2,  2,  true  , Oper.BVC),
            new Instruction (0x51, "eor", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.EOR),
            new Instruction (0x52, "eor", AddrMode.ZpInd    ,2,  5,  false , Oper.EOR),
            new Instruction (0x53, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x54, "???", AddrMode.Imp      ,2,  4,  false , Oper.NOP),
            new Instruction (0x55, "eor", AddrMode.ZpX      ,2,  4,  false , Oper.EOR),
            new Instruction (0x56, "lsr", AddrMode.ZpX      ,2,  6,  false , Oper.LSR),
            new Instruction (0x57, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x58, "cli", AddrMode.Imp      ,1,  2,  false , Oper.CLI),
            new Instruction (0x59, "eor", AddrMode.AbsY     ,3,  4,  true  , Oper.EOR),
            new Instruction (0x5A, "phy", AddrMode.Stack    ,1,  3,  false , Oper.PHY),
            new Instruction (0x5B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x5C, "???", AddrMode.Imp      ,3,  8,  false , Oper.NOP),
            new Instruction (0x5D, "eor", AddrMode.AbsX     ,3,  4,  true  , Oper.EOR),
            new Instruction (0x5E, "lsr", AddrMode.AbsX     ,3,  6,  true  , Oper.LSR),
            new Instruction (0x5F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $60-$6f
            new Instruction (0x60, "rts", AddrMode.Stack    ,1,  6,  false , Oper.RTS),
            new Instruction (0x61, "adc", AddrMode.ZpIxIndX ,2,  6,  false , Oper.ADC),
            new Instruction (0x62, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0x63, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x64, "stz", AddrMode.Zp       ,2,  3,  false , Oper.STZ),
            new Instruction (0x65, "adc", AddrMode.Zp       ,2,  3,  false , Oper.ADC),
            new Instruction (0x66, "ror", AddrMode.Zp       ,2,  5,  false , Oper.ROR),
            new Instruction (0x67, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x68, "pla", AddrMode.Stack    ,1,  4,  false , Oper.PLA),
            new Instruction (0x69, "adc", AddrMode.Imm      ,2,  2,  false , Oper.ADC),
            new Instruction (0x6A, "ror", AddrMode.Acc      ,1,  2,  false , Oper.ROR),
            new Instruction (0x6B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x6C, "jmp", AddrMode.AbsInd   ,3,  6,  false , Oper.JMP),
            new Instruction (0x6D, "adc", AddrMode.Abs      ,3,  4,  false , Oper.ADC),
            new Instruction (0x6E, "ror", AddrMode.Abs      ,3,  6,  false , Oper.ROR),
            new Instruction (0x6F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $70-$7f
            new Instruction (0x70, "bvs", AddrMode.Rel      ,2,  2,  true  , Oper.BVS),
            new Instruction (0x71, "adc", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.ADC),
            new Instruction (0x72, "adc", AddrMode.ZpInd    ,2,  5,  false , Oper.ADC),
            new Instruction (0x73, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x74, "stz", AddrMode.ZpX      ,2,  4,  false , Oper.STZ),
            new Instruction (0x75, "adc", AddrMode.ZpX      ,2,  4,  false , Oper.ADC),
            new Instruction (0x76, "ror", AddrMode.ZpX      ,2,  6,  false , Oper.ROR),
            new Instruction (0x77, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x78, "sei", AddrMode.Imp      ,1,  2,  false , Oper.SEI),
            new Instruction (0x79, "adc", AddrMode.AbsY     ,3,  4,  true  , Oper.ADC),
            new Instruction (0x7A, "ply", AddrMode.Stack    ,1,  4,  false , Oper.PLY),
            new Instruction (0x7B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x7C, "jmp", AddrMode.AbsIxInd ,3,  6,  false , Oper.JMP),
            new Instruction (0x7D, "adc", AddrMode.AbsX     ,3,  4,  true  , Oper.ADC),
            new Instruction (0x7E, "ror", AddrMode.AbsX     ,3,  6,  true  , Oper.ROR),
            new Instruction (0x7F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $80-$8f
            new Instruction (0x80, "bra", AddrMode.Rel      ,1,  3,  true  , Oper.BRA),
            new Instruction (0x81, "sta", AddrMode.ZpIxIndX ,2,  6,  false , Oper.STA),
            new Instruction (0x82, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0x83, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x84, "sty", AddrMode.Zp       ,2,  3,  false , Oper.STY),
            new Instruction (0x85, "sta", AddrMode.Zp       ,2,  3,  false , Oper.STA),
            new Instruction (0x86, "stx", AddrMode.Zp       ,2,  3,  false , Oper.STX),
            new Instruction (0x87, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x88, "dey", AddrMode.Imp      ,1,  2,  false , Oper.DEY),
            new Instruction (0x89, "bit", AddrMode.Imm      ,2,  2,  false , Oper.BIT),
            new Instruction (0x8A, "txa", AddrMode.Imp      ,1,  2,  false , Oper.TXA),
            new Instruction (0x8B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x8C, "sty", AddrMode.Abs      ,3,  4,  false , Oper.STY),
            new Instruction (0x8D, "sta", AddrMode.Abs      ,3,  4,  false , Oper.STA),
            new Instruction (0x8E, "stx", AddrMode.Abs      ,3,  4,  false , Oper.STX),
            new Instruction (0x8F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $90-$9f
            new Instruction (0x90, "bcc", AddrMode.Rel      ,2,  2,  true  , Oper.BCC),
            new Instruction (0x91, "sta", AddrMode.ZpIndIxY ,2,  6,  false , Oper.STA),
            new Instruction (0x92, "sta", AddrMode.ZpInd    ,2,  5,  false , Oper.STA),
            new Instruction (0x93, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x94, "sty", AddrMode.ZpX      ,2,  4,  false , Oper.STY),
            new Instruction (0x95, "sta", AddrMode.ZpX      ,2,  4,  false , Oper.STA),
            new Instruction (0x96, "stx", AddrMode.ZpY      ,2,  4,  false , Oper.STX),
            new Instruction (0x97, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x98, "tya", AddrMode.Imp      ,1,  2,  false , Oper.TYA),
            new Instruction (0x99, "sta", AddrMode.AbsY     ,3,  5,  false , Oper.STA),
            new Instruction (0x9A, "txs", AddrMode.Imp      ,1,  2,  false , Oper.TXS),
            new Instruction (0x9B, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0x9C, "stz", AddrMode.Abs      ,3,  4,  false , Oper.STZ),
            new Instruction (0x9D, "sta", AddrMode.AbsX     ,3,  5,  false , Oper.STA),
            new Instruction (0x9E, "stz", AddrMode.AbsX     ,3,  5,  false , Oper.STZ),
            new Instruction (0x9F, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $a0-$af
            new Instruction (0xA0, "ldy", AddrMode.Imm      ,2,  2,  false , Oper.LDY),
            new Instruction (0xA1, "lda", AddrMode.ZpIxIndX ,2,  6,  false , Oper.LDA),
            new Instruction (0xA2, "ldx", AddrMode.Imm      ,2,  2,  false , Oper.LDX),
            new Instruction (0xA3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xA4, "ldy", AddrMode.Zp       ,2,  3,  false , Oper.LDY),
            new Instruction (0xA5, "lda", AddrMode.Zp       ,2,  3,  false , Oper.LDA),
            new Instruction (0xA6, "ldx", AddrMode.Zp       ,2,  3,  false , Oper.LDX),
            new Instruction (0xA7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xA8, "tay", AddrMode.Imp      ,1,  2,  false , Oper.TAY),
            new Instruction (0xA9, "lda", AddrMode.Imm      ,2,  2,  false , Oper.LDA),
            new Instruction (0xAA, "tax", AddrMode.Imp      ,1,  2,  false , Oper.TAX),
            new Instruction (0xAB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xAC, "ldy", AddrMode.Abs      ,3,  4,  false , Oper.LDY),
            new Instruction (0xAD, "lda", AddrMode.Abs      ,3,  4,  false , Oper.LDA),
            new Instruction (0xAE, "ldx", AddrMode.Abs      ,3,  4,  false , Oper.LDX),
            new Instruction (0xAF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $b0-$bf
            new Instruction (0xB0, "bcs", AddrMode.Rel      ,2,  2,  true  , Oper.BCS),
            new Instruction (0xB1, "lda", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.LDA),
            new Instruction (0xB2, "lda", AddrMode.ZpInd    ,2,  5,  false , Oper.LDA),
            new Instruction (0xB3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xB4, "ldy", AddrMode.ZpX      ,2,  4,  false , Oper.LDY),
            new Instruction (0xB5, "lda", AddrMode.ZpX      ,2,  4,  false , Oper.LDA),
            new Instruction (0xB6, "ldx", AddrMode.ZpY      ,2,  4,  false , Oper.LDX),
            new Instruction (0xB7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xB8, "clv", AddrMode.Imp      ,1,  2,  false , Oper.CLV),
            new Instruction (0xB9, "lda", AddrMode.AbsY     ,3,  4,  true  , Oper.LDA),
            new Instruction (0xBA, "tsx", AddrMode.Imp      ,1,  2,  false , Oper.TSX),
            new Instruction (0xBB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xBC, "ldy", AddrMode.AbsX     ,3,  4,  true  , Oper.LDY),
            new Instruction (0xBD, "lda", AddrMode.AbsX     ,3,  4,  true  , Oper.LDA),
            new Instruction (0xBE, "ldx", AddrMode.AbsY     ,3,  4,  true  , Oper.LDX),
            new Instruction (0xBF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $c0-$cf
            new Instruction (0xC0, "cpy", AddrMode.Imm      ,2,  2,  false , Oper.CPY),
            new Instruction (0xC1, "cmp", AddrMode.ZpIxIndX ,2,  6,  false , Oper.CMP),
            new Instruction (0xC2, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0xC3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xC4, "cpy", AddrMode.Zp       ,2,  3,  false , Oper.CPY),
            new Instruction (0xC5, "cmp", AddrMode.Zp       ,2,  3,  false , Oper.CMP),
            new Instruction (0xC6, "dec", AddrMode.Zp       ,2,  5,  false , Oper.DEC),
            new Instruction (0xC7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xC8, "iny", AddrMode.Imp      ,1,  2,  false , Oper.INY),
            new Instruction (0xC9, "cmp", AddrMode.Imm      ,2,  2,  false , Oper.CMP),
            new Instruction (0xCA, "dex", AddrMode.Imp      ,1,  2,  false , Oper.DEX),
            new Instruction (0xCB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xCC, "cpy", AddrMode.Abs      ,3,  4,  false , Oper.CPY),
            new Instruction (0xCD, "cmp", AddrMode.Abs      ,3,  4,  false , Oper.CMP),
            new Instruction (0xCE, "dec", AddrMode.Abs      ,3,  6,  false , Oper.DEC),
            new Instruction (0xCF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $d0-$df
            new Instruction (0xD0, "bne", AddrMode.Rel      ,2,  2,  true  , Oper.BNE),
            new Instruction (0xD1, "cmp", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.CMP),
            new Instruction (0xD2, "cmp", AddrMode.ZpInd    ,2,  5,  false , Oper.CMP),
            new Instruction (0xD3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xD4, "???", AddrMode.Imp      ,2,  4,  false , Oper.NOP),
            new Instruction (0xD5, "cmp", AddrMode.ZpX      ,2,  4,  false , Oper.CMP),
            new Instruction (0xD6, "dec", AddrMode.ZpX      ,2,  6,  false , Oper.DEC),
            new Instruction (0xD7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xD8, "cld", AddrMode.Imp      ,1,  2,  false , Oper.CLD),
            new Instruction (0xD9, "cmp", AddrMode.AbsY     ,3,  4,  true  , Oper.CMP),
            new Instruction (0xDA, "phx", AddrMode.Stack    ,1,  3,  false , Oper.PHX),
            new Instruction (0xDB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xDC, "???", AddrMode.Imp      ,3,  4,  false , Oper.NOP),
            new Instruction (0xDD, "cmp", AddrMode.AbsX     ,3,  4,  true  , Oper.CMP),
            new Instruction (0xDE, "dec", AddrMode.AbsX     ,3,  7,  false , Oper.DEC),
            new Instruction (0xDF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $e0-$ef
            new Instruction (0xE0, "cpx", AddrMode.Imm      ,2,  2,  false , Oper.CPX),
            new Instruction (0xE1, "sbc", AddrMode.ZpIxIndX ,2,  6,  false , Oper.SBC),
            new Instruction (0xE2, "???", AddrMode.Imp      ,2,  2,  false , Oper.NOP),
            new Instruction (0xE3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xE4, "cpx", AddrMode.Zp       ,2,  3,  false , Oper.CPX),
            new Instruction (0xE5, "sbc", AddrMode.Zp       ,2,  3,  false , Oper.SBC),
            new Instruction (0xE6, "inc", AddrMode.Zp       ,2,  5,  false , Oper.INC),
            new Instruction (0xE7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xE8, "inx", AddrMode.Imp      ,1,  2,  false , Oper.INX),
            new Instruction (0xE9, "sbc", AddrMode.Imm      ,2,  2,  false , Oper.SBC),
            new Instruction (0xEA, "nop", AddrMode.Imp      ,1,  2,  false , Oper.NOP),
            new Instruction (0xEB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xEC, "cpx", AddrMode.Abs      ,3,  4,  false , Oper.CPX),
            new Instruction (0xED, "sbc", AddrMode.Abs      ,3,  4,  false , Oper.SBC),
            new Instruction (0xEE, "inc", AddrMode.Abs      ,3,  6,  false , Oper.INC),
            new Instruction (0xEF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),

            //Opcodes $f0-$ff
            new Instruction (0xF0, "beq", AddrMode.Rel      ,2,  2,  true  , Oper.BEQ),
            new Instruction (0xF1, "sbc", AddrMode.ZpIndIxY ,2,  5,  true  , Oper.SBC),
            new Instruction (0xF2, "sbc", AddrMode.ZpInd    ,2,  5,  false , Oper.SBC),
            new Instruction (0xF3, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xF4, "???", AddrMode.Imp      ,2,  4,  false , Oper.NOP),
            new Instruction (0xF5, "sbc", AddrMode.ZpX      ,2,  4,  false , Oper.SBC),
            new Instruction (0xF6, "inc", AddrMode.ZpX      ,2,  6,  false , Oper.INC),
            new Instruction (0xF7, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xF8, "sed", AddrMode.Imp      ,1,  2,  false , Oper.SED),
            new Instruction (0xF9, "sbc", AddrMode.AbsY     ,3,  4,  true  , Oper.SBC),
            new Instruction (0xFA, "plx", AddrMode.Stack    ,1,  4,  false , Oper.PLX),
            new Instruction (0xFB, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP),
            new Instruction (0xFC, "???", AddrMode.Imp      ,3,  4,  false , Oper.NOP),
            new Instruction (0xFD, "sbc", AddrMode.AbsX     ,3,  4,  true  , Oper.SBC),
            new Instruction (0xFE, "inc", AddrMode.AbsX     ,3,  7,  false , Oper.INC),
            new Instruction (0xFF, "???", AddrMode.Imp      ,1,  1,  false , Oper.NOP)
        };
    }
}
