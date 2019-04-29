using System;

namespace RandomFixtureKit.Generators
{
    public class NullableGenerator : IGenerator
    {
        Type nullableType;
        Type underlyingType;

        public Type Type => nullableType;

        public NullableGenerator(Type nullableType, Type underlyingType)
        {
            this.underlyingType = underlyingType;
            this.nullableType = nullableType;
        }

        public object Generate(in GenerationContext context)
        {
            if (RandomProvider.GetRandom().NextBoolean())
            {
                return null;
            }

            return context.GetGenerator(underlyingType).Generate(context);
        }
    }
}
