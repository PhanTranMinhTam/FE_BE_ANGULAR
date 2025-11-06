namespace Test_Backend.Models
{
    public class PermisstionRoleDTO
    {
        public int RoleId { get; set; }
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
