using System;

namespace RandomFixtureKit.Generators
{
    public abstract class EdgeCaseGeneratorBase<T> : IGenerator
    {
        public Type Type => typeof(T);

        protected abstract T[] Values { get; }

        public object Generate(GenerationContext context)
        {
            return Values[RandomProvider.GetRandom().Next(0, Values.Length)];
        }
    }

    public sealed class EdgeCaseByteGenerator : EdgeCaseGeneratorBase<Byte>
    {
        readonly byte[] values = new byte[] { byte.MinValue, 1, byte.MaxValue };
        protected override byte[] Values => values;
    }

    public sealed class EdgeCaseSByteGenerator : EdgeCaseGeneratorBase<SByte>
    {
        readonly sbyte[] values = new sbyte[] { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue };
        protected override sbyte[] Values => values;
    }

    public sealed class EdgeCaseInt16Generator : EdgeCaseGeneratorBase<Int16>
    {
        readonly short[] values = new short[] { short.MinValue, -1, 0, 1, short.MaxValue };
        protected override short[] Values => values;
    }

    public sealed class EdgeCaseInt32Generator : EdgeCaseGeneratorBase<Int32>
    {
        readonly int[] values = new int[] { int.MinValue, -1, 0, 1, int.MaxValue };
        protected override int[] Values => values;
    }

    public sealed class EdgeCaseInt64Generator : EdgeCaseGeneratorBase<Int64>
    {
        readonly long[] values = new long[] { long.MinValue, -1, 0, 1, long.MaxValue };
        protected override long[] Values => values;
    }

    public sealed class EdgeCaseUInt16Generator : EdgeCaseGeneratorBase<UInt16>
    {
        readonly ushort[] values = new ushort[] { ushort.MinValue, 1, ushort.MaxValue };
        protected override ushort[] Values => values;
    }

    public sealed class EdgeCaseUInt32Generator : EdgeCaseGeneratorBase<UInt32>
    {
        readonly uint[] values = new uint[] { uint.MinValue, 1, uint.MaxValue };
        protected override uint[] Values => values;
    }

    public sealed class EdgeCaseUInt64Generator : EdgeCaseGeneratorBase<UInt64>
    {
        readonly ulong[] values = new ulong[] { ulong.MinValue, 1, ulong.MaxValue };
        protected override ulong[] Values => values;
    }

    public sealed class EdgeCaseSingleGenerator : EdgeCaseGeneratorBase<Single>
    {
        readonly float[] values = new float[] { float.MinValue, -1, 0, 1, float.MaxValue, float.Epsilon, float.NaN, float.NegativeInfinity, float.PositiveInfinity };
        protected override float[] Values => values;
    }

    public sealed class EdgeCaseDoubleGenerator : EdgeCaseGeneratorBase<Double>
    {
        readonly double[] values = new double[] { double.MinValue, -1, 0, 1, double.MaxValue, double.Epsilon, double.NaN, double.NegativeInfinity, double.PositiveInfinity };
        protected override double[] Values => values;
    }

    public sealed class EdgeCaseCharGenerator : EdgeCaseGeneratorBase<Char>
    {
        readonly char[] values = new char[] { char.MinValue, char.MaxValue, '0', ' ', '\r', '\n', 'a', 'Z' };
        protected override char[] Values => values;
    }

    public sealed class EdgeCaseDecimalGenerator : EdgeCaseGeneratorBase<Decimal>
    {
        readonly decimal[] values = new decimal[] { decimal.MinValue, decimal.MinusOne, decimal.Zero, decimal.One, decimal.MaxValue };
        protected override decimal[] Values => values;
    }
}
