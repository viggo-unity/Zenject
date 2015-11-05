using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestBothInterfaceAndConcreteBoundToSameSingleton : TestWithContainer
    {
        abstract class Test0
        {
        }

        class Test1 : Test0
        {
        }

        [Test]
        public void TestCaseBothInterfaceAndConcreteBoundToSameSingleton()
        {
            Container.Bind<Test0>().ToSingle<Test1>();
            Container.Bind<Test1>().ToSingle();

            Assert.That(Container.ValidateResolve<Test0>().IsEmpty());
            var test1 = Container.Resolve<Test0>();

            Assert.That(Container.ValidateResolve<Test1>().IsEmpty());
            var test2 = Container.Resolve<Test1>();

            Assert.That(ReferenceEquals(test1, test2));
        }
    }
}


