namespace Task4.Db
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime LastSeen { get; set; }
    }
}
