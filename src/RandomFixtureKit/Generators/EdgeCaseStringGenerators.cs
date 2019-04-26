using System;

namespace RandomFixtureKit.Generators
{
    public class EdgeCaseStringGenerator : IGenerator
    {
        public Type Type => typeof(string);

        public object Generate(GenerationContext context)
        {
            var rand = RandomProvider.GetRandom();
            var x = rand.Next(0, 5);
            if (x == 0)
            {
                return null;
            }
            else if (x == 1)
            {
                return string.Empty;
            }
            else if (x == 2)
            {
                return " ";
            }

            var len = (x == 3) ? 1 : 9;

            return new AsciiCharacterWithSpaceAndQuotationGenerator(len).Generate(context);
        }
    }

    public class EdgeCaseStringIncludeJapaneGenerator : IGenerator
    {
        public Type Type => typeof(string);

        public object Generate(GenerationContext context)
        {
            var rand = RandomProvider.GetRandom();
            var x = rand.Next(0, 5);
            if (x == 0)
            {
                return null;
            }
            else if (x == 1)
            {
                return string.Empty;
            }
            else if (x == 2)
            {
                return " ";
            }

            var len = (x == 3) ? 1 : 9;

            return new JapaneseHiraganaGenerator(len).Generate(context);
        }
    }
}
