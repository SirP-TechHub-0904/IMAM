using Application.Queries.TrainingCategoryQueries;
using Application.Queries.TrainingQueries;
using Application.Queries.UserTestQueries;
using Domain.DTOs;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Areas.ITrainings.Pages.Admin
{
     [Microsoft.AspNetCore.Authorization.Authorize]
    public class GeneralStateReportModel : PageModel
    {


        private readonly ILogger<GeneralStateReportModel> _logger;
        private readonly IMediator _mediator;

        public GeneralStateReportModel(ILogger<GeneralStateReportModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public List<UserTestListDto> UserTest { get; set; }
 
        public string StateName { get; set; }
        public async Task<IActionResult> OnGetAsync(string state)
        {
 
            // Initialize the UserTest list for use in the view model
           
                // Create a command to get user test details by training ID
                var command = new ListUserTestByStateQuery(state);
            UserTest = await _mediator.Send(command);

                 
             
            StateName = state.ToUpper();
            return Page();
        }

    }
}
