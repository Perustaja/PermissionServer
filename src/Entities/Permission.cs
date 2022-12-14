using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Permission based off of a developer-designed underlying enum.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    public class Permission<TPerm, TPermCat> 
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsObsolete { get; set; }
        public string PermCategoryId { get; set; }
        [ForeignKey("PermCategoryId")]
        public PermissionCategory<TPerm, TPermCat> PermCategory { get; set; }
        public List<PSRolePermission<TPerm, TPermCat>> RolePermissions { get; set; }
        public Permission() { }
        public Permission(TPerm pEnum) => Id = pEnum.ToString();
    }
}