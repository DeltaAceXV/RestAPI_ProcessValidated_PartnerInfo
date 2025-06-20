namespace RestAPI_ProcessValidated_PartnerInfo.TestUnits
{
    using Moq;
    using NUnit.Framework;
    using RestAPI_ProcessValidated_PartnerInfo.DTO;
    using RestAPI_ProcessValidated_PartnerInfo.Entities;
    using RestAPI_ProcessValidated_PartnerInfo.Service;

    [TestFixture]
    public class IProcessAmountServiceTests
    {

        private Mock<IProcessAmountService> _processAmountService;

        [SetUp]
        public void Init()
        {
            this._processAmountService = new Mock<IProcessAmountService>();
        }

        [Test]
        public async Task ProcessAmount_WhenValidated_Total1000()
        {
            this._processAmountService
                .Setup(i => i.ProcessAmount(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(SummaryAmountResponse.Success(1000));

            var result = await this._processAmountService.Object.ProcessAmount(new());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(1));
            Assert.That(result.TotalAmount, Is.EqualTo(1000));
            Assert.That(result.TotalDiscount, Is.EqualTo(0));
            Assert.That(result.FinalAmount, Is.EqualTo(result.TotalAmount));
        }

        [Test]
        public async Task ProcessAmount_WhenValidated_Total100000()
        {
            this._processAmountService
                .Setup(i => i.ProcessAmount(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(SummaryAmountResponse.Success(100000));

            var result = await this._processAmountService.Object.ProcessAmount(new());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(1));
            Assert.That(result.TotalAmount, Is.EqualTo(100000));
            Assert.That(result.TotalDiscount, Is.EqualTo(10000));
            Assert.That(result.FinalAmount, Is.EqualTo(90000));
        }

        [Test]
        public async Task ProcessAmount_WhenValidated_Total120500()
        {
            this._processAmountService
                .Setup(i => i.ProcessAmount(It.IsAny<TransactionDetails>()))
                .ReturnsAsync(SummaryAmountResponse.Success(120500));

            var result = await this._processAmountService.Object.ProcessAmount(new());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(1));
            Assert.That(result.TotalAmount, Is.EqualTo(120500));
            Assert.That(result.TotalDiscount, Is.EqualTo(24100));
            Assert.That(result.FinalAmount, Is.EqualTo(96400));
        }

    }
}
