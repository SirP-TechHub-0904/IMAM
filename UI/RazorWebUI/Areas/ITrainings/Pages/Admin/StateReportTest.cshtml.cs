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
    public class StateReportTestModel : PageModel
    {


        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public StateReportTestModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public List<UserTestListDto> UserTest { get; set; }
        public TrainingCategory TrainingCategory { get; set; }

        public string StateName { get; set; }
        public async Task<IActionResult> OnGetAsync(long id, string state)
        {

            var getTrainingCategory = new GetByIdTrainingCategoryQuery(id);
            var result = await _mediator.Send(getTrainingCategory);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                TrainingCategory = result;
            }
            // Create a query to get training details by category ID
            var query = new ListTrainingByCategoryQuery(state, id, "");
            var activeDatas = await _mediator.Send(query);

            // Initialize the UserTest list for use in the view model
            UserTest = new List<UserTestListDto>();

            foreach (var item in activeDatas)
            {
                // Create a command to get user test details by training ID
                var command = new ListUserTestByTrainingQuery(item.Id);
                var userTests = await _mediator.Send(command);

                // Add the user test data to the model
                UserTest.AddRange(userTests);
            }
            StateName = state.ToUpper();
            return Page();
        }

    }
}
