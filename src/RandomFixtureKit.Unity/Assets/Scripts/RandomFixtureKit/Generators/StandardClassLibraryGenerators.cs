using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomFixtureKit.Generators
{
    public sealed class DateTimeGenerator : IGenerator
    {
        public Type Type => typeof(DateTime);

        static readonly DateTimeOffsetGenerator generator = new DateTimeOffsetGenerator();

        public object Generate(in GenerationContext context)
        {
            return ((DateTimeOffset)generator.Generate(context)).DateTime;
        }
    }

    public sealed class DateTimeOffsetGenerator : IGenerator
    {
        public Type Type => typeof(DateTimeOffset);

        const int DaysPerYear = 365;
        const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
        const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162
        const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059
        const long TicksPerMillisecond = 10000;
        const long TicksPerSecond = TicksPerMillisecond * 1000;
        const long TicksPerMinute = TicksPerSecond * 60;
        const long TicksPerHour = TicksPerMinute * 60;
        const long TicksPerDay = TicksPerHour * 24;
        const long MinTicks = 0;
        const long MaxTicks = DaysTo10000 * TicksPerDay - 1;
        const long UnixEpochTicks = TimeSpan.TicksPerDay * DaysTo1970; // 621,355,968,000,000,000
        const long UnixEpochMilliseconds = UnixEpochTicks / TimeSpan.TicksPerMillisecond; // 62,135,596,800,000
        const long MinMilliseconds = MinTicks / TimeSpan.TicksPerMillisecond - UnixEpochMilliseconds;
        const long MaxMilliseconds = MaxTicks / TimeSpan.TicksPerMillisecond - UnixEpochMilliseconds;

        public object Generate(in GenerationContext context)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(RandomProvider.GetRandom().NextLong(MinMilliseconds, MaxMilliseconds));
        }
    }

    public sealed class GuidGenerator : IGenerator
    {
        public Type Type => typeof(Guid);

        public object Generate(in GenerationContext context)
        {
            return Guid.NewGuid();
        }
    }

    public sealed class UriGenerator : IGenerator
    {
        static IGenerator alphabetGenerator = new SmallAlphabetGenerator(6);

        public Type Type => typeof(Uri);

        public object Generate(in GenerationContext context)
        {
            var url = "http://" + (string)alphabetGenerator.Generate(context) + ".com/" + (string)alphabetGenerator.Generate(context);
            return new Uri(url);
        }
    }

    public sealed class VersionGenerator : IGenerator
    {
        public Type Type => typeof(Version);

        public object Generate(in GenerationContext context)
        {
            var major = RandomProvider.GetRandom().Next(0, 999);
            var minor = RandomProvider.GetRandom().Next(0, 999);
            var build = RandomProvider.GetRandom().Next(0, 999);
            var revision = RandomProvider.GetRandom().Next(0, 999);

            return new Version(major, minor, build, revision);
        }
    }

    public sealed class KeyValuePairGenerator : IGenerator
    {
        public Type Type { get; private set; }

        public KeyValuePairGenerator(Type type)
        {
            this.Type = type;
        }

        public object Generate(in GenerationContext context)
        {
            var genArgs = Type.GetGenericArguments();
            var keyGen = context.GetGenerator(genArgs[0]);
            var valueGen = context.GetGenerator(genArgs[1]);

            return Activator.CreateInstance(Type, new[] { keyGen.Generate(context), valueGen.Generate(context) });
        }
    }

    public sealed class StringBuilderGenerator : IGenerator
    {
        public Type Type => typeof(StringBuilder);

        public object Generate(in GenerationContext context)
        {
            var strGen = context.GetGenerator(typeof(string));
            return new StringBuilder((string)strGen.Generate(context));
        }
    }

    public sealed class BitArrayGenerator : IGenerator
    {
        public Type Type => typeof(BitArray);

        public object Generate(in GenerationContext context)
        {
            var generator = new ArrayGenerator(typeof(bool[]), 8 * 9);
            var values = (bool[])generator.Generate(context);
            return new BitArray(values);
        }
    }

    public sealed class TypeGenerator : IGenerator
    {
        public Type Type => typeof(Type);

        Type[] typeCache;

        public object Generate(in GenerationContext context)
        {
            if (typeCache == null)
            {
                typeCache = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x =>
                {
                    try
                    {
                        return x.GetTypes();
                    }

                    catch (ReflectionTypeLoadException ex)
                    {
                        return ex.Types ?? Array.Empty<Type>();
                    }
                })
                .ToArray();
            }

            return typeCache[RandomProvider.GetRandom().Next(0, typeCache.Length)];
        }
    }


    public sealed class BigIntegerGenerator : IGenerator
    {
        static readonly IGenerator byteArrayGenerator = new ArrayGenerator(typeof(byte[]), 16);

        public Type Type => typeof(BigInteger);

        public object Generate(in GenerationContext context)
        {
            return new BigInteger((byte[])byteArrayGenerator.Generate(context));
        }
    }

    public sealed class ComplexGenerator : IGenerator
    {
        static readonly IGenerator doubleGenerator = new DoubleGenerator();

        public Type Type => typeof(Complex);

        public object Generate(in GenerationContext context)
        {
            return new Complex((double)doubleGenerator.Generate(context), (double)doubleGenerator.Generate(context));
        }
    }

    public sealed class LazyGenerator : IGenerator
    {
        public Type Type { get; private set; }

        public LazyGenerator(Type type)
        {
            this.Type = type;
        }

        public object Generate(in GenerationContext context)
        {
            var elem = Type.GetGenericArguments()[0];
            var helper = Activator.CreateInstance(typeof(ValueGenerator<>).MakeGenericType(elem), new[] { (object)context.GetGenerator(elem).Generate(context) });
            return Activator.CreateInstance(Type, new[] { helper.GetType().GetMethod("GetFunc").Invoke(helper, null) });
        }

        class ValueGenerator<T>
        {
            readonly T value;

            public ValueGenerator(T value)
            {
                this.value = value;
            }

            public Func<T> GetFunc()
            {
                return () => value;
            }
        }
    }

    public sealed class TaskUnitGenerator : IGenerator
    {
        public Type Type => typeof(Task);

        public object Generate(in GenerationContext context)
        {
            return Task.CompletedTask;
        }
    }

    public sealed class TaskValueGenerator : IGenerator
    {
        public Type Type { get; private set; }

        public TaskValueGenerator(Type type)
        {
            this.Type = type;
        }

        public object Generate(in GenerationContext context)
        {
            return typeof(Task).GetMethod("FromResult").MakeGenericMethod(Type.GetGenericArguments()[0]).Invoke(null, new[] { context.GetGenerator(Type.GetGenericArguments()[0]).Generate(context) });
        }
    }
}
