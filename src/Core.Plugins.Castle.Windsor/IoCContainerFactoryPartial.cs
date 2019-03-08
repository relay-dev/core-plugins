//using Core.Application;
//using Core.Framework;
//using Core.IoC;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Core.Plugins.Castle.Windsor
//{
//    public partial class IoCContainerFactory : IIoCContainerFactory
//    {
//        private readonly IFactory _factory;

//        public IoCContainerFactory(IFactory factory)
//        {
//            _factory = factory;
//        }

//        public IIoCContainer Create(IoCContainerType iocContainerType)
//        {
//            throw new NotImplementedException();
//        }

//        public TToCreate Create<TToCreate>() where TToCreate : class, IIoCContainer
//        {
//            return _factory.Create<TToCreate>();
//        }

//        public IIoCContainer Create(string name = null)
//        {
//            return _factory.Create<IIoCContainer>(name);
//        }
//    }
//}
