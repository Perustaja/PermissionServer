using System.ComponentModel.DataAnnotations;

namespace PermissionServer.Common.Entities
{
    /// <summary>Base class without navigation properties for polymorphism in dbcontext.</summary>
    public abstract class BasePermissionCategory<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public BasePermissionCategory() { }
        public BasePermissionCategory(TPermCat pcEnum) => Id = pcEnum.ToString();
    }
}