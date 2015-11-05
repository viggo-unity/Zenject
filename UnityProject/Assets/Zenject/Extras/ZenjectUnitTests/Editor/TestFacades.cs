using System;
using NUnit.Framework;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestFacades
    {
        public class FooFacade : Facade
        {
            public class Factory : FacadeFactory<FooFacade>
            {
            }
        }

        public class MyTickable : ITickable
        {
            public static bool HasTicked;

            public void Tick()
            {
                HasTicked = true;
            }
        }

        public class MyInit : IInitializable
        {
            public static bool HasInitialized;

            public void Initialize()
            {
                HasInitialized = true;
            }
        }

        public class MyDispose : IDisposable
        {
            public static bool HasDisposed;

            public void Dispose()
            {
                HasDisposed = true;
            }
        }

        [Test]
        public void Test()
        {
            MyTickable.HasTicked = false;
            MyInit.HasInitialized = false;
            MyDispose.HasDisposed = false;

            var container = new DiContainer();
            container.BindFacadeFactory<FooFacade, FooFacade.Factory>(FacadeInstaller);

            var facadeFactory = container.Resolve<FooFacade.Factory>();

            Assert.That(!MyInit.HasInitialized);
            var facade = facadeFactory.Create();
            Assert.That(MyInit.HasInitialized);

            Assert.That(!MyTickable.HasTicked);
            facade.Tick();
            Assert.That(MyTickable.HasTicked);

            Assert.That(!MyDispose.HasDisposed);
            facade.Dispose();
            Assert.That(MyDispose.HasDisposed);
        }

        void FacadeInstaller(DiContainer subContainer)
        {
            subContainer.Bind<IDisposable>().ToSingle<MyDispose>();
            subContainer.Bind<IInitializable>().ToSingle<MyInit>();
            subContainer.Bind<ITickable>().ToSingle<MyTickable>();
        }
    }
}
