using GameCenter.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenter.Tests.UnitTests
{
    [TestClass]
    public class WireTransferValidatorTest
    {
        [TestMethod]
        public void ValidateReturnsErrorWhenInsufficientFunds()
        {
            Account origin = new Account() { Funds = 0 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTranser = 5m;

            var services = new WiretransferValidator();

            var result = services.Validate(origin, destination, amountToTranser);

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("The origin account does not have enough funds available", result.ErrorMessage);
        }

        [TestMethod]
        public void ValidateReturnsSuccessfulOperation()
        {
            Account origin = new Account() { Funds = 7 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTranser = 5m;

            var services = new WiretransferValidator();

            var result = services.Validate(origin, destination, amountToTranser);

            Assert.IsTrue(result.IsSuccessful);
        }

    }
}
