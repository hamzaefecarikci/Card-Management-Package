using CardManagement.Data.Entities;
using CardManagement.Data.Repositories;
using CardManagement.Shared.DTOs.MerchantDTOs;

namespace Backoffice.Service.Implementations
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantService(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task<Merchant> CreateMerchant(CreateMerchantDto createMerchantDto)
        {
            var PasswordHash = BCrypt.Net.BCrypt.HashPassword(createMerchantDto.Password);

            var merchant = new Merchant
            {
                Name = createMerchantDto.Name,
                Email = createMerchantDto.Email,
                PasswordHash = PasswordHash,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return await _merchantRepository.AddMerchant(merchant);
        }

        public async Task<GetMerchantDTO?> GetMerchantById(int id)
        {
            var merchant = await _merchantRepository.GetMerchantById(id);
            if (merchant == null) return null;

            return MapToDTO(merchant);
        }

        public async Task<List<GetMerchantDTO>> GetAllMerchants()
        {
            var merchants = await _merchantRepository.GetAllMerchant();
            return merchants.Select(MapToDTO).ToList();
        }

        public async Task<bool> DeleteMerchant(int id)
        {
            return await _merchantRepository.DeleteMerchant(id);
        }

        public async Task<GetMerchantDTO?> UpdateMerchant(int id, UpdateMerchantDto updateMerchantDto)
        {
            var merchant = await _merchantRepository.GetMerchantById(id);
            if (merchant == null) return null;

            if (!string.IsNullOrEmpty(updateMerchantDto.Name)) merchant.Name = updateMerchantDto.Name;
            if (!string.IsNullOrEmpty(updateMerchantDto.Email)) merchant.Email = updateMerchantDto.Email;

            merchant.UpdatedAt = DateTime.Now;

            await _merchantRepository.SaveChanges();
            return MapToDTO(merchant);
        }

        private static GetMerchantDTO MapToDTO(Merchant merchant)
        {
            return new GetMerchantDTO
            {
                Name = merchant.Name,
                Email = merchant.Email,
            };
        }
    }
}
