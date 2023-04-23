using System;
using System.Runtime.Serialization;

namespace IdentityService.ServiceDeployment
{
    [Serializable]
    internal class ServiceDeploymentException : Exception
    {
        public ServiceDeploymentException()
        {
        }

        public ServiceDeploymentException(string message) : base(message)
        {
        }

        public ServiceDeploymentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceDeploymentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}