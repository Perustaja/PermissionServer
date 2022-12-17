using PermissionServer.Attributes;
using PermissionServer.Entities.Bases;

namespace PermissionServer.EntityFramework.Configuration.Common
{
    internal class DefaultPermissionSeeder<TPerm, TPermCat, TPermEntity> 
        : IPermissionSeeder<TPerm, TPermCat, TPermEntity>
            where TPerm : Enum
            where TPermCat : Enum
            where TPermEntity : BasePermission<TPerm, TPermCat>
    {
        public IEnumerable<TPermEntity> GetSeedPermissions()
        {
            // It should be impossible to get duplicate enum values here unless ToString() is overridden
            var permsDict = new Dictionary<string, TPermEntity>();
            foreach (TPerm e in Enum.GetValues(typeof(TPerm)))
            {
                var p = Activator.CreateInstance(typeof(TPermEntity), e) as TPermEntity;
                var attribs = SeedHelpers.GetCustomAttributes(typeof(TPerm), e.ToString());

                var seedData = attribs.OfType<PermissionDataAttribute>().First();
                p.Name = seedData?.Name ?? p.Id;
                p.Description = seedData?.Description ?? String.Empty;
                p.PermCategoryId = seedData?.PermissionCategory?.ToString() ?? String.Empty;

                if (!permsDict.TryAdd(p.Id, p))
                    throw new Exception($"Permission seeding failed. {p.Id} was found twice when calling {typeof(TPerm)}.ToString(). Ensure each enum member has a unique ToString() value.");
            }
            return permsDict.Values;
        }
    }
}