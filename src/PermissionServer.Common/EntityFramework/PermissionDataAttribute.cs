namespace PermissionServer.Common.EntityFramework
{
    /// <summary>
    /// Allows customization of the underlying permission with extra data to be added to the database.
    /// </summary>
    /// <typeparam name="TPermCat">The category enum associated with your permissions.</typeparam>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class PermissionDataAttribute<TPermCat> : Attribute
        where TPermCat : Enum
    {
        /// <summary>The formatted name of the permission to be displayed to end-users.</summary>
        public string Name { get; set; }
        /// <summary>The formatted description of the permission to be displayed to end-users.</summary>
        public string Description { get; set; }
        /// <summary>
        /// The category this permission belongs to. Must be the type that PermissionServer was
        /// registered with.
        /// </summary>
        public Enum PermissionCategory { get; set; }

        /// <summary>
        /// Allows customization of the underlying permission with extra data to be added to the database.
        /// </summary>
        /// <param name="category">
        /// The category this permission belongs to. Must be the type that PermissionServer was
        /// registered with.
        /// </param>
        /// <param name="name">
        /// The formatted name of the permission to be displayed to end-users.
        /// </param>
        /// <param name="description">
        /// The formatted description of the permission to be displayed to end-users.
        /// </param>
        public PermissionDataAttribute(TPermCat category, string name = "",
            string description = "")
        {
            Name = name;
            PermissionCategory = category;
            Description = description;
        }
    }
}