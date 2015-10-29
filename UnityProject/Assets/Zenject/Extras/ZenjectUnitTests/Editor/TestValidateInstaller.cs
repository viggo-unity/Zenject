using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using Zenject;
using System.Linq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestValidateInstaller
    {
        [Test]
        public void TestBasicSuccess()
        {
            var container = new DiContainer();

            container.Bind<IFoo>().ToSingle<Foo>();
            container.Bind<Bar>().ToSingle();

            Assert.That(container.ValidateResolve<IFoo>().IsEmpty());
        }

        [Test]
        public void TestBasicFailure()
        {
            var container = new DiContainer();

            container.Bind<IFoo>().ToSingle<Foo>();
            //container.Bind<Bar>().ToSingle();

            Assert.That(!container.ValidateResolve<IFoo>().IsEmpty());
        }

        [Test]
        public void TestList()
        {
            var container = new DiContainer();

            container.Bind<IFoo>().ToSingle<Foo>();
            container.Bind<IFoo>().ToSingle<Foo2>();

            container.Bind<Bar>().ToSingle();

            container.Bind<Qux>().ToSingle();

            Assert.That(container.ValidateResolve<Qux>().IsEmpty());
        }

        [Test]
        public void TestValidateDynamicSuccess()
        {
            var container = new DiContainer();

            container.Bind<Foo>().ToSingle();

            Assert.That(container.ValidateObjectGraph<Foo>(typeof(Bar)).IsEmpty());
        }

        [Test]
        public void TestValidateDynamicFailure()
        {
            var container = new DiContainer();

            container.Bind<Foo>().ToSingle();

            Assert.That(!container.ValidateObjectGraph<Foo>().IsEmpty());
        }

        [Test]
        public void TestValidateDynamicFailure2()
        {
            var container = new DiContainer();

            container.Bind<Foo>().ToSingle();

            Assert.That(!container.ValidateObjectGraph<Foo>(typeof(Bar), typeof(string)).IsEmpty());
        }

        [Test]
        public void TestValidateNestedContainerSuccess()
        {
            var container = new DiContainer();

            var nestedContainer = new DiContainer();
            nestedContainer.FallbackProvider = new DiContainerProvider(container);

            // Should fail without Bar<> bound
            Assert.That(!nestedContainer.ValidateObjectGraph<Foo>().IsEmpty());

            container.Bind<Bar>().ToSingle();

            Assert.That(nestedContainer.ValidateObjectGraph<Foo>().IsEmpty());
        }

        [Test]
        public void TestValidateNestedContainerList()
        {
            var container = new DiContainer();

            var nestedContainer = new DiContainer();
            nestedContainer.FallbackProvider = new DiContainerProvider(container);

            container.Bind<IFoo>().ToSingle<Foo>();
            container.Bind<IFoo>().ToSingle<Foo2>();

            Assert.That(!container.ValidateResolve<List<IFoo>>().IsEmpty());
            Assert.That(!nestedContainer.ValidateResolve<List<IFoo>>().IsEmpty());

            container.Bind<Bar>().ToSingle();

            Assert.That(container.ValidateResolve<List<IFoo>>().IsEmpty());

            // Should not throw
            nestedContainer.Resolve<List<IFoo>>();

            Assert.That(nestedContainer.ValidateResolve<List<IFoo>>().IsEmpty());
        }

        interface IFoo
        {
        }

        class Foo : IFoo
        {
            public Foo(Bar bar)
            {
            }
        }

        class Foo2 : IFoo
        {
            public Foo2(Bar bar)
            {
            }
        }

        class Bar
        {
        }

        class Qux
        {
            public Qux(List<IFoo> foos)
            {
            }
        }
    }
}
