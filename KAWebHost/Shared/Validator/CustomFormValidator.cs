using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KAWebHost.Shared.Validator
{
    public class CustomFormValidator : ComponentBase
    {
        private ValidationMessageStore validationMessageStore; // maintain the current list of form errors

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; } // hold metadata related to a data editing process

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomFormValidator)} requires a cascading parameter of type {nameof(EditContext)}.");
            }

            validationMessageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnValidationRequested += (s, e) =>
                validationMessageStore.Clear();
            CurrentEditContext.OnFieldChanged += (s, e) =>
                validationMessageStore.Clear(e.FieldIdentifier);
        }
        public void DisplayFormErrors(Dictionary<string, List<string>> errors)
        {
            foreach (var err in errors)
            {
                validationMessageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        public void ClearFormErrors()
        {
            validationMessageStore.Clear();
            CurrentEditContext.NotifyValidationStateChanged();
        }
    }
}