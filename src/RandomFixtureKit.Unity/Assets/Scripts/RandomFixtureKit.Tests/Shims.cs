using NUnit.Framework;

namespace Xunit
{
    public class FactAttribute : TestAttribute
    {

    }
}

namespace FluentAssertions
{
    public static class FluentAssertionsExtensions
    {
        public static Int Should(this int value)
        {
            return new Int(value);
        }

        public static Generic<T> Should<T>(this T value)
        {
            return new Generic<T>(value);
        }

        public class Generic<T>
        {
            readonly T actual;

            public Generic(T value)
            {
                actual = value;
            }

            public void Be(T expected)
            {
                Assert.AreEqual(expected, actual);
            }

            public void NotBe(T expected)
            {
                Assert.AreNotEqual(expected, actual);
            }

            public void BeNull()
            {
                Assert.IsNull(actual);
            }

            public void NotBeNull()
            {
                Assert.IsNotNull(actual);
            }
        }

        public class Int
        {
            readonly int actual;

            public Int(int value)
            {
                actual = value;
            }

            public void Be(int expected)
            {
                Assert.AreEqual(expected, actual);
            }

            public void NotBe(int expected)
            {
                Assert.AreNotEqual(expected, actual);
            }

            public void BeCloseTo(int expected, int delta)
            {
                if (expected - delta <= actual && actual <= expected + delta)
                {
                    // OK.
                }
                else
                {
                    Assert.Fail($"Fail BeCloseTo, actual {actual} but expected:{expected} +- {delta}");
                }
            }
        }
    }
}