using System;
using ExchangeCurrency.Model.ExchangeCurrency;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ExchangeCurrency.Model.Tests
{
    public class ExchangeHelperTests
    {
        private ExchangeHelper _exchangeHelper;
        [SetUp]
        public void Setup()
        {
            _exchangeHelper = new ExchangeHelper();
        }

        [Test]
        public void CheckCodesWhenFormatDataIsCorrect()
        {
            // Arrange
            var expected = "THB,USD,AUD";
            var templateDataCurrency = @"[{""currency"": ""bat (Tajlandia)"",""code"": ""THB"",""mid"": 0.1267},
                                        {""currency"": ""dolar amerykañski"",""code"": ""USD"",""mid"": 3.8408},
                                        {""currency"": ""dolar australijski"",""code"": ""AUD"",""mid"": 2.6355}]";
            // Act
            var actual = _exchangeHelper.GetCodes(JToken.Parse(templateDataCurrency));
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckCodesWhenFormatDataIsEmpty()
        {
            // Arrange
            var expected = "";
            var templateDataCurrency = @"[]";
            // Act
            var actual = _exchangeHelper.GetCodes(JToken.Parse(templateDataCurrency));
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckCodesWhenFormatDataIsNotComplete()
        {
            // Arrange
            var expected = "THB,AUD";
            var templateDataCurrency = @"[{""code"": ""THB"",""mid"": 0.1267},
                                        { ""currency"": ""dolar amerykañski"",""corde"": ""USD""},
                                        { ""code"": ""AUD""}]";
            // Act
            var actual = _exchangeHelper.GetCodes(JToken.Parse(templateDataCurrency));
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ThrowArgumentNullExceptionWhenFormatDataIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                _exchangeHelper.GetCodes(JToken.Parse(null)));
        }
    }
}