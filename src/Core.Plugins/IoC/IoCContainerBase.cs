using Core.Exceptions;
using Core.Framework;
using Core.IoC;
using Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.IoC
{
    public class IoCContainerBase : IIoCContainer
    {
        private readonly List<IoCContainerRegistration> _registrations;

        public IoCContainerBase()
        {
            _registrations = new List<IoCContainerRegistration>();
        }

        public virtual IIoCContainer Register<TService, TImplementation>(IoCContainerSettings ioCContainerSettings = null) where TService : class where TImplementation : TService
        {
            ioCContainerSettings = ioCContainerSettings ?? Settings;

            _registration = new IoCContainerRegistration
            (
                serviceType: typeof(TService),
                implementationType: typeof(TImplementation),
                registrationLifetime: GetRegistrationLifetime(ioCContainerSettings),
                name: GetRegistrationName(ioCContainerSettings, typeof(TImplementation).FullName),
                isFactory: false
            );

            Registrations.Add(_registration);

            return this;
        }

        public virtual IIoCContainer Register(Type serviceType, Type implementationType, IoCContainerSettings ioCContainerSettings = null)
        {
            _registration = new IoCContainerRegistration
            (
                serviceType: serviceType,
                implementationType: implementationType,
                registrationLifetime: GetRegistrationLifetime(ioCContainerSettings),
                name: GetRegistrationName(ioCContainerSettings, implementationType.FullName),
                isFactory: false
            );

            Registrations.Add(_registration);

            return this;
        }

        public virtual IIoCContainer Register<TService>(TService instance, IoCContainerSettings ioCContainerSettings = null) where TService : class
        {
            _registration = new IoCContainerRegistration
            (
                serviceType: typeof(TService),
                implementationType: instance.GetType(),
                registrationLifetime: GetRegistrationLifetime(ioCContainerSettings),
                name: GetRegistrationName(ioCContainerSettings, instance.GetType().FullName),
                isFactory: false
            );

            Registrations.Add(_registration);

            return this;
        }

        public virtual IIoCContainer RegisterFactory<TFactory>(IoCContainerSettings ioCContainerSettings = null) where TFactory : class
        {
            _registration = new IoCContainerRegistration
            (
                serviceType: typeof(TFactory),
                implementationType: null,
                registrationLifetime: GetRegistrationLifetime(ioCContainerSettings),
                name: GetRegistrationName(ioCContainerSettings, typeof(TFactory).FullName),
                isFactory: true
            );

            Registrations.Add(_registration);

            return this;
        }

        public virtual bool IsRegistered<TService>()
        {
            return _registrations.Any(registration => registration.ServiceType == typeof(TService));
        }

        public virtual void Release(object instance)
        {
            IoCContainerRegistration registration = _registrations.Find(r => r == instance);

            _registrations.Remove(registration);
        }

        public virtual TService Resolve<TService>(string registrationName = null)
        {
            var objectQuery = _registrations.Where(r => r.ServiceType == typeof(TService));

            if (String.IsNullOrEmpty(registrationName))
            {
                objectQuery = objectQuery.Where(r => String.Equals(registrationName, r.Name, StringComparison.CurrentCultureIgnoreCase));
            }

            List<IoCContainerRegistration> registrations = objectQuery.ToList();

            if (!registrations.Any())
                throw new CoreException(ErrorCode.CORE, $"Could not find any registrations for {typeof(TService)} with Name {registrationName}");

            if (registrations.Count > 1)
                throw new CoreException(ErrorCode.CORE, $"Multiple registrations found for {typeof(TService)} with Name = {registrationName}");

            return (TService)Activator.CreateInstance(registrations.Single().ServiceType);
        }

        public virtual object Resolve(Type service)
        {
            List<IoCContainerRegistration> registrations = _registrations.Where(r => r.ServiceType == service.GetType()).ToList();

            if (!registrations.Any())
                throw new CoreException(ErrorCode.CORE, $"Could not find any registrations for {service.GetType().Name}");

            if (registrations.Count > 1)
                throw new CoreException(ErrorCode.CORE, $"Multiple registrations found for {service.GetType().Name}");

            return Activator.CreateInstance(registrations.Single().ServiceType);
        }

        private IoCContainerSettings _settings;
        public virtual IoCContainerSettings Settings
        {
            get
            {
                return _settings ?? (_settings = new IoCContainerSettings());
            }
            set
            {
                _settings = value;
            }
        }

        private IoCContainerRegistration _registration;
        public virtual IoCContainerRegistration Registration
        {
            get { return _registration; }
        }

        public virtual List<IoCContainerRegistration> Registrations
        {
            get { return _registrations; }
        }

        #region Private

        private RegistrationLifetime GetRegistrationLifetime(IoCContainerSettings ioCContainerSettings)
        {
            return (ioCContainerSettings == null || ioCContainerSettings.RegistrationLifetime == RegistrationLifetime.Undefined)
                ? Settings.RegistrationLifetime : ioCContainerSettings.RegistrationLifetime;
        }

        private string GetRegistrationName(IoCContainerSettings ioCContainerSettings, string implementationName)
        {
            return (ioCContainerSettings == null || String.IsNullOrEmpty(ioCContainerSettings.RegistrationNameFormat))
                ? String.Format(Settings.RegistrationNameFormat, implementationName) : String.Format(ioCContainerSettings.RegistrationNameFormat, implementationName);
        }

        #endregion
    }
}
