using KA.ViewModels.Common;
using KA.ViewModels.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Service.Contacts
{
    public interface IContactService : IService<Contact>
    {
        Task<DataGridResponse<ContactViewModel>> GetAllContactPaging(int skip, int top);
        Task SaveContact(ContactInputModel input);
    }
}
