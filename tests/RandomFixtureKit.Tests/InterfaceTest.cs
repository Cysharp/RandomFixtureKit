using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RandomFixtureKit.Tests
{
    public class InterfaceTest
    {
        [Fact]
        public void InterfaceCheck()
        {
            FixtureFactory.RegisterMap<IMyInterface, Foo>();

            var foo  = FixtureFactory.Create<Foo>();
        }

        public interface IMyInterface
        {
            int MyProperty1 { get; }
            string MyProperty2 { get; }
        }

        public class Foo : IMyInterface
        {
            public int MyProperty1 { get; set; }

            public string MyProperty2 { get; set; }

        }
    }
}
