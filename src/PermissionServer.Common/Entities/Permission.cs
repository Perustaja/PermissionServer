using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PermissionServer.Common.Entities
{
    /// <summary>
    /// Permission based off of a developer-designed underlying enum.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    [Table("Permissions")]
    public class Permission<TPerm, TPermCat> 
        where TPerm : Enum
        where TPermCat : Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PermCategoryId { get; set; }

        [ForeignKey("PermCategoryId")]
        public PermissionCategory<TPerm, TPermCat> PermCategory { get; set; }
        
        public Permission() { }
        public Permission(TPerm pEnum) => Id = pEnum.ToString();
    }
}