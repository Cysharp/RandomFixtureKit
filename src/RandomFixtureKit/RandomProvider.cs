using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RandomFixtureKit
{
    public static class RandomProvider
    {
        [ThreadStatic]
        static Random random;

        // this random is async-unsafe, be careful to use.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Random GetRandom()
        {
            if (random == null)
            {
                random = CreateRandom();
            }
            return random;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Random CreateRandom()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                // Span<byte> buffer = stackalloc byte[sizeof(int)];
                var buffer = new byte[sizeof(int)];
                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);
                return new Random(seed);
            }
        }
    }

    internal static class RandomExtensions
    {
        internal static bool NextBoolean(this Random random)
        {
            return (random.Next(0, 2) == 0);
        }

        // https://stackoverflow.com/questions/6651554/random-number-in-long-range-is-this-the-way
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }
    }
}
