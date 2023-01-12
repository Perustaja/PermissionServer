using PermissionServer.Common.Entities;

namespace PermissionServer.Common.EntityFramework
{
    public static class PermissionSeeder<TPerm, TPermCat>
            where TPerm : Enum
            where TPermCat : Enum
    {
        public static IEnumerable<Permission<TPerm, TPermCat>> GetSeedPermissions()
        {
            // It should be impossible to get duplicate enum values here unless ToString() is overridden
            var permsDict = new Dictionary<string, Permission<TPerm, TPermCat>>();
            foreach (TPerm e in Enum.GetValues(typeof(TPerm)))
            {
                var p = new Permission<TPerm, TPermCat>();
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