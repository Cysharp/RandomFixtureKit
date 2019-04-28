using RandomFixtureKit;
using RandomFixtureKit.Generators;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var resolver = new CompositeResolver(new[] { new MyClassSequentialIdGenerator() }, new[] { StandardResolver.NonNull });
            var fixture = new Fixture(resolver);

            var foo = fixture.CreateMany<MyClass>(100);
            foreach (var item in foo)
            {
                Console.WriteLine(item);
            }
        }
    }

    public class MyClassSequentialIdGenerator : IGenerator
    {
        int sequence = 0;
        IGenerator fallbackGenerator = new Int32Generator();

        public Type Type => typeof(int);

        public object Generate(in GenerationContext context)
        {
            if (context.FieldInfo != null)
            {
                // auto-implemented property's field: <Id>k__BackingField
                if (context.FieldInfo.DeclaringType == typeof(MyClass) && context.FieldInfo.Name.StartsWith("<Id>"))
                {
                    return (sequence++);
                }
            }

            return fallbackGenerator.Generate(context);
        }
    }


    public class MyClass
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return (Id, Age, Name).ToString();
        }
    }
}
