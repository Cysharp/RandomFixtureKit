using System;
using System.Collections.Generic;

namespace RandomFixtureKit
{
    public sealed class Fixture
    {
        internal IGeneratorResolver defaultResolver; // can access from FixtureFactory

        public Fixture(IGeneratorResolver defaultResolver)
        {
            this.defaultResolver = defaultResolver;
        }

        public object Create(Type type, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? defaultResolver;
            return resolver.GetGenerator(type).Generate(new GenerationContext(recursiveCount, new TypeStack(new Stack<Type>()), resolver, null));
        }

        public T Create<T>(int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return (T)Create(typeof(T), recursiveCount, resolver);
        }

        public T[] CreateMany<T>(int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? defaultResolver;
            var xs = new T[length];
            var generator = resolver.GetGenerator(typeof(T));
            var stack = new TypeStack(new Stack<Type>());
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = (T)generator.Generate(new GenerationContext(recursiveCount, stack, resolver, null));
            }
            return xs;
        }

        public object[] CreateMany(Type type, int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? defaultResolver;
            var xs = new object[length];
            var generator = resolver.GetGenerator(type);
            var stack = new TypeStack(new Stack<Type>());
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = generator.Generate(new GenerationContext(recursiveCount, stack, resolver, null));
            }
            return xs;
        }
    }

    // Static /* default */ Helpers of Fixture
    public static class FixtureFactory
    {
        static readonly Fixture DefaultInstance = new Fixture(StandardResolver.NonNull);

        public static IGeneratorResolver Default
        {
            get
            {
                return DefaultInstance.defaultResolver;
            }
            set
            {
                DefaultInstance.defaultResolver = value;
            }
        }

        /// <summary>
        /// Shorthand of MappingObjectResolver.Default.Register.
        /// </summary>
        public static void RegisterMap<TFrom, TTo>()
            where TTo : TFrom
        {
            MappingObjectResolver.Default.Register(typeof(TFrom), typeof(TTo));
        }

        public static object Create(Type type, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return DefaultInstance.Create(type, recursiveCount, resolver);
        }

        public static T Create<T>(int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return DefaultInstance.Create<T>(recursiveCount, resolver);
        }

        public static T[] CreateMany<T>(int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return DefaultInstance.CreateMany<T>(length, recursiveCount, resolver);
        }

        public static object[] CreateMany(Type type, int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return DefaultInstance.CreateMany(type, length, recursiveCount, resolver);
        }
    }
}