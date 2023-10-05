using BackendFirma.Models;

namespace BackendFirma.Interfaces
{
    public interface IFirmaProvider
    {
        Task<ICollection<Firma>> GetAllAsync();

        Task<Firma> GetAsync(int id);

        Task<bool> DeleteAsync(int id);

        Task<bool> AddAsync(Firma firma);

        Task<bool> UpdateAsync(int id, Firma firma);
    }
}
