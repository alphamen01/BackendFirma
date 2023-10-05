using BackendFirma.Context;
using BackendFirma.Interfaces;
using BackendFirma.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendFirma.Providers
{
    public class FirmaProvider : IFirmaProvider
    {
        private readonly FirmaDBContext context;

        public FirmaProvider(FirmaDBContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(Firma firma)
        {
            context.Firmas.Add(firma);
            var result = await context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = context.Firmas.Where(f => f.id_firma == id).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            else
            {
                context.Firmas.Remove(result);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<ICollection<Firma>> GetAllAsync()
        {
            var results = await context.Firmas
                                .OrderBy(f => f.id_firma)
                                .ToListAsync();
            return results;
        }

        public async Task<Firma> GetAsync(int id)
        {
            var result = await context.Firmas.FirstOrDefaultAsync(f =>
            f.id_firma == id);
            return result!;
        }

        public async Task<bool> UpdateAsync(int id, Firma firma)
        {
            context.Firmas.Update(firma);
            var result = await context.SaveChangesAsync();
            return result == 1;
        }
    }
}
