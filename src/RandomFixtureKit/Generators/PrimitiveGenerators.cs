using System;

namespace RandomFixtureKit.Generators
{
    public abstract class PrimitiveGeneratorBase<T> : IGenerator
        where T : unmanaged
    {
        public Type Type => typeof(T);

        public abstract object Generate();

        public object Generate(in GenerationContext context) => Generate();

        protected unsafe object Generate(Func<byte[], int, T> converter)
        {
            // Span<byte> buffer = stackalloc byte[sizeof(T)];
            var buffer = new byte[sizeof(T)];
            RandomProvider.GetRandom().NextBytes(buffer);
            return converter(buffer, 0);
        }
    }

    public sealed class ByteGenerator : PrimitiveGeneratorBase<Byte>
    {
        public override object Generate()
        {
            return Generate((x, _) => x[0]);
        }
    }

    public sealed class SByteGenerator : PrimitiveGeneratorBase<SByte>
    {
        public override object Generate()
        {
            return Generate((x, _) => unchecked((sbyte)x[0]));
        }
    }

    public sealed class Int16Generator : PrimitiveGeneratorBase<Int16>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToInt16);
        }
    }

    public sealed class Int32Generator : PrimitiveGeneratorBase<Int32>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToInt32);
        }
    }

    public sealed class Int64Generator : PrimitiveGeneratorBase<Int64>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToInt64);
        }
    }

    public sealed class UInt16Generator : PrimitiveGeneratorBase<UInt16>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToUInt16);
        }
    }

    public sealed class UInt32Generator : PrimitiveGeneratorBase<UInt32>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToUInt32);
        }
    }

    public sealed class UInt64Generator : PrimitiveGeneratorBase<UInt64>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToUInt64);
        }
    }

    public sealed class SingleGenerator : PrimitiveGeneratorBase<Single>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToSingle);
        }
    }

    public sealed class DoubleGenerator : PrimitiveGeneratorBase<Double>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToDouble);
        }
    }

    public sealed class CharGenerator : PrimitiveGeneratorBase<Char>
    {
        public override object Generate()
        {
            return Generate(BitConverter.ToChar);
        }
    }

    public sealed class BooleanGenerator : IGenerator
    {
        public Type Type => typeof(Boolean);

        public object Generate(in GenerationContext context)
        {
            return (bool)(RandomProvider.GetRandom().Next(0, 2) == 0);
        }
    }

    public sealed class DecimalGenerator : IGenerator
    {
        public Type Type => typeof(Decimal);

        public object Generate(in GenerationContext context)
        {
            var rand = RandomProvider.GetRandom();
            var lo = rand.Next();
            var mid = rand.Next();
            var hi = rand.Next();
            var isNegative = rand.NextBoolean();
            var scale = (byte)rand.Next(0, 29);
            return new decimal(lo, mid, hi, isNegative, scale);
        }
    }
}
