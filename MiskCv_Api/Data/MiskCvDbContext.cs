namespace MiskCv_Api.Data;

public class MiskCvDbContext: DbContext
{
    public MiskCvDbContext(DbContextOptions<MiskCvDbContext> options): base(options)
    {
    }
    public DbSet<User> User { get; set; } = default!;
    public DbSet<Skill> Skill { get; set; } = default!;
    public DbSet<Company> Company { get; set; } = default!;
    public DbSet<Address> Address { get; set; } = default!;
}
