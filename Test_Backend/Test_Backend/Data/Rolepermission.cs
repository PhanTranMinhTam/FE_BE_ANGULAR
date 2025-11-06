using System.Security;

namespace Test_Backend.Data
{
    public class Rolepermission
    {
        public int IdRole { get; set; }
        public Role Role { get; set; }
        public int IdPermission { get; set; }
        public Permission Permission { get; set; }
    }
}
