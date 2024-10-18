namespace WorkBuddy.Api.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        //one to many relationship with appuser
        public ICollection<AppUser>? AppUsers { get; set; }

    }
}
