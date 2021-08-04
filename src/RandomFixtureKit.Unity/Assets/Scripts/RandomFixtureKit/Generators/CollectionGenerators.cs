using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RandomFixtureKit.Generators
{
    public class ArrayGenerator : IGenerator
    {
        readonly int length;

        public ArrayGenerator(Type type, int length)
        {
            this.Type = type;
            this.length = length;
        }

        public Type Type { get; private set; }

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var elemType = Type.GetElementType();
                var generator = context.GetGenerator(elemType);
                var rank = Type.GetArrayRank();

                var array = Array.CreateInstance(elemType, Enumerable.Range(0, rank).Select(_ => length).ToArray());
                switch (rank)
                {
                    case 1: SetOne(array, generator, context); break;
                    case 2: SetTwo(array, generator, context); break;
                    case 3: SetThree(array, generator, context); break;
                    default:
                        throw new InvalidOperationException($"Array rank:{rank} is not supported.");
                }

                return array;
            }
        }

        void SetOne(Array array, IGenerator generator, GenerationContext context)
        {
            for (int i = 0; i < length; i++)
            {
                array.SetValue(generator.Generate(context), i);
            }
        }

        void SetTwo(Array array, IGenerator generator, GenerationContext context)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    array.SetValue(generator.Generate(context), i, j);
                }
            }
        }

        void SetThree(Array array, IGenerator generator, GenerationContext context)
        {
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        array.SetValue(generator.Generate(context), i, j, k);
                    }
                }
            }
        }
    }

    public class ArraySegmentGenerator : IGenerator
    {
        readonly Type type;
        readonly int length;

        public ArraySegmentGenerator(Type type, int length)
        {
            this.type = type;
            this.length = length;
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var elemType = type.GetGenericArguments()[0];
                var arrayGenerator = context.GetGenerator(elemType.MakeArrayType());

                var innerArray = arrayGenerator.Generate(context);
                return ReflectionHelper.CreateInstance(type, new[] { innerArray });
            }
        }
    }

    // type must implement IList
    public class ListGenerator : IGenerator
    {
        readonly Type type;
        readonly int length;

        public ListGenerator(Type type, int length)
        {
            this.type = type;
            this.length = length;
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var elemType = type.GetGenericArguments()[0];
                var generator = context.GetGenerator(elemType);

                var list = ReflectionHelper.CreateInstance(type) as IList;
                for (int i = 0; i < length; i++)
                {
                    list.Add(generator.Generate(context));
                }

                return list;
            }
        }
    }

    // type must implement IDictionary
    public class DictionaryGenerator : IGenerator
    {
        readonly Type type;
        readonly int length;

        public DictionaryGenerator(Type type, int length)
        {
            this.type = type;
            this.length = length;
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var genArgs = type.GetGenericArguments();
                var keyType = genArgs[0];
                var valueType = genArgs[1];
                var keyGenerator = context.GetGenerator(keyType);
                var valueGenerator = context.GetGenerator(valueType);

                var dict = ReflectionHelper.CreateInstance(type) as IDictionary;
                for (int i = 0; i < length; i++)
                {
                    dict[keyGenerator.Generate(context)] = valueGenerator.Generate(context);
                }

                return dict;
            }
        }
    }

    public abstract class GenericReflectionCollectionGeneratorBase : IGenerator
    {
        public Type Type { get; private set; }
        protected abstract string AddMethodName { get; }

        readonly int length;

        public GenericReflectionCollectionGeneratorBase(Type type, int length)
        {
            this.Type = type;
            this.length = length;
        }

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var elemType = Type.GetGenericArguments()[0];
                var generator = context.GetGenerator(elemType);

                var add = Type.GetMethod(AddMethodName, new[] { elemType });

                var collection = ReflectionHelper.CreateInstance(Type);
                for (int i = 0; i < length; i++)
                {
                    add.Invoke(collection, new[] { generator.Generate(context) });
                }

                return collection;
            }
        }
    }

    public sealed class QueueGenerator : GenericReflectionCollectionGeneratorBase
    {
        public QueueGenerator(Type type, int length)
            : base(type, length)
        {
        }

        protected override string AddMethodName => "Enqueue";
    }

    public sealed class StackGenerator : GenericReflectionCollectionGeneratorBase
    {
        public StackGenerator(Type type, int length)
            : base(type, length)
        {
        }

        protected override string AddMethodName => "Push";
    }

    public sealed class LinkedListGenerator : GenericReflectionCollectionGeneratorBase
    {
        public LinkedListGenerator(Type type, int length)
            : base(type, length)
        {
        }

        protected override string AddMethodName => "AddLast";
    }

    public sealed class ConcurrentQueueGenerator : GenericReflectionCollectionGeneratorBase
    {
        public ConcurrentQueueGenerator(Type type, int length)
            : base(type, length)
        {
        }

        protected override string AddMethodName => "Enqueue";
    }

    public sealed class ConcurrentStackGenerator : GenericReflectionCollectionGeneratorBase
    {
        public ConcurrentStackGenerator(Type type, int length)
            : base(type, length)
        {
        }

        protected override string AddMethodName => "Push";
    }

    public class InterfaceLookupGenerator : IGenerator
    {
        readonly Type type;
        readonly int length;

        public InterfaceLookupGenerator(Type type, int length)
        {
            this.type = type;
            this.length = length;
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            using (var scope = context.TypeStack.Enter(Type))
            {
                var genType = type.GenericTypeArguments;
                var generator = context.GetGenerator(typeof(Dictionary<,>).MakeGenericType(new[] { genType[0], genType[1].MakeArrayType() }));
                var dictionary = generator.Generate(context);

                var lookup = ReflectionHelper.CreateInstance(typeof(PseudoLookup<,>).MakeGenericType(genType), new[] { dictionary });
                return lookup;
            }
        }

        // require to type hint to use in IL2CPP
        public class PseudoLookup<TKey, TValue> : ILookup<TKey, TValue>
        {
            readonly Dictionary<TKey, TValue[]> dict;

            public PseudoLookup(Dictionary<TKey, TValue[]> dict)
            {
                this.dict = dict;
            }

            public IEnumerable<TValue> this[TKey key] => dict[key];

            public int Count => dict.Count;

            public bool Contains(TKey key)
            {
                return dict.ContainsKey(key);
            }

            public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator()
            {
                foreach (var item in dict)
                {
                    yield return new Grouping(item.Key, item.Value);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            class Grouping : IGrouping<TKey, TValue>
            {
                readonly TKey key;
                readonly TValue[] values;

                public Grouping(TKey key, TValue[] values)
                {
                    this.key = key;
                    this.values = values;
                }

                public TKey Key => key;

                public IEnumerator<TValue> GetEnumerator()
                {
                    return values.AsEnumerable().GetEnumerator();
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }
        }
    }
}
