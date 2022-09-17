
using KA.ViewModels.Common;
namespace KA.Service.Address
{
    public interface IAddressService : IService<Province>
    {
        Task<Dictionary<string, string>> GetAllDistrictByProvinceId(string provinceCode);
        Task<Dictionary<string, string>> GetAllProvince();

        Task<Dictionary<string, string>> GetAllWardByDistrictId(string districtCode);
        Task<string> GetDistrictName(string code);
        Task<string> GetProvinceName(string provinceCode);
        Task<string> GetWardName(string code);
    }
}