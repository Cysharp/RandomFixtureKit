using System;
using System.Collections.Generic;

namespace RandomFixtureKit
{
    public interface IGeneratorResolver
    {
        IGenerator GetGenerator(Type type);
    }

    public interface IGenerator
    {
        Type Type { get; }
        object Generate(GenerationContext context);
    }

    public struct GenerationContext
    {
        public readonly int RecursiveCount;
        public readonly TypeStack TypeStack;
        public readonly IGeneratorResolver Resolver;

        public GenerationContext(int recursiveCount, TypeStack typeStack, IGeneratorResolver resolver)
        {
            RecursiveCount = recursiveCount;
            TypeStack = typeStack;
            Resolver = resolver;
        }

        public IGenerator GetGenerator(Type type)
        {
            return Resolver.GetGenerator(type);
        }
    }

    public sealed class TypeStack
    {
        readonly Stack<Type> stack = new Stack<Type>();

        public IDisposable Enter(Type type)
        {
            stack.Push(type);
            return new Exit(this);
        }

        public bool IsRoot => stack.Count == 0;

        public int GetCount(Type type)
        {
            var c = 0;
            foreach (var item in stack)
            {
                if (item == type) c++;
            }
            return c;
        }

        class Exit : IDisposable
        {
            readonly TypeStack parent;

            public Exit(TypeStack parent)
            {
                this.parent = parent;
            }

            public void Dispose()
            {
                parent.stack.Pop();
            }
        }
    }
}
