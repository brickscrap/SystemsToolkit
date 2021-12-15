using Microsoft.AspNetCore.Components;

namespace SystemsUI.Components
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("login");
            base.OnInitialized();
        }
    }
}
