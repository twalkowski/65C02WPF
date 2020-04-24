using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _65C02WPF
{
    [TestClass]
    public class MemoryTests
    {
        [TestMethod]
        public void MemoryLengthIsCorrect()
        {
            var memSize = 256;
            var testMem = new Memory(memSize);

            Assert.AreEqual(memSize, testMem.Length);
        }

        [TestMethod]
        public void MemoryReadReturnsValue()
        {
            var testMem = new Memory(256);
            byte val1 = 32;
            byte val2 = 200;
            var address = 50;

            testMem.Write(address, val1);
            Assert.AreEqual(val1, testMem.Read(address));

            testMem.Write(address, val2);
            Assert.AreEqual(val2, testMem.Read(address));
        }
    }
}
