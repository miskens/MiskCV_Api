using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MiskCv_Api.Data;

public class MiskIdentityDbContext: IdentityDbContext
{
    public MiskIdentityDbContext(DbContextOptions<MiskIdentityDbContext> options): base(options)
    { }
}
