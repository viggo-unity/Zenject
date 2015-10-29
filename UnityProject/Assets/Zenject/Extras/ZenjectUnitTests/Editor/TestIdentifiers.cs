using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestIdentifiers : TestWithContainer
    {
        class Test0
        {
        }

        [Test]
        public void TestBasic()
        {
            Container.Bind<Test0>("foo").ToTransient();

            Assert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });

            Container.Resolve<Test0>("foo");
            Assert.That(Container.ValidateResolve<Test0>("foo").IsEmpty());
        }

        [Test]
        public void TestBasic2()
        {
            Container.Bind<Test0>("foo").ToSingle();

            Assert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });

            Container.Resolve<Test0>("foo");
            Assert.That(Container.ValidateResolve<Test0>("foo").IsEmpty());
        }

        [Test]
        public void TestBasic3()
        {
            Container.Bind<Test0>("foo").ToMethod((ctx) => new Test0());

            Assert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });

            Container.Resolve<Test0>("foo");
            Assert.That(Container.ValidateResolve<Test0>("foo").IsEmpty());
        }

        [Test]
        public void TestBasic4()
        {
            Container.Bind<Test0>("foo").ToTransient();
            Container.Bind<Test0>("foo").ToTransient();

            Assert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });

            Assert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>("foo"); });

            Assert.IsEqual(Container.ResolveAll<Test0>("foo").Count, 2);
        }

        [Test]
        public void TestToMethodUntyped()
        {
            Container.Bind(typeof(Test0)).ToMethod((ctx) => new Test0());

            Container.Resolve<Test0>();

            Assert.That(Container.ValidateResolve<Test0>().IsEmpty());
        }
    }
}
