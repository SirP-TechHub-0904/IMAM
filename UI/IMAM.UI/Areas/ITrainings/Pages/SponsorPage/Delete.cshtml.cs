using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMAM.UI.Areas.ITrainings.Pages.SponsorPage
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class DeleteModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
