using BackendFirma.Models;
using Microsoft.EntityFrameworkCore;
namespace BackendFirma.Context
{
    public class FirmaDBContext: DbContext
    {
        public FirmaDBContext(DbContextOptions<FirmaDBContext> options) : base(options)
        {
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<Firma> Firmas { get; set; }
    }
}
