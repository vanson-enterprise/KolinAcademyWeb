
using AutoMapper;

namespace KA.Service.Address
{
    public class AddressService : BaseService<Province>, IAddressService
    {
        private IRepository<Province> _provinceRepository;
        private IRepository<District> _districtRepository;
        private IRepository<Ward> _wardRepository;
        private IMapper _mapper;
        public AddressService(IRepository<Province> provinceRepository, IRepository<District> districtRepository, IRepository<Ward> wardRepository, IMapper mapper) : base(provinceRepository)
        {
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> GetAllProvince()
        {
            return _provinceRepository.GetAll().Select(p => new { p.code, p.full_name }).ToDictionary(p => p.code, p => p.full_name);
        }

        public async Task<string> GetProvinceName(string provinceCode)
        {
            var province = await _provinceRepository.GetFirstOrDefaultAsync(p => p.code == provinceCode);
            return province?.full_name;
        }

        public async Task<string> GetDistrictName(string code)
        {
            var district = await _districtRepository.GetFirstOrDefaultAsync(p => p.code == code);
            return district?.full_name;
        }

        public async Task<string> GetWardName(string code)
        {
            var ward = await _wardRepository.GetFirstOrDefaultAsync(p => p.code == code);
            return ward?.full_name;
        }


        public async Task<Dictionary<string, string>> GetAllDistrictByProvinceId(string provinceCode)
        {
            return _districtRepository.GetAll().Where(d => d.province_code == provinceCode).Select(p => new { p.code, p.full_name }).ToDictionary(p => p.code, p => p.full_name);
        }

        public async Task<Dictionary<string, string>> GetAllWardByDistrictId(string districtCode)
        {
            return _wardRepository.GetAll().Where(w => w.district_code == districtCode).Select(p => new { p.code, p.full_name }).ToDictionary(p => p.code, p => p.full_name);
        }

      
      

    }
}