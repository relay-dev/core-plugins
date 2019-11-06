using Core.Exceptions;
using System;

namespace Core.Plugins.Microservices.Exceptions
{
    public class MicroserviceException : CoreException
    {
        public MicroserviceException(string message) : base(message) { }

        public MicroserviceException(string errorCode, string message) : base(errorCode, message) { }

        public MicroserviceException(Exception innerException, string message) : base(innerException, message) { }

        public MicroserviceException(Exception innerException, string errorCode, string message) : base(innerException, errorCode, message) { }
    }
}
