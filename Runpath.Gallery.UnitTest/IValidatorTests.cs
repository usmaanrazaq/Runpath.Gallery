using NUnit.Framework;
using Runpath.Gallery.Service;

namespace Runpath.Gallery.UnitTest
{
    public class IValidatorTests
    {
        private IValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new Validator();

        }

        [Test]
        public void WhenICallValidator_WithValidInput_IGetReturnedTrue()
        {
            bool valid = _validator.IsValid(1);
            Assert.AreEqual(true, valid);
        }

        [Test]
        public void WhenICallValidator_WithInvalidInput_IGetReturnedFalse()
        {
            bool valid = _validator.IsValid(-1);
            Assert.AreEqual(false, valid);
        }
    }
}