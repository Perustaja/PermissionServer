using PermissionServer.Common.Entities;

namespace PermissionServer.Entities
{
    /// <summary>
    /// Base entity for a user. 
    /// </summary>
    public class PSUser<TPerm, TPermCat> : BaseUser<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public List<PSUserRole<TPerm, TPermCat>> UserRoles { get; set; }
        
        public PSUser() { }
    }
}