using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMAM.UI.Areas.ITrainings.Pages.GalleryPage
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class InfoModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
