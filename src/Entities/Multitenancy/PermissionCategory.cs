using System.ComponentModel.DataAnnotations;
using PermissionServer.Entities.Bases;

namespace PermissionServer.Entities.Multitenancy
{
    /// <summary>
    /// Developer-designed category for grouping and displaying permissions to user.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    public sealed class PermissionCategory<TPerm, TPermCat> : BasePermissionCategory<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<Permission<TPerm, TPermCat>> Permissions { get; set; }
        public PermissionCategory() { }
        public PermissionCategory(TPermCat pcEnum) : base(pcEnum) { }
    }
}