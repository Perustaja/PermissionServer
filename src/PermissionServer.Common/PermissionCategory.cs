using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionServer.Common
{
    /// <summary>
    /// Developer-designed category for grouping and displaying permissions to user.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    [Table("PermissionCategories")]
    public class PermissionCategory<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Permission<TPerm, TPermCat>> Permissions { get; set; }

        public PermissionCategory() { }
        public PermissionCategory(TPermCat pcEnum) => Id = pcEnum.ToString();
    }
}