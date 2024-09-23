using Application.Commands.TrainingTestCommand;
using Application.Commands.TrainingTestOptionCommand;
using Application.Queries.TrainingTestQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Areas.ITrainings.Pages.TestQuestions
{
    public class AddOptionModel : PageModel
    {
        private readonly IMediator _mediator;

        public AddOptionModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public TrainingTestOption TrainingTestOption { get; set; }
        public TrainingTest TrainingTest { get; set; }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            GetByIdTrainingTestQuery Command = new GetByIdTrainingTestQuery(id);
            TrainingTest = await _mediator.Send(Command);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                AddTrainingTestOptionCommand Command = new AddTrainingTestOptionCommand(TrainingTestOption);
                await _mediator.Send(Command);
                TempData["success"] = "Success";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                return Page();

            }
        }
    }
}
