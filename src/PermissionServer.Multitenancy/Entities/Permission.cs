using System.ComponentModel.DataAnnotations.Schema;
using PermissionServer.Common.Entities;

namespace PermissionServer.Multitenancy.Entities
{
    /// <summary>
    /// Permission based off of a developer-designed underlying enum.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    public class Permission<TPerm, TPermCat> : BasePermission<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        [ForeignKey("PermCategoryId")]
        public PermissionCategory<TPerm, TPermCat> PermCategory { get; set; }
        
        public Permission() { }
        public Permission(TPerm pEnum) : base(pEnum) {}
    }
}