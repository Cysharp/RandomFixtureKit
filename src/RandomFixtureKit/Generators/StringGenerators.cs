using System;

namespace RandomFixtureKit.Generators
{
    public abstract class StringGeneratorBase : IGenerator
    {
        public Type Type => typeof(string);

        protected abstract int Length { get; }
        protected abstract char[] Chars { get; }

        public object Generate(in GenerationContext context)
        {
            var rand = RandomProvider.GetRandom();
            var xs = new char[Length];
            for (int i = 0; i < Length; i++)
            {
                xs[i] = Chars[rand.Next(0, Chars.Length)];
            }
            return new string(xs);
        }
    }

    public class SmallAlphabetGenerator : StringGeneratorBase
    {
        static readonly char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        protected override int Length => length;
        protected override char[] Chars => chars;

        readonly int length;

        public SmallAlphabetGenerator(int stringLength)
        {
            this.length = stringLength;
        }
    }

    public class AsciiCharacterGenerator : StringGeneratorBase
    {
        static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        protected override int Length => length;
        protected override char[] Chars => chars;

        readonly int length;

        public AsciiCharacterGenerator(int stringLength)
        {
            this.length = stringLength;
        }
    }

    public class AsciiCharacterWithSpaceAndQuotationGenerator : StringGeneratorBase
    {
        static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 \"'".ToCharArray();

        protected override int Length => length;
        protected override char[] Chars => chars;

        readonly int length;

        public AsciiCharacterWithSpaceAndQuotationGenerator(int stringLength)
        {
            this.length = stringLength;
        }
    }

    public class JapaneseHiraganaGenerator : StringGeneratorBase
    {
        static readonly char[] chars = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよわをん".ToCharArray();

        protected override int Length => length;
        protected override char[] Chars => chars;

        readonly int length;

        public JapaneseHiraganaGenerator(int stringLength)
        {
            this.length = stringLength;
        }
    }

    public class GuidStringGenerator : IGenerator
    {
        public Type Type => typeof(string);

        public object Generate(in GenerationContext context)
        {
            return Guid.NewGuid().ToString();
        }
    }

}
