using System;
using System.Collections.Generic;
using System.Reflection;

namespace RandomFixtureKit
{
    public interface IGeneratorResolver
    {
        IGenerator GetGenerator(Type type);
    }

    public interface IGenerator
    {
        Type Type { get; }
        object Generate(in GenerationContext context);
    }

    public readonly ref struct GenerationContext
    {
        public readonly int RecursiveCount;
        public readonly TypeStack TypeStack;
        public readonly IGeneratorResolver Resolver;
        public readonly FieldInfo FieldInfo;

        public GenerationContext(int recursiveCount, TypeStack typeStack, IGeneratorResolver resolver, FieldInfo fieldInfo)
        {
            RecursiveCount = recursiveCount;
            TypeStack = typeStack;
            Resolver = resolver;
            FieldInfo = fieldInfo;
        }

        public IGenerator GetGenerator(Type type)
        {
            return Resolver.GetGenerator(type);
        }
    }

    public readonly ref struct TypeStack
    {
        readonly Stack<Type> stack;

        public TypeStack(Stack<Type> stack)
        {
            this.stack = stack;
        }

        public TypeStack.Exit Enter(Type type)
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
                if (type.IsAssignableFrom(item)) c++;
            }
            return c;
        }

        public readonly ref struct Exit // : IDisposable
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
