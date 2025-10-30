namespace BootstrapBlazorApp1.Server.Model
{
    public class UserTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
