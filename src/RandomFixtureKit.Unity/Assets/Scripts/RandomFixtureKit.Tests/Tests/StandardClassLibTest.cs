using FluentAssertions;
using RandomFixtureKit.Generators;
using RandomFixtureKit.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RandomFixtureKit.Tests
{
    public class StandardClassLibTest
    {
        [Fact]
        public void Generics()
        {

            {
                var x = FixtureFactory.Create<KeyValuePair<string, int>>();
                var y = FixtureFactory.Create<KeyValuePair<string, int>>();
                x.Key.Should().NotBe(y.Key);
                x.Value.Should().NotBe(y.Value);
            }
            {
                // type hint for IL2CPP
                _ = new LazyGenerator.ValueGenerator<int>(0);
                _ = new Lazy<int>(() => 0);

                var x = FixtureFactory.Create<Lazy<int>>();
                var y = FixtureFactory.Create<Lazy<int>>();
                x.Value.Should().NotBe(y.Value);
            }
            {
                var x = FixtureFactory.Create<Task<int>>();
                var y = FixtureFactory.Create<Task<int>>();
                x.Result.Should().NotBe(y.Result);
            }


            var resolver = new CompositeResolver(new[] {
                    new JapaneseHiraganaGenerator(stringLength:9),
                }, new[] {
                    StandardResolver.NonNull
                });
        }

        [Fact]
        public void Concrete()
        {
            var types = new IGenerator[]
            {
                new DateTimeGenerator(),
                new DateTimeOffsetGenerator(),
                new GuidGenerator(),
                new UriGenerator(),
                new VersionGenerator(),
                new StringBuilderGenerator(),
                new BitArrayGenerator(),
                new TypeGenerator(),
                new BigIntegerGenerator(),
                new ComplexGenerator(),
              //  new TaskUnitGenerator(),
            }.Select(x => x.Type).ToArray();

            foreach (var item in types)
            {
                var x = FixtureFactory.Create(item);
                var y = FixtureFactory.Create(item);
                x.Should().NotBe(y);
            }

            {
                var x = FixtureFactory.Create<Task>();
                x.Should().NotBeNull(); // ok to create
            }
        }
    }
}
