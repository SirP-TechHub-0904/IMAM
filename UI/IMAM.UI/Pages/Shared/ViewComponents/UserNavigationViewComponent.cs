using Microsoft.AspNetCore.Mvc;

namespace IMAM.UI.Pages.Shared.ViewComponents
{
    public class UserNavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
        }
}
