using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace KAWebHost.Pages.Admin.Components
{
    public partial class Select2<TValue> : InputBase<TValue>
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public bool? HideSearch { get; set; }

        [Parameter]
        public string Size { get; set; } = "";

        [Parameter]
        public ICollection<KeyValuePair<string, string>> DataSource { get; set; }

        [Parameter]
        public EventCallback<TValue> OnSelected { get; set; }

        [Parameter]
        public bool IsFromModal { get; set; }

        public DotNetObjectReference<Select2<TValue>> DotNetRef;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (value == "null")
            {
                value = null;
            }
            if (typeof(TValue) == typeof(string))
            {
                result = (TValue)(object)value;
                validationErrorMessage = null;
                return true;
            }
            else if (typeof(TValue) == typeof(int) || typeof(TValue) == typeof(int?))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                result = (TValue)(object)parsedValue;
                validationErrorMessage = null;
                return true;
            }
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            DotNetRef = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("import", "/scripts/components/select2Component.js");
                await JSRuntime.InvokeVoidAsync("select2Component.init", Id, DotNetRef, "Change_Select", HideSearch, IsFromModal);
            }
        }

        [JSInvokable("Change_Select")]
        public void ChangeValue(string value)
        {
            if (value == "null")
            {
                value = null;
            }
            if (typeof(TValue) == typeof(string))
            {
                CurrentValue = (TValue)(object)value;
            }
            else if (typeof(TValue) == typeof(int))
            {
                int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
                CurrentValue = (TValue)(object)parsedValue;
            }
            else if (typeof(TValue) == typeof(int?))
            {
                if (value == null)
                {
                    CurrentValue = (TValue)(object)null;
                }
                else
                {
                    int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedValue);
                    CurrentValue = (TValue)(object)parsedValue;
                }
            }
            OnSelected.InvokeAsync(CurrentValue);
        }

        public void Dispose()
        {
            DotNetRef?.Dispose();
        }
    }
}