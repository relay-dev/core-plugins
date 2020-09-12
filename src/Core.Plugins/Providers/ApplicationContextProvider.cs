using Core.Application;
using Core.Providers;

namespace Core.Plugins.Providers
{
    public class ApplicationContextProvider : IApplicationContextProvider
    {
        private readonly ApplicationContext _applicationContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="applicationContext">The current ApplicationContext</param>
        public ApplicationContextProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Gets the current ApplicationContext
        /// </summary>
        /// <returns>The current ApplicationContext</returns>
        public ApplicationContext Get()
        {
            return _applicationContext;
        }
    }
}
