using Microsoft.EntityFrameworkCore;
using MiskCv_Api.Models;

namespace MiskCv_Api.Data
{
    public class MiskCvDbContext: DbContext
    {
        public MiskCvDbContext(DbContextOptions<MiskCvDbContext> options): base(options)
        {
        }
        public DbSet<MiskCv_Api.Models.User> User { get; set; } = default!;
    }
}
