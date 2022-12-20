namespace PermissionServer.Entities
{
    /// <summary>
    /// Join table representing permissions roles have.
    /// </summary>
    public class PSRolePermission<TPerm, TPermCat> 
        where TPerm : Enum
        where TPermCat : Enum
    {
        public Guid RoleId { get; set; }
        public string PermissionId { get; set; }
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