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

        public object Generate(GenerationContext context)
        {
            using (context.TypeStack.Enter(type))
            {
                var obj = FormatterServices.GetUninitializedObject(type);

                foreach (var item in fields)
                {
                    if (context.TypeStack.GetCount(item.FieldType) > context.RecursiveCount)
                    {
                        continue;
                    }

                    var generator = context.GetGenerator(item.FieldType);
                    var value = generator.Generate(context);
                    item.SetValue(obj, value);
                }

                return obj;
            }
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

        public object Generate(GenerationContext context)
        {
            if (RandomProvider.GetRandom().NextBoolean())
            {
                return null;
            }

            return objectGenerator.Generate(context);
        }
    }
}
