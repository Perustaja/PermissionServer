using System.ComponentModel.DataAnnotations;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Developer-designed category for grouping and displaying permissions to user.
    /// </summary>
    /// <typeparam name="TPerm">The underlying permission enum</typeparam>
    /// <typeparam name="TPermCat">The underlying permission category enum</typeparam>
    public class PermissionCategory<TPerm, TPermCat> 
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsObsolete { get; set; }
        public List<Permission<TPerm, TPermCat>> Permissions { get; set; }
        public PermissionCategory() { }
        public PermissionCategory(TPermCat pcEnum) => Id = pcEnum.ToString();
    }
}