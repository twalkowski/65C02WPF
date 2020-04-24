using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _65C02WPF
{
    [TestClass]
    public class CPUTests
    {
        [TestMethod]
        public void TestReset()
        {
            CPU cpu = new CPU();

            int ExA = 0x20;
            int ExX = 0;
            int ExY = 0;
            int ExSP = 0xff;
            int ExPC = 0xfffc;
            int ExSR = 0;
            int ExAddressBus = 0;
            int ExDataBus = 0;

            cpu.Reset();

            Assert.AreEqual(cpu.A, ExA);
            Assert.AreEqual(cpu.X, ExX);
            Assert.AreEqual(cpu.Y, ExY);
            Assert.AreEqual(cpu.SP, ExSP);
            Assert.AreEqual(cpu.PC, ExPC);
            Assert.AreEqual(cpu.SR, ExSR);
            Assert.AreEqual(cpu.AddressBus, ExAddressBus);
            Assert.AreEqual(cpu.DataBus, ExDataBus);
        }
    }
}
