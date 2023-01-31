using AutoMapper;
using KA.ViewModels.Common;
using KA.ViewModels.Contact;
using KA.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Contacts
{
    public class ContactService : BaseService<Contact>, IContactService
    {
        public IRepository<Contact> _contactReponsitory { get; }
        private IMapper _mapper;

        public ContactService(IRepository<Contact> baseReponsitory, IMapper mapper) : base(baseReponsitory)
        {
            _contactReponsitory = baseReponsitory;
            _mapper = mapper;
        }

        public async Task SaveContact(ContactInputModel input)
        {
            await _contactReponsitory.AddAsync(_mapper.Map<Contact>(input));
        }

        public async Task<DataGridResponse<ContactViewModel>> GetAllContactPaging(int skip, int top)
        {
            var result = new DataGridResponse<ContactViewModel>();

            var registers = (from c in _contactReponsitory.GetAll()
                             select c).OrderByDescending(r => r.CreatedDate).ToList();

            result.TotalItem = registers.Count();
            result.Items = registers.Skip(skip).Take(top).ToList().Select((c, i) =>
            {
                var ci = new ContactViewModel()
                {
                    Id = c.Id,
                    CreatedDate = c.CreatedDate.ToString("dd/MM/yyyy"),
                    Email = c.Email,
                    FullName = c.FullName,
                    Index = (i + 1) + skip,
                    PhoneNumber = c.PhoneNumber
                };
                return ci;
            }).ToList();
            return result;
        }
    }
}
