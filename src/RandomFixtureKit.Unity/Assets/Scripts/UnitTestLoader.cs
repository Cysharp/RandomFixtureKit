using UnityEngine;
using RuntimeUnitTestToolkit;
using RandomFixtureKit.Tests;

namespace SampleUnitTest
{
    public static class UnitTestLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Register()
        {
            UnitTest.RegisterAllMethods<CollectionTest>();
            UnitTest.RegisterAllMethods<EdgeCaseTest>();
            UnitTest.RegisterAllMethods<EnumTest>();
            UnitTest.RegisterAllMethods<InterfaceTest>();
            UnitTest.RegisterAllMethods<StandardClassLibTest>();
            UnitTest.RegisterAllMethods<StandardTest>();
        }
    }
}