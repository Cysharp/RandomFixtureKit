RandomFixturekit
===
[![CircleCI](https://circleci.com/gh/Cysharp/RandomFixturekit.svg?style=svg)](https://circleci.com/gh/Cysharp/RandomFixturekit)

Fill random/edge-case value to target type for unit testing, supports both .NET Standard and Unity.

Documantation is not yet but core library is completed. .unitypackage and upm coming soon.

NuGet: [Ulid](https://www.nuget.org/packages/RandomFixturekit)

```
Install-Package RandomFixturekit
```

How to use
---

```csharp
// get single value
var value = FixtureFactory.Create<Foo>();

// get array
var values = FixtureFactory.CreateMany<Bar>();

// get temporal value
var (x, y, z) = FixtureFactory.Create<(int, string, short)>();
```

In the default, all fill values are random(and not includes null). This behaviour is configure by `IGeneratorResolver resolver` overload.

```csharp
// default is NotNull resolver, the value is completely random
var value1 = FixtureFactory.Create<int>(resolver: StandardResolver.NotNull);

// you can change the EdgeCaseResolver, 
var value2 = FixtureFactory.Create<int>(resolver: StandardResolver.EdgeCase);

// you can change the default resolver.
FixtureFactory.Default = StandardResolver.EdgeCase;

// or make the new you combine resolver.
// this configuration uses string => fill Japanese Hiragana and others as NonNullResolver
var resolver = new CompositeResolver(new[] {
        new JapaneseHiraganaGenerator(stringLength:9),
    }, new[] {
        StandardResolver.NonNull
    });
```

edge-case, for example int is filled `int.MinValue`, `int.MaxValue`, `0`, `-1` or `1`. collection(array, list, etc...) is filled `null`, `zero-elements`, `one-elements`, `nine-elements`.

TODO: More sample of combine resolvers and create custom generator.

Author Info
---
This library is mainly developed by Yoshifumi Kawai(a.k.a. neuecc).  
He is the CEO/CTO of Cysharp which is a subsidiary of [Cygames](https://www.cygames.co.jp/en/).  
He is awarding Microsoft MVP for Developer Technologies(C#) since 2011.  
He is known as the creator of [UniRx](https://github.com/neuecc/UniRx/) and [MessagePack for C#](https://github.com/neuecc/MessagePack-CSharp/).

License
---
This library is under the MIT License.
