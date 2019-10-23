using System;
using System.Collections.Generic;
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

        [Test]
        public void CheckDataCodesWhenFormatDataIsCorrect()
        {
            // Arrange
            var expected = new Dictionary<string, int>
            {
                {"THB", 1},
                {"USD", 2},
                {"CZK", 3},
            };
            var codes = new[] {"THB", "USD", "CZK"};
            // Act
            var actual = _exchangeHelper.AddCodes(codes);
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckSizeWhenFormatDataIsCorrect()
        {
            // Arrange
            var expected = 3;
            var codes = new[] { "THB", "USD", "CZK" };
            // Act
            var actual = _exchangeHelper.AddCodes(codes).Count;
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckDataCodesIfAreNotNull()
        {
            // Arrange
            var codes = new[] { "THB", "USD", "CZK" };
            // Act
            var actual = _exchangeHelper.AddCodes(codes);
            // Assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void CheckDataCodesWhenFormatDataIsEmpty()
        {
            // Arrange
            var expected = new Dictionary<string, int>
            {
                {"THB", 1},
                {"USD", 2},
                {"CZK", 3},
            };
            var codes = new[] { "THB", "USD", "CZK" };
            // Act
            var actual = _exchangeHelper.AddCodes(codes);
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckDataCodesWhenFormatDataIsNull()
        {
            // Assert
            Assert.Throws<NullReferenceException>(() =>
                _exchangeHelper.AddCodes(null));
        }

        [Test]
        public void CheckExchangeRatesWhenFormatDataIsCorrect()
        {
            // Arrange
            var expected = "THB:0.1267 USD:3.8408 AUD:2.6355";
            var templateDataCurrency = @"[{""currency"": ""bat (Tajlandia)"",""code"": ""THB"",""mid"": 0.1267},
                                        {""currency"": ""dolar amerykañski"",""code"": ""USD"",""mid"": 3.8408},
                                        {""currency"": ""dolar australijski"",""code"": ""AUD"",""mid"": 2.6355}]";
            // Act
            var actual = _exchangeHelper.GetExchangeRates(JToken.Parse(templateDataCurrency));
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void CheckExchangeRatesWhenFormatDataIsEmpty()
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
        public void CheckExchangeRatesWhenFormatDataIsNotComplete()
        {
            // Arrange
            var expected = "THB:0.1267 AUD:2.6355";
            var templateDataCurrency = @"[{""currency"": ""bat (Tajlandia)"",""code"": ""THB"",""mid"": 0.1267},
                                        {""currency"": ""dolar amerykañski"",""code"": ""USD""},
                                        {""code"": ""AUD"",""mid"": 2.6355}]";
            // Act
            var actual = _exchangeHelper.GetExchangeRates(JToken.Parse(templateDataCurrency));
            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ThrowArgumentNullExceptionForExchangeRatesWhenFormatDataIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                _exchangeHelper.GetExchangeRates(JToken.Parse(null)));
        }


    }
}