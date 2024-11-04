using System;
using KA.Service.Contacts;
using KA.ViewModels.Contact;
using KAWebHost.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KAWebHost.Pages.Site;

public partial class About : OwningComponentBase
{
    private ContactInputModel contactModel = new();
    private IContactService _contactService;
    
    [CascadingParameter]
    private MainLayout mainLayout { get; set; }
    protected override async Task OnInitializedAsync()
    {
        _contactService = ScopedServices.GetRequiredService<IContactService>();

    }

    protected override void OnAfterRender(bool isFirstRender)
    {
        if (isFirstRender)
        {
            jsr.InvokeVoidAsync("import", "/scripts/site/about.js");
        }
    }

    protected void SaveContactInfo()
    {
        _contactService.SaveContact(contactModel);
        mainLayout.ShowAlert("Cảm ơn bạn đã để lại thông tin! Chúng tôi sẽ liên hệ bạn sớm nhất có thể.", "Thông báo"); contactModel = new();
    }
}
