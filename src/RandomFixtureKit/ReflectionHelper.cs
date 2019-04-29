using System;

namespace RandomFixtureKit
{
    internal static class ReflectionHelper
    {
        // helper for IL2CPP(Unity) debugging, show type info in exception.

        internal static object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                throw new Exception("Fail to create instance, type:" + type.FullName);
            }
        }

        internal static object CreateInstance(Type type, params object[] args)
        {
            try
            {
                return Activator.CreateInstance(type, args);
            }
            catch
            {
                throw new Exception("Fail to create instance, type:" + type.FullName);
            }
        }
    }
}
