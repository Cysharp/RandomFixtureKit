using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace RandomFixtureKit.Generators
{
    public class ObjectGenerator : IGenerator
    {
        readonly Type type;
        readonly FieldInfo[] fields;

        public ObjectGenerator(Type type)
        {
            this.type = type;
            this.fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            var scope = context.TypeStack.Enter(type);
            try
            {
                var obj = FormatterServices.GetUninitializedObject(type);

                foreach (var item in fields)
                {
                    if (context.TypeStack.GetCount(item.FieldType) > context.RecursiveCount)
                    {
                        continue;
                    }

                    var generator = context.GetGenerator(item.FieldType);
                    var value = generator.Generate(new GenerationContext(context.RecursiveCount, context.TypeStack, context.Resolver, item));
                    item.SetValue(obj, value);
                }

                return obj;
            }
            finally
            {
                scope.Dispose();
            }
        }
    }

    public class MappingGenerator : IGenerator
    {
        readonly Type from;
        readonly Type to;

        public Type Type => from;

        public MappingGenerator(Type from, Type to)
        {
            this.from = from;
            this.to = to;
        }


        public object Generate(in GenerationContext context)
        {
            return context.GetGenerator(to).Generate(context);
        }
    }

    public class AllowNullObjectGenerator : IGenerator
    {
        readonly Type type;
        readonly ObjectGenerator objectGenerator;

        public AllowNullObjectGenerator(Type type)
        {
            this.type = type;
            this.objectGenerator = new ObjectGenerator(type);
        }

        public Type Type => type;

        public object Generate(in GenerationContext context)
        {
            // root object should not be null.
            if (!context.TypeStack.IsRoot && RandomProvider.GetRandom().NextBoolean())
            {
                return null;
            }

            return objectGenerator.Generate(context);
        }
    }
}
