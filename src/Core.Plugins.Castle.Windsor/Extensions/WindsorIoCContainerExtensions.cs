using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;
using Core.Exceptions;
using Core.IoC;

namespace Core.Plugins.Castle.Windsor.Extensions
{
    public static class WindsorIoCContainerExtensions
    {
        public static ComponentRegistration<TService> Set<TService>(this LifestyleGroup<TService> lifestyleGroup, RegistrationLifetime registrationLifetime) where TService : class
        {
            switch (registrationLifetime)
            {
                case RegistrationLifetime.Singleton:
                    lifestyleGroup.Is(LifestyleType.Singleton);
                    break;
                case RegistrationLifetime.Transient:
                    lifestyleGroup.Is(LifestyleType.Transient);
                    break;
                case RegistrationLifetime.PerRequest:
                    lifestyleGroup.Is(LifestyleType.Scoped);
                    break;
                default:
                    throw new CoreException($"Could not resolve a Windsor LifestyleType for IoCLifestyleType type {registrationLifetime}");
            }

            return lifestyleGroup.Registration;
        }
    }
}
