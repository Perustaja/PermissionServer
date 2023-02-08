using System.Reflection;

namespace PermissionServer.Common.Internal
{
    internal class SeedHelpers
    {
        public static IEnumerable<Attribute> GetCustomAttributes(Type t, string member)
        {
            return t
                .GetMember(member)
                .FirstOrDefault(m => m.DeclaringType == t)
                .GetCustomAttributes();
        }
    }
}