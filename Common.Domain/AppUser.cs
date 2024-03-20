namespace Common.Domain
{
    public class AppUser
    {
        public int Id { get; set; } 
        public string Login { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public virtual IEnumerable<AppUserAppRole> AppUserAppRoles { get; set; }

        public AppUser() { }
        public AppUser(int id, string name)
        {
            Id = id;
            Login = name;
        }
    }
}
