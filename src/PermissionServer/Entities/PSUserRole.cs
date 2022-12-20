namespace PermissionServer.Entities
{
    /// <summary>
    /// Join table that represents the roles a user has.
    /// </summary>
    public class PSUserRole<TPerm, TPermCat>
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public PSUser<TPerm, TPermCat> User { get; set; }
        public PSRole<TPerm, TPermCat> Role { get; set; }
        
        public PSUserRole() { }
        public PSUserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}