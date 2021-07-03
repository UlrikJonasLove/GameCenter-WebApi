using GameCenter.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GameCenterTests.UnitTests
{
    [TestClass]
    public class TransferServiceTest
    {
        [TestMethod]
        public void WireTransferWithInsufficentFundsThrowsAnError()
        {
            //Preperation
            Account origin = new Account() { Funds = 0 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;
            string errorMessage = "Custom error message";
            var mockValidateWireTransfer = new Mock<IValidateWireTransfer>();
            mockValidateWireTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(false, errorMessage));
            var service = new TransferService(mockValidateWireTransfer.Object);
            Exception exceptionExpected = null;

            //Testing
            try
            {
                service.WireTransfer(origin, destination, amountToTransfer);
                Assert.Fail();
            }
            catch(Exception ex)
            {
                exceptionExpected = ex;
            }

            //Verification
            if(exceptionExpected == null)
            {
                Assert.Fail("An exception was expected");
            }

            Assert.IsTrue(exceptionExpected is ApplicationException);
            Assert.AreEqual(errorMessage, exceptionExpected.Message);
        }

        [TestMethod]
        public void WireTransferCorrectlyEditFunds()
        {
            //Preperation
            Account origin = new Account() { Funds = 10 };
            Account destination = new Account() { Funds = 5 };
            decimal amountToTransfer = 7;
            var mockValidateWireTransfer = new Mock<IValidateWireTransfer>();
            mockValidateWireTransfer.Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(true));
            var service = new TransferService(mockValidateWireTransfer.Object);

            //Testing
            service.WireTransfer(origin, destination, amountToTransfer);

            //Verification
            Assert.AreEqual(3, origin.Funds);
            Assert.AreEqual(12, destination.Funds);
        }
    }
}
