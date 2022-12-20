namespace PermissionServer.Common.EntityFramework
{
    /// <summary>
    /// Allows customization of the underlying permission category with extra data to be added to the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CategoryDataAttribute : Attribute
    {
        /// <summary>The formatted name of the category to be displayed to end-users.</summary>
        public string Name { get; set; }

        /// <summary>
        /// Allows customization of the underlying category with extra data to be added to the database.
        /// </summary>
        /// <param name="name">
        /// The formatted name of the category to be displayed to end-users.
        /// </param>
        public CategoryDataAttribute(string name = "")
        {
            Name = name;
        }
    }
}