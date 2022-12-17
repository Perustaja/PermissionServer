using System.Reflection;

namespace PermissionServer.EntityFramework.Configuration.Common
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