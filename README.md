RandomFixtureKit
===
[![CircleCI](https://circleci.com/gh/Cysharp/RandomFixtureKit.svg?style=svg)](https://circleci.com/gh/Cysharp/RandomFixtureKit)

Fill random/edge-case value to target type for unit testing, supports both .NET Standard and Unity.

![image](https://user-images.githubusercontent.com/46207/56805033-abce0480-6862-11e9-91d0-7ca9c08aa688.png)

Documantation is not yet but core library is completed. .unitypackage and upm coming soon.

NuGet: [RandomFixtureKit](https://www.nuget.org/packages/RandomFixtureKit)

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

![image](https://user-images.githubusercontent.com/46207/56805214-44fd1b00-6863-11e9-9541-b8ff30b7599a.png)

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
