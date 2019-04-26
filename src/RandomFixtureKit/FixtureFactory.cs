using System;

namespace RandomFixtureKit
{
    public static class FixtureFactory
    {
        static IGeneratorResolver defaultResolver = StandardResolver.NonNull;

        public static IGeneratorResolver Default
        {
            get
            {
                return defaultResolver;
            }
            set
            {
                defaultResolver = value;
            }
        }

        public static object Create(Type type, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? Default;
            return resolver.GetGenerator(type).Generate(new GenerationContext(recursiveCount, new TypeStack(), resolver));
        }

        public static T Create<T>(int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            return (T)Create(typeof(T), recursiveCount, resolver);
        }

        public static T[] CreateMany<T>(int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? Default;
            var xs = new T[length];
            var generator = resolver.GetGenerator(typeof(T));
            var stack = new TypeStack();
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = (T)generator.Generate(new GenerationContext(recursiveCount, stack, resolver));
            }
            return xs;
        }

        public static object[] CreateMany(Type type, int length = 9, int recursiveCount = 3, IGeneratorResolver resolver = null)
        {
            resolver = resolver ?? Default;
            var xs = new object[length];
            var generator = resolver.GetGenerator(type);
            var stack = new TypeStack();
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = generator.Generate(new GenerationContext(recursiveCount, stack, resolver));
            }
            return xs;
        }
    }
}