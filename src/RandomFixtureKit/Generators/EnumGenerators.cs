using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomFixtureKit.Generators
{
    public sealed class EnumGenerator : IGenerator
    {
        public Type Type => enumType;

        readonly Type enumType;
        readonly Array values;

        public EnumGenerator(Type enumType)
        {
            this.enumType = enumType;
            this.values = Enum.GetValues(enumType);

            if (enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0)
            {
                var combination = Combination(0, 0)
                    .Distinct()
                    .Select(x => Enum.ToObject(enumType, x))
                    .ToArray();
                values = combination;
            }
        }

        public object Generate(GenerationContext context)
        {
            return values.GetValue(RandomProvider.GetRandom().Next(0, values.Length));
        }

        IEnumerable<long> Combination(long current, int index)
        {
            for (int i = index; i < values.Length; i++)
            {
                var v = (current | Convert.ToInt64(values.GetValue(i)));
                yield return v;
                foreach (var item in Combination(v, i + 1))
                {
                    yield return item;
                }
            }
        }
    }
}
