using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace RandomFixtureKit.Tests
{
    public class EdgeCaseTest
    {
        [Fact]
        public void Primitives()
        {
            {
                var value = FixtureFactory.CreateMany<int>(length: 1000, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(5);
                value[int.MinValue].Count().Should().BeCloseTo(200, 50);
                value[int.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<sbyte>(length: 1000, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(5);
                value[sbyte.MinValue].Count().Should().BeCloseTo(200, 50);
                value[sbyte.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<short>(length: 1000, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(5);
                value[short.MinValue].Count().Should().BeCloseTo(200, 50);
                value[short.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<long>(length: 1000, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(5);
                value[long.MinValue].Count().Should().BeCloseTo(200, 50);
                value[long.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }

            {
                var value = FixtureFactory.CreateMany<byte>(length: 600, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(3);
                value[byte.MinValue].Count().Should().BeCloseTo(200, 50);
                value[byte.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }

            {
                var value = FixtureFactory.CreateMany<ushort>(length: 600, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(3);
                value[ushort.MinValue].Count().Should().BeCloseTo(200, 50);
                value[ushort.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }

            {
                var value = FixtureFactory.CreateMany<uint>(length: 600, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(3);
                value[uint.MinValue].Count().Should().BeCloseTo(200, 50);
                value[uint.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }

            {
                var value = FixtureFactory.CreateMany<ulong>(length: 600, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(3);
                value[ulong.MinValue].Count().Should().BeCloseTo(200, 50);
                value[ulong.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
            }

            {
                var value = FixtureFactory.CreateMany<float>(length: 1800, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(9);
                value[float.MinValue].Count().Should().BeCloseTo(200, 50);
                value[float.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
                value[float.Epsilon].Count().Should().BeCloseTo(200, 50);
                value[float.NaN].Count().Should().BeCloseTo(200, 50);
                value[float.NegativeInfinity].Count().Should().BeCloseTo(200, 50);
                value[float.PositiveInfinity].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<double>(length: 1800, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(9);
                value[double.MinValue].Count().Should().BeCloseTo(200, 50);
                value[double.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[-1].Count().Should().BeCloseTo(200, 50);
                value[0].Count().Should().BeCloseTo(200, 50);
                value[1].Count().Should().BeCloseTo(200, 50);
                value[double.Epsilon].Count().Should().BeCloseTo(200, 50);
                value[double.NaN].Count().Should().BeCloseTo(200, 50);
                value[double.NegativeInfinity].Count().Should().BeCloseTo(200, 50);
                value[double.PositiveInfinity].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<char>(length: 1600, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(8);
                value[char.MinValue].Count().Should().BeCloseTo(200, 50);
                value[char.MaxValue].Count().Should().BeCloseTo(200, 50);
                value['0'].Count().Should().BeCloseTo(200, 50);
                value[' '].Count().Should().BeCloseTo(200, 50);
                value['\r'].Count().Should().BeCloseTo(200, 50);
                value['\n'].Count().Should().BeCloseTo(200, 50);
                value['a'].Count().Should().BeCloseTo(200, 50);
                value['Z'].Count().Should().BeCloseTo(200, 50);
            }
            {
                var value = FixtureFactory.CreateMany<decimal>(length: 1000, resolver: StandardResolver.EdgeCase).ToLookup(x => x);
                value.Count.Should().Be(5);
                value[decimal.MinValue].Count().Should().BeCloseTo(200, 50);
                value[decimal.MaxValue].Count().Should().BeCloseTo(200, 50);
                value[decimal.MinusOne].Count().Should().BeCloseTo(200, 50);
                value[decimal.Zero].Count().Should().BeCloseTo(200, 50);
                value[decimal.One].Count().Should().BeCloseTo(200, 50);
            }
        }


        [Fact]
        public void Collections()
        {
            var value = FixtureFactory.CreateMany<int[]>(length: 800, resolver: StandardResolver.EdgeCase).ToLookup(x => x?.Length);
            value.Count.Should().Be(4);
            value[null].Count().Should().BeCloseTo(200, 50);
            value[0].Count().Should().BeCloseTo(200, 50);
            value[1].Count().Should().BeCloseTo(200, 50);
            value[9].Count().Should().BeCloseTo(200, 50);
        }
    }
}