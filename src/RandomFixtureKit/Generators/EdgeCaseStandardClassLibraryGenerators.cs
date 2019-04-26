using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RandomFixtureKit.Generators
{
    public class AllowNullCodeGenerator : IGenerator
    {
        readonly Type type;
        readonly Func<Type, IGenerator> ctor;

        public AllowNullCodeGenerator(Type type, Func<Type, IGenerator> ctor)
        {
            this.type = type;
            this.ctor = ctor;
        }

        public Type Type => type;

        public object Generate(GenerationContext context)
        {
            var x = RandomProvider.GetRandom().Next(0, 4);
            if (x == 0)
            {
                // null
                return null;
            }

            return ctor(type).Generate(context);
        }
    }

    public class EdgeCaseDateTimeGenerator : EdgeCaseGeneratorBase<DateTime>
    {
        readonly DateTime[] values = new DateTime[] { DateTime.MinValue, DateTime.MaxValue, DateTime.UtcNow };
        protected override DateTime[] Values => values;
    }

    public class EdgeCaseDateTimeOffsetGenerator : EdgeCaseGeneratorBase<DateTimeOffset>
    {
        readonly DateTimeOffset[] values = new DateTimeOffset[] { DateTimeOffset.MinValue, DateTimeOffset.MaxValue, DateTimeOffset.UtcNow };
        protected override DateTimeOffset[] Values => values;
    }

    public class EdgeCaseComplexGenerator : EdgeCaseGeneratorBase<Complex>
    {
        readonly Complex[] values = new Complex[] { Complex.ImaginaryOne, Complex.One, Complex.Zero };
        protected override Complex[] Values => values;
    }

    public class EdgeCaseBigIntegerGenerator : EdgeCaseGeneratorBase<BigInteger>
    {
        readonly BigInteger[] values = new BigInteger[] { BigInteger.MinusOne, BigInteger.Zero, BigInteger.One };
        protected override BigInteger[] Values => values;
    }
}
