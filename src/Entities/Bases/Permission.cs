using System.ComponentModel.DataAnnotations;

namespace PermissionServer.Entities.Bases
{
    /// <summary>Base class without navigation properties for polymorphism in dbcontext.</summary>
    public abstract class BasePermission<TPerm, TPermCat> 
        where TPerm : Enum
        where TPermCat : Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PermCategoryId { get; set; }
        
        public BasePermission() { }
        public BasePermission(TPerm pEnum) => Id = pEnum.ToString();
    }
}