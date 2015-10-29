using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModestTree.Util.Debugging;
using Zenject;
using NUnit.Framework;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    public class TestWithContainer
    {
        DiContainer _container;

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        [SetUp]
        public virtual void Setup()
        {
            _container = new DiContainer();
            InstallBindings();

            Validate();
            _container.Inject(this);
        }

        void Validate()
        {
            var errors = _container.ValidateValidatables().Take(5).ToList();

            if (!errors.IsEmpty())
            {
                throw errors.First();

                // Print out 5 so you don't have to recompile and execute one by one
                //foreach (var err in errors)
                //{
                    //Log.ErrorException(err);
                //}

                //throw new Exception("Zenject Validation failed");
            }
        }

        protected virtual void InstallBindings()
        {
        }

        [TearDown]
        public virtual void Destroy()
        {
            _container = null;
        }
    }
}
