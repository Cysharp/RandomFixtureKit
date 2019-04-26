using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace RandomFixtureKit.Tests
{
    public class EnumTest
    {
        [Fact]
        public void NonFlag()
        {
            var xs = FixtureFactory.CreateMany<Fruit>(900);

            var foo = xs.ToLookup(x => x);
            foo.Count.Should().Be(3);
            foo[Fruit.Orange].Count().Should().BeCloseTo(300, 30);
            foo[Fruit.Apple].Count().Should().BeCloseTo(300, 30);
            foo[Fruit.Grape].Count().Should().BeCloseTo(300, 30);
        }

        [Fact]
        public void Flag()
        {
            var xs = FixtureFactory.CreateMany<Nanika>(800);

            // None, Tako, Tako|Ika, Tako|Tamago, Tako|Ika|Tamago, Ika, Ika|Tamago, Tamago

            var foo = xs.ToLookup(x => x);
            foo.Count.Should().Be(8);
            foo[Nanika.None].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Tako].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Tako | Nanika.Ika].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Tako | Nanika.Tamago].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Tako | Nanika.Ika | Nanika.Tamago].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Ika].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Ika | Nanika.Tamago].Count().Should().BeCloseTo(100, 35);
            foo[Nanika.Tamago].Count().Should().BeCloseTo(100, 35);
        }

        enum Fruit
        {
            Orange, Apple, Grape
        }

        [Flags]
        enum Nanika
        {
            None = 0,
            Tako = 1,
            Ika = 2,
            Tamago = 4
        }
    }
}
