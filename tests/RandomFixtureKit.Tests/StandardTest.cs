using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RandomFixtureKit.Tests
{
    public class StandardTest
    {
        [Fact]
        public void BoolCheck()
        {
            var xs = Enumerable.Range(1, 1000).Select(_ => FixtureFactory.Create<bool>()).ToArray();
            var foo = xs.ToLookup(x => x);
            foo.Count.Should().Be(2);
            foo[true].Count().Should().BeCloseTo(500, 50);
            foo[false].Count().Should().BeCloseTo(500, 50);
        }

        [Fact]
        public void TestPrimitive()
        {
            var a = FixtureFactory.Create<Primitives>();
            var b = FixtureFactory.Create<Primitives>();

            a.ByteValue.Should().NotBe(b.ByteValue); // byte, 1/255 fail?
            a.SByteValue.Should().NotBe(b.SByteValue);
            a.ShortValue.Should().NotBe(b.ShortValue);
            a.IntValue.Should().NotBe(b.IntValue);
            a.LongValue.Should().NotBe(b.LongValue);
            a.UShortValue.Should().NotBe(b.UShortValue);
            a.UIntValue.Should().NotBe(b.UIntValue);
            a.ULongValue.Should().NotBe(b.ULongValue);
            a.FloatValue.Should().NotBe(b.FloatValue);
            a.DoubleValue.Should().NotBe(b.DoubleValue);
            a.CharValue.Should().NotBe(b.CharValue);
            // boolean not check
            a.DecimalValue.Should().NotBe(b.DecimalValue);
            a.StringValue.Should().NotBe(b.StringValue);
        }

        [Fact]
        public void StructTest()
        {
            var a = FixtureFactory.Create<Vector3>();
            var b = FixtureFactory.Create<Vector3>();

            a.x.Should().NotBe(b.x);
            a.y.Should().NotBe(b.y);
            a.z.Should().NotBe(b.z);
        }

        [Fact]
        public void TupleTest()
        {
            var a = FixtureFactory.Create<Tuple<int, string, short>>();
            var b = FixtureFactory.Create<Tuple<int, string, short>>();

            a.Item1.Should().NotBe(b.Item1);
            a.Item2.Should().NotBe(b.Item2);
            a.Item3.Should().NotBe(b.Item3);
        }

        [Fact]
        public void ValueTupleTest()
        {
            var a = FixtureFactory.Create<(int, string, short)>();
            var b = FixtureFactory.Create<(int, string, short)>();

            a.Item1.Should().NotBe(b.Item1);
            a.Item2.Should().NotBe(b.Item2);
            a.Item3.Should().NotBe(b.Item3);

            var (x, y, z) = FixtureFactory.Create<(int, string, short)>();
            var (x2, y2, z2) = FixtureFactory.Create<(int, string, short)>();
            x.Should().NotBe(x2);
            y.Should().NotBe(y2);
            z.Should().NotBe(z2);
        }

        [Fact]
        public void PrivateFieldTest()
        {
            var a = FixtureFactory.Create<PrivateFields>();

            a.GetPrivateSet1().Should().NotBe(0);
            a.GetField1().Should().NotBe(0);
            a.GetReadonlyField1().Should().NotBe(0);
        }

        [Fact]
        public void Foo()
        {
            {
                var a = FixtureFactory.Create<RecursiveType>(0);
                a.X.Should().NotBe(0);
                a.Next.Should().BeNull();
            }
            {
                var a = FixtureFactory.Create<RecursiveType>(1);
                a.X.Should().NotBe(0);
                a.Next.X.Should().NotBe(0);
                a.Next.Next.Should().BeNull();
            }
            {
                var a = FixtureFactory.Create<RecursiveType>(2);
                a.X.Should().NotBe(0);
                a.Next.X.Should().NotBe(0);
                a.Next.Next.X.Should().NotBe(0);
                a.Next.Next.Next.Should().BeNull();
            }
        }

        class Primitives
        {
            public byte ByteValue { get; }
            public sbyte SByteValue { get; }
            public short ShortValue { get; }
            public int IntValue { get; }
            public long LongValue { get; }
            public ushort UShortValue { get; }
            public uint UIntValue { get; }
            public ulong ULongValue { get; }
            public float FloatValue { get; }
            public double DoubleValue { get; }
            public char CharValue { get; }
            public bool BoolValue { get; }
            public decimal DecimalValue { get; }
            public string StringValue { get; }
        }

        struct Vector3
        {
            public float x;
            public float y;
            public float z;
        }

        class PrivateFields
        {
            public int PrivateSet1 { get; private set; }
            private int PrivateField1;
            private readonly int ReadonlyField1;

            public PrivateFields(int privateField1, int readonlyField1)
            {
                PrivateField1 = privateField1;
                ReadonlyField1 = readonlyField1;
            }

            public int GetPrivateSet1() => PrivateSet1;
            public int GetField1() => PrivateField1;
            public int GetReadonlyField1() => ReadonlyField1;
        }

        class RecursiveType
        {
            public int X;
            public RecursiveType Next;
        }
    }
}
