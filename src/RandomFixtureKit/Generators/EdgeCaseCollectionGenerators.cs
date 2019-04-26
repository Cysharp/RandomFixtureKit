using System;

namespace RandomFixtureKit.Generators
{
    public class EdgeCaseCollectionGenerator : IGenerator
    {
        readonly Type type;
        readonly Func<Type, int, IGenerator> ctor;

        public EdgeCaseCollectionGenerator(Type type, Func<Type, int, IGenerator> ctor)
        {
            this.type = type;
            this.ctor = ctor;
        }

        public Type Type => type;

        public object Generate(GenerationContext context)
        {
            var x = RandomProvider.GetRandom().Next(0, 4);
            if (x == 0)
            {
                // null
                return null;
            }

            var len = (x == 1) ? 0 // or zero-length
                    : (x == 2) ? 1 // or one-length
                    : 9;

            return ctor(type, len).Generate(context);
        }
    }
}
