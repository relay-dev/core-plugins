using System;

namespace Core.Plugins.Utilities.Processor
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class ProcessorAttribute : Attribute
    {
        /// <summary>
        /// Specifies the name of the process the class represents
        /// </summary>
        public string Name { get; set; }
    }
}
