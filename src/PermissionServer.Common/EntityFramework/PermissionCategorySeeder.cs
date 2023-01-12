using PermissionServer.Common.Entities;

namespace PermissionServer.Common.EntityFramework
{
    public static class PermissionCategorySeeder<TPerm, TPermCat, TPermCatEntity>
            where TPerm : Enum
            where TPermCat : Enum
            where TPermCatEntity : BasePermissionCategory<TPerm, TPermCat>
    {
        public static IEnumerable<TPermCatEntity> GetSeedPermissionCategories()
        {
            // Note that there is a one-many relation modeled in EF core, so these categories can
            // have their permissions pulled out using the dbcontext and LINQ include just by the foreign key,
            // adding the permissions here is unnecessary

            // It should be impossible to get duplicate enum values here unless ToString() is overridden
            var categoriesDict = new Dictionary<string, TPermCatEntity>();
            foreach (TPermCat e in Enum.GetValues(typeof(TPermCat)))
            {
                var pc = Activator.CreateInstance(typeof(TPermCatEntity), e) as TPermCatEntity;
                var attribs = SeedHelpers.GetCustomAttributes(typeof(TPermCat), e.ToString());

                var seedData = attribs.OfType<CategoryDataAttribute>().First();
                pc.Name = seedData.Name ?? pc.Id;

                if (!categoriesDict.TryAdd(pc.Id, pc))
                    throw new Exception($"Permission seeding failed. {pc.Id} was found twice when calling {typeof(TPermCat)}.ToString(). Ensure each enum member has a unique ToString() value.");
            }
            return categoriesDict.Values;
        }
    }
}