namespace PermissionServer.Entities
{
    /// <summary>
    /// Join table representing permissions roles have.
    /// </summary>
    public class PSRolePermission<TPerm, TPermCat> 
        where TPerm : System.Enum
        where TPermCat : System.Enum
    {
        public Guid RoleId { get; private set; }
        public string PermissionId { get; private set; }
        public PSRole<TPerm, TPermCat> Role { get; set; }
        public Permission<TPerm, TPermCat> Permission { get; set; }
        public PSRolePermission() { }
        public PSRolePermission(Guid roleId, TPerm pEnum)
        {
            RoleId = roleId;
            PermissionId = pEnum.ToString();
        }
    }
}