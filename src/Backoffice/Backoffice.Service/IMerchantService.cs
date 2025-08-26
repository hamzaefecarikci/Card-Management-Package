using CardManagement.Data.Entities;
using CardManagement.Shared.DTOs.MerchantDTOs;

namespace Backoffice.Service
{
    public interface IMerchantService
    {
        Task<Merchant> CreateMerchant(CreateMerchantDto dto);
        Task<GetMerchantDTO?> GetMerchantById(int id);
        Task<List<GetMerchantDTO>> GetAllMerchants();
        Task<bool> DeleteMerchant(int id);
        Task<GetMerchantDTO?> UpdateMerchant(int id, UpdateMerchantDto dto);
    }
}
