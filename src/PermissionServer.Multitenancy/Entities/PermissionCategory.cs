using PermissionServer.Common.Entities;

namespace PermissionServer.Multitenancy.Entities
{
    /// <summary>
    /// Developer-designed category for grouping and displaying permissions to user.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    public class PermissionCategory<TPerm, TPermCat> : BasePermissionCategory<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<Permission<TPerm, TPermCat>> Permissions { get; set; }
        public PermissionCategory() { }
        public PermissionCategory(TPermCat pcEnum) : base(pcEnum) { }
    }
}