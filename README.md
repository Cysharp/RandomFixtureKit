RandomFixtureKit
===
[![CircleCI](https://circleci.com/gh/Cysharp/RandomFixtureKit.svg?style=svg)](https://circleci.com/gh/Cysharp/RandomFixtureKit)

Fill random/edge-case value to target type for unit testing, supports both .NET Standard and Unity. This lib is similar as [AutoFixture](https://github.com/AutoFixture/AutoFixture) but has some different things.

![image](https://user-images.githubusercontent.com/46207/56805033-abce0480-6862-11e9-91d0-7ca9c08aa688.png)

NuGet: [RandomFixtureKit](https://www.nuget.org/packages/RandomFixtureKit)

```
Install-Package RandomFixturekit
```

Unity: [releases/RandomFixtureKit.unitypackage](https://github.com/Cysharp/RandomFixtureKit/releases)

or package.json exists on `src/RandomFixtureKit.Unity/Assets/Scripts/RandomFixtureKit` for unity package manager.

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents
<!-- END doctoc generated TOC please keep comment here to allow auto update -->

How to use
---

```csharp
// get single value
var value = FixtureFactory.Create<Foo>();

// get array
var values = FixtureFactory.CreateMany<Bar>();

// get temporal value(you can use this values to use invoke target method)
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

// for fill interface type, need to map before call create
FixtureFactory.RegisterMap<IFoo, Foo>();
var v = FixtureFactory.Create<IFoo>(); // return Foo object
```

![image](https://user-images.githubusercontent.com/46207/56805214-44fd1b00-6863-11e9-9541-b8ff30b7599a.png)

edge-case, for example int is filled `int.MinValue`, `int.MaxValue`, `0`, `-1` or `1`. collection(array, list, etc...) is filled `null`, `zero-elements`, `one-elements`, `nine-elements`.

Custom Generator
---
Implement `IGenerator` and setup composite-resolver, you can control how generate values.


```csharp
// for example, increment Id on create MyClass value.
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

// Create IGenerator.
public class MyClassSequentialIdGenerator : IGenerator
{
    int sequence = 0;
    IGenerator fallbackGenerator = new Int32Generator();

    // this generator called if requires int value.
    public Type Type => typeof(int);

    public object Generate(in GenerationContext context)
    {
        // check target int field belongs MyClass.
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

// setup generator to use
var resolver = new CompositeResolver(new[] { new MyClassSequentialIdGenerator() }, new[] { StandardResolver.NonNull });
var fixture = new Fixture(resolver); // fixture is instance version of FixtureFactory

var foo = fixture.CreateMany<MyClass>(100);
foreach (var item in foo)
{
    Console.WriteLine(item);
}
```

Author Info
---
This library is mainly developed by Yoshifumi Kawai(a.k.a. neuecc).  
He is the CEO/CTO of Cysharp which is a subsidiary of [Cygames](https://www.cygames.co.jp/en/).  
He is awarding Microsoft MVP for Developer Technologies(C#) since 2011.  
He is known as the creator of [UniRx](https://github.com/neuecc/UniRx/) and [MessagePack for C#](https://github.com/neuecc/MessagePack-CSharp/).

License
---
This library is under the MIT License.