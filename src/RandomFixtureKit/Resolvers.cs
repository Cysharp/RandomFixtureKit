using RandomFixtureKit.Generators;
using RandomFixtureKit.Resolvers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFixtureKit
{
    public static class StandardResolver
    {
        public static readonly IGeneratorResolver NonNull = new CompositeResolver(new[]
        {
            BuiltinResolver.Instance,
            ObjectResolver.Instance
        });

        public static readonly IGeneratorResolver AllowNull = new CompositeResolver(new[]
        {
            BuiltinResolver.Instance,
            AllowNullObjectResolver.Instance
        });

        public static readonly IGeneratorResolver EdgeCase = new CompositeResolver(new[]
        {
            EdgeCaseBuiltinResolver.Instance,
            StandardResolver.AllowNull,
        });

        public static readonly IGeneratorResolver RandomAll = new RandomSelectResolver(
            AllowNull,
            EdgeCase
        );
    }

    public class CompositeResolver : IGeneratorResolver
    {
        readonly IGenerator[] generators;
        readonly IGeneratorResolver[] resolvers;

        readonly ConcurrentDictionary<Type, IGenerator> cache = new ConcurrentDictionary<Type, IGenerator>();

        public CompositeResolver(IGenerator[] generators)
        {
            this.generators = generators;
            this.resolvers = Array.Empty<IGeneratorResolver>();
        }

        public CompositeResolver(IGeneratorResolver[] resolvers)
        {
            this.generators = Array.Empty<IGenerator>();
            this.resolvers = resolvers;
        }

        public CompositeResolver(IGenerator[] generators, IGeneratorResolver[] resolvers)
        {
            this.generators = generators;
            this.resolvers = resolvers;
        }

        public IGenerator GetGenerator(Type type)
        {
            if (cache.TryGetValue(type, out var generator))
            {
                return generator;
            }

            foreach (var item in generators)
            {
                if (item.Type == type)
                {
                    return (cache[type] = item);
                }
            }

            foreach (var item in resolvers)
            {
                generator = item.GetGenerator(type);
                if (generator != null)
                {
                    return (cache[type] = generator);
                }
            }

            return null;
        }
    }

    public class RandomSelectResolver : IGeneratorResolver
    {
        readonly IGeneratorResolver[] resolvers;

        public RandomSelectResolver(params IGeneratorResolver[] resolvers)
        {
            this.resolvers = resolvers;
        }

        public IGenerator GetGenerator(Type type)
        {
            return resolvers[RandomProvider.GetRandom().Next(0, resolvers.Length)].GetGenerator(type);
        }
    }
}

namespace RandomFixtureKit.Resolvers
{
    public class BuiltinResolver : IGeneratorResolver
    {
        public static readonly IGeneratorResolver Instance = new BuiltinResolver();

        const int CollectionSize = 9;

        readonly Dictionary<Type, IGenerator> generators = new IGenerator[]
        {
            // PrimitiveGenerators.cs
            new ByteGenerator(),
            new SByteGenerator(),
            new Int16Generator(),
            new Int32Generator(),
            new Int64Generator(),
            new UInt16Generator(),
            new UInt32Generator(),
            new UInt64Generator(),
            new SingleGenerator(),
            new DoubleGenerator(),
            new CharGenerator(),
            new BooleanGenerator(),
            new DecimalGenerator(),
            // StringGenerators.cs
            new AsciiCharacterGenerator(9),
            // StandardClassLibraryGenerator.cs
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
            new TaskUnitGenerator(),
        }.ToDictionary(x => x.Type);

        BuiltinResolver()
        {

        }

        public IGenerator GetGenerator(Type type)
        {
            // primitive and standard builtin
            if (generators.TryGetValue(type, out var generator))
            {
                return generator;
            }

            if (type.IsArray)
            {
                return new ArrayGenerator(type, CollectionSize);
            }

            if (type.IsEnum)
            {
                return new EnumGenerator(type);
            }

            {
                var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
                if (nullableUnderlyingType != null)
                {
                    return new NullableGenerator(type, nullableUnderlyingType);
                }
            }

            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return new DictionaryGenerator(type, CollectionSize);
            }
            if (typeof(System.Collections.IList).IsAssignableFrom(type))
            {
                return new ListGenerator(type, CollectionSize);
            }
            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(KeyValuePair<,>))
                {
                    return new KeyValuePairGenerator(type);
                }
                if (genericDef == typeof(Lazy<>))
                {
                    return new LazyGenerator(type);
                }
                if (genericDef == typeof(Task<>))
                {
                    return new TaskValueGenerator(type);
                }

                // collections

                if (genericDef == typeof(ArraySegment<>))
                {
                    return new ArraySegmentGenerator(type, CollectionSize);
                }
                if (genericDef == typeof(Queue<>))
                {
                    return new QueueGenerator(type, CollectionSize);
                }
                if (genericDef == typeof(Stack<>))
                {
                    return new StackGenerator(type, CollectionSize);
                }
                if (genericDef == typeof(LinkedList<>))
                {
                    return new LinkedListGenerator(type, CollectionSize);
                }
                if (genericDef == typeof(ConcurrentQueue<>))
                {
                    return new ConcurrentQueueGenerator(type, CollectionSize);
                }
                if (genericDef == typeof(ConcurrentStack<>))
                {
                    return new ConcurrentStackGenerator(type, CollectionSize);
                }

                // interfaces
                if (genericDef == typeof(IEnumerable<>)
                 || genericDef == typeof(ICollection<>)
                 || genericDef == typeof(IList<>)
                 || genericDef == typeof(IReadOnlyCollection<>)
                 || genericDef == typeof(IReadOnlyList<>))
                {
                    return new ArrayGenerator(type.GetGenericArguments()[0].MakeArrayType(), CollectionSize);
                }
                if (genericDef == typeof(IDictionary<,>)
                 || genericDef == typeof(IReadOnlyDictionary<,>))
                {
                    return new DictionaryGenerator(
                        typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments()), CollectionSize);
                }
                if (genericDef == typeof(ILookup<,>))
                {
                    return new InterfaceLookupGenerator(type, CollectionSize);
                }
            }

            return null;
        }
    }

    public class EdgeCaseBuiltinResolver : IGeneratorResolver
    {
        public static readonly IGeneratorResolver Instance = new EdgeCaseBuiltinResolver();

        readonly Dictionary<Type, IGenerator> generators = new IGenerator[]
        {
            // EdgeCasePrimitiveGenerators.cs
            new EdgeCaseByteGenerator(),
            new EdgeCaseSByteGenerator(),
            new EdgeCaseInt16Generator(),
            new EdgeCaseInt32Generator(),
            new EdgeCaseInt64Generator(),
            new EdgeCaseUInt16Generator(),
            new EdgeCaseUInt32Generator(),
            new EdgeCaseUInt64Generator(),
            new EdgeCaseSingleGenerator(),
            new EdgeCaseDoubleGenerator(),
            new EdgeCaseCharGenerator(),
            // new EdgeCaseBooleanGenerator(),
            new EdgeCaseDecimalGenerator(),
            // EdgeCaseStringGenerators.cs
            new EdgeCaseStringGenerator(),
            // EdgeCaseStandardClassLibraryGenerator.cs
            new EdgeCaseDateTimeGenerator(),
            new EdgeCaseDateTimeOffsetGenerator(),
            new EdgeCaseBigIntegerGenerator(),
            new EdgeCaseComplexGenerator(),
            new AllowNullCodeGenerator(typeof(Version), _ => new VersionGenerator()),
            new AllowNullCodeGenerator(typeof(StringBuilder), _ => new StringBuilderGenerator()),
            new AllowNullCodeGenerator(typeof(BitArray), _ => new BitArrayGenerator()),
            new AllowNullCodeGenerator(typeof(Type), _ => new TypeGenerator()),
            new AllowNullCodeGenerator(typeof(Task), _ => new TaskUnitGenerator()),
        }.ToDictionary(x => x.Type);

        EdgeCaseBuiltinResolver()
        {
        }

        public IGenerator GetGenerator(Type type)
        {
            if (generators.TryGetValue(type, out var generator))
            {
                return generator;
            }

            if (type.IsArray)
            {
                return new EdgeCaseCollectionGenerator(type, (x, i) => new ArrayGenerator(x, i));
            }

            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
            {
                return new EdgeCaseCollectionGenerator(type, (x, i) => new DictionaryGenerator(x, i));
            }
            if (typeof(System.Collections.IList).IsAssignableFrom(type))
            {
                return new EdgeCaseCollectionGenerator(type, (x, i) => new ListGenerator(x, i));
            }
            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(Lazy<>))
                {
                    return new AllowNullCodeGenerator(type, x => new LazyGenerator(x));
                }
                if (genericDef == typeof(Task<>))
                {
                    return new AllowNullCodeGenerator(type, x => new TaskValueGenerator(x));
                }

                // collections

                if (genericDef == typeof(Queue<>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new QueueGenerator(x, i));
                }
                if (genericDef == typeof(Stack<>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new StackGenerator(x, i));
                }
                if (genericDef == typeof(LinkedList<>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new LinkedListGenerator(x, i));
                }
                if (genericDef == typeof(ConcurrentQueue<>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new ConcurrentQueueGenerator(x, i));
                }
                if (genericDef == typeof(ConcurrentStack<>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new ConcurrentStackGenerator(x, i));
                }

                // interfaces
                if (genericDef == typeof(IEnumerable<>)
                 || genericDef == typeof(ICollection<>)
                 || genericDef == typeof(IList<>)
                 || genericDef == typeof(IReadOnlyCollection<>)
                 || genericDef == typeof(IReadOnlyList<>))
                {
                    return new EdgeCaseCollectionGenerator(type.GetGenericArguments()[0].MakeArrayType(), (x, i) => new ArrayGenerator(x, i));
                }
                if (genericDef == typeof(IDictionary<,>)
                 || genericDef == typeof(IReadOnlyDictionary<,>))
                {
                    return new EdgeCaseCollectionGenerator(typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments()), (x, i) => new DictionaryGenerator(x, i));
                }
                if (genericDef == typeof(ILookup<,>))
                {
                    return new EdgeCaseCollectionGenerator(type, (x, i) => new InterfaceLookupGenerator(x, i));
                }
            }

            return null;
        }
    }

    public class ObjectResolver : IGeneratorResolver
    {
        public static readonly IGeneratorResolver Instance = new ObjectResolver();

        ObjectResolver()
        {

        }

        public IGenerator GetGenerator(Type type)
        {
            return new ObjectGenerator(type);
        }
    }

    public class AllowNullObjectResolver : IGeneratorResolver
    {
        public static readonly IGeneratorResolver Instance = new AllowNullObjectResolver();

        AllowNullObjectResolver()
        {

        }

        public IGenerator GetGenerator(Type type)
        {
            return new AllowNullObjectGenerator(type);
        }
    }
}
