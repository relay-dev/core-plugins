using Castle.Facilities.TypedFactory;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers;
using Castle.Windsor;
using Core.Exceptions;
using Core.IoC;
using Core.Plugins.Castle.Windsor.Extensions;
using Core.Plugins.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Plugins.Castle.Windsor.Impl
{
    public class WindsorIoCContainer : IoCContainerBase
    {
        private readonly WindsorContainer _windsorContainer;

        public WindsorIoCContainer()
        {
            _windsorContainer = new WindsorContainer();

            Init();
        }

        public override IDisposable BeginScope()
        {
            return _windsorContainer.BeginScope();
        }

        public override IIoCContainer Register<TService, TImplementation>(IoCContainerSettings ioCContainerSettings = null)
        {
            base.Register<TService, TImplementation>(ioCContainerSettings);

            ComponentRegistration<TService> componentRegistration = 
                Component.For<TService>().ImplementedBy<TImplementation>().Named(Registration.Name).LifeStyle.Set(Registration.RegistrationLifetime);

            RegisterService(componentRegistration, ioCContainerSettings);

            return this;
        }

        public override IIoCContainer Register(Type serviceType, Type implementationType, IoCContainerSettings ioCContainerSettings = null)
        {
            base.Register(serviceType, implementationType, ioCContainerSettings);

            ComponentRegistration<object> componentRegistration = 
                Component.For(serviceType).ImplementedBy(implementationType).Named(Registration.Name).LifeStyle.Set(Registration.RegistrationLifetime);

            RegisterService(componentRegistration, ioCContainerSettings);

            return this;
        }

        public override IIoCContainer Register<TService>(TService instance, IoCContainerSettings ioCContainerSettings = null)
        {
            base.Register<TService>(instance, ioCContainerSettings);

            ComponentRegistration<TService> componentRegistration =
                Component.For<TService>().Instance(instance).Named(Registration.Name).LifeStyle.Set(Registration.RegistrationLifetime);

            RegisterService(componentRegistration, ioCContainerSettings);

            return this;
        }

        public override IIoCContainer RegisterFactory<TFactory>(IoCContainerSettings ioCContainerSettings = null)
        {
            base.RegisterFactory<TFactory>(ioCContainerSettings);

            if (!IsFacilityRegistered(typeof(TypedFactoryFacility)))
            {
                _windsorContainer.AddFacility<TypedFactoryFacility>();
            }

            ComponentRegistration<TFactory> componentRegistration =
                Component.For<TFactory>().AsFactory().Named(Registration.Name).LifeStyle.Set(Registration.RegistrationLifetime);

            RegisterService(componentRegistration, ioCContainerSettings);

            return this;
        }

        public void ThrowIfInvalid()
        {
            List<IHandler> handlers = GetHandlersForType(typeof(object));

            var stringBuilder = new StringBuilder();

            foreach (IHandler handler in handlers)
            {
                try
                {
                    Resolve(handler.ComponentModel.Services.FirstOrDefault());
                }
                catch (Exception)
                {
                    stringBuilder.AppendLine(GetRegistrationDetails(handler));
                }
            }

            if (stringBuilder.ToString().Length > 1)
            {
                throw new CoreException(ErrorCode.CORE, $"Invalid service(s) found:{Environment.NewLine}{stringBuilder.ToString()}");
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder(string.Empty.PadRight(50, '='));

            stringBuilder.AppendLine();

            foreach (IoCContainerRegistration registration in Registrations)
            {
                stringBuilder.AppendLine(registration.ToString());

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }

        private static void Init()
        {
            // Add support for Castle Windsor to automatically inject ILazy<TService> with only a mapping from TService to TImpl
            Component.For<ILazyComponentLoader>().ImplementedBy<LazyOfTComponentLoader>();
        }

        private void RegisterService<TService>(ComponentRegistration<TService> componentRegistration, IoCContainerSettings ioCContainerSettings) where TService : class
        {
            ioCContainerSettings ??= new IoCContainerSettings();

            if (GetHandlersForType(typeof(TService)).Any())
            {
                switch (ioCContainerSettings.DuplicateRegistrationOption)
                {
                    case DuplicateRegistrationOption.ThrowException:
                        throw new CoreException($"Duplicate registration detected. The following registration(s) already exist for component {typeof(TService).Name}: {String.Join(", ", GetHandlersForType(typeof(TService)).Select(h => h.ComponentModel.Name))}");
                    case DuplicateRegistrationOption.OverrideExistingRegistration:
                        componentRegistration.IsDefault();
                        return;
                    case DuplicateRegistrationOption.IgnoreRegistrationRequest:
                        return;
                    default:
                        throw new CoreException($"WindsorContainerIoCWrapper did not have a handler for IoCContainerSetting.IoCDuplicateRegistrationOption for option {ioCContainerSettings.DuplicateRegistrationOption}");
                }
            }

            _windsorContainer.Register(componentRegistration);
        }

        private string GetRegistrationDetails(IHandler handler)
        {
            var stringBuilder = new StringBuilder(string.Empty.PadRight(50, '='));

            if (!handler.ComponentModel.Services.Any())
            {
                stringBuilder.AppendLine($"Service        : <No services found>");
            }
            else if (handler.ComponentModel.Services.Count() == 1)
            {
                stringBuilder.AppendLine($"Service        : {handler.ComponentModel.Services.Single().FullName})");
            }
            else
            {
                stringBuilder.AppendLine($"Services       : {string.Join(", ", handler.ComponentModel.Services.Select(svc => svc.FullName))}");
            }

            stringBuilder.AppendLine($"Implementation : {handler.ComponentModel.Implementation.FullName})");
            stringBuilder.AppendLine($"Lifecycle      : {string.Join(", ", handler.ComponentModel.Lifecycle.CommissionConcerns.Select(c => c.ToString()))}");
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        private bool IsFacilityRegistered(Type type)
        {
            return _windsorContainer.Kernel.GetFacilities().Any(f => f.GetType() == type);
        }

        private List<IHandler> GetHandlersForType(Type type)
        {
            return _windsorContainer.Kernel.GetAssignableHandlers(type).ToList();
        }
    }
}
