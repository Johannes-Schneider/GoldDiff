using System.Globalization;
using GoldDiff.View.Converter;
using NUnit.Framework;

namespace Test.GoldDiff.View.Converter
{
    [TestFixture]
    public class IntToLoLGoldValueConverterTest
    {
        [Test]
        public void TestConvertToPositiveValue()
        {
            var converter = new IntToLoLGoldValueConverter();
            var culture = new CultureInfo("en_US");
            
            Assert.AreEqual("337", converter.Convert(337, typeof(string), null!, culture));
            Assert.AreEqual("1.3K", converter.Convert(1337, typeof(string), null!, culture));

            converter.PostDecimalPlaces = 2;
            Assert.AreEqual("1.34K", converter.Convert(1337, typeof(string), null!, culture));
        }

        [Test]
        public void TestConvertToNegativeValue()
        {
            var converter = new IntToLoLGoldValueConverter();
            var culture = new CultureInfo("en_US");
            
            Assert.AreEqual("-337", converter.Convert(-337, typeof(string), null!, culture));
            Assert.AreEqual("-1.3K", converter.Convert(-1337, typeof(string), null!, culture));

            converter.PostDecimalPlaces = 2;
            Assert.AreEqual("-1.34K", converter.Convert(-1337, typeof(string), null!, culture));
        }
    }
}