namespace TodoAppNTier.UI.Models
{
    public class RoleAssignViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public bool HasRole { get; set; } // Kutucuk işaretli mi?
    }
}