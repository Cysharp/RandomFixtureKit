using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using System.Collections.Concurrent;

namespace RandomFixtureKit.Tests
{
    public class CollectionTest
    {
        [Fact]
        public void ConcreteTypes()
        {
            ShouldAllRandom(FixtureFactory.Create<int[]>());
            ShouldAllRandom(FixtureFactory.Create<ArraySegment<int>>());
            ShouldAllRandom(FixtureFactory.Create<List<int>>());
            ShouldAllRandom(FixtureFactory.Create<Dictionary<int, int>>());
            ShouldAllRandom(FixtureFactory.Create<Queue<int>>());
            ShouldAllRandom(FixtureFactory.Create<Stack<int>>());
            ShouldAllRandom(FixtureFactory.Create<LinkedList<int>>());
            ShouldAllRandom(FixtureFactory.Create<ConcurrentQueue<int>>());
            ShouldAllRandom(FixtureFactory.Create<ConcurrentStack<int>>());
            ShouldAllRandom(FixtureFactory.Create<ConcurrentDictionary<int, int>>());
        }

        [Fact]
        public void InterfaceTypes()
        {
            ShouldAllRandom(FixtureFactory.Create<IEnumerable<int>>());
            ShouldAllRandom(FixtureFactory.Create<ICollection<int>>());
            ShouldAllRandom(FixtureFactory.Create<IList<int>>());
            ShouldAllRandom(FixtureFactory.Create<IReadOnlyCollection<int>>());
            ShouldAllRandom(FixtureFactory.Create<IReadOnlyList<int>>());
            ShouldAllRandom(FixtureFactory.Create<IDictionary<int, int>>());
            ShouldAllRandom(FixtureFactory.Create<IReadOnlyDictionary<int, int>>());
            ShouldAllRandom(FixtureFactory.Create<ILookup<int, int>>());
        }

        [Fact]
        public void MultiDimentionalArray()
        {
            ShouldAllRandom<int>(FixtureFactory.Create<int[,]>());
            ShouldAllRandom<int>(FixtureFactory.Create<int[,,]>());
        }

        static void ShouldAllRandom<T>(IEnumerable<T> source)
        {
            var xs = source.ToArray();
            xs.Distinct().Count().Should().Be(xs.Length); // all random
        }

        static void ShouldAllRandom<T>(System.Collections.IEnumerable source)
        {
            var xs = source.Cast<T>().ToArray();
            xs.Distinct().Count().Should().Be(xs.Length); // all random
        }
    }
}
