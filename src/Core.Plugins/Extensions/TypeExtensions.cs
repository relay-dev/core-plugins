using Core.Framework;
using System;
using System.Linq;

namespace Core.Plugins.Extensions
{
    public static class TypeExtensions
    {
        public static bool UnlessAutoWiringOptOut(this Type type)
        {
            return !type.GetCustomAttributes(typeof(InjectableAttribute), true).Any() ||
                    type.GetCustomAttributes(typeof(InjectableAttribute), true).Select(ia => ((InjectableAttribute)ia).AutoWiring != Opt.Out).FirstOrDefault();
        }
    }
}
