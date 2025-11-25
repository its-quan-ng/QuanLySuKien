namespace QuanLySuKien.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = default!;
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
