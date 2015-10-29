using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestConstructorInjectionOptional : TestWithContainer
    {
        private class Test1
        {
        }

        private class Test2
        {
            public Test1 val;

            public Test2(Test1 val = null)
            {
                this.val = val;
            }
        }

        private class Test3
        {
            public Test1 val;

            public Test3(Test1 val)
            {
                this.val = val;
            }
        }

        [Test]
        public void TestCase1()
        {
            Container.Bind<Test2>().ToSingle();

            Assert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var test1 = Container.Resolve<Test2>();

            Assert.That(test1.val == null);
        }

        [Test]
        [ExpectedException(typeof(ZenjectResolveException))]
        public void TestCase2()
        {
            Container.Bind<Test3>().ToSingle();

            var test1 = Container.Instantiate<Test3>();

            Assert.That(test1.val == null);
        }

        [Test]
        public void TestConstructByFactory()
        {
            Container.Bind<Test2>().ToSingle();

            var test1 = Container.Instantiate<Test2>();

            Assert.That(test1.val == null);
        }
    }
}


