using GoldDiff.Shared.Utility;
using NUnit.Framework;

namespace Test.GoldDiff.Shared.Utility
{
    [TestFixture]
    public class MultiSetTest
    {
        [Test]
        public void TestDifference()
        {
            var a = new[] {0, 1, 1, 2, 3, 4};
            var b = new[] {1, 3, 4};
            
            CollectionAssert.AreEqual(new[] {0, 1, 2}, MultiSet.Difference(a, b));
        }
    }
}