using Application.Commands.TrainingTestOptionCommand;
using Application.Queries.TrainingTestOptionQueries;
using Application.Queries.TrainingTestQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Areas.ITrainings.Pages.TestQuestions
{
    public class DeleteOptionModel : PageModel
    {
        private readonly IMediator _mediator;

        public DeleteOptionModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public TrainingTestOption TrainingTestOption { get; set; }
        public TrainingTest TrainingTest { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id < 0)
            {
                return NotFound();
            }
            GetByIdTrainingTestOptionQuery Command = new GetByIdTrainingTestOptionQuery(id);
            TrainingTestOption = await _mediator.Send(Command);

            GetByIdTrainingTestQuery xCommand = new GetByIdTrainingTestQuery(TrainingTestOption.TrainingTestId ?? 0);
            TrainingTest = await _mediator.Send(xCommand);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteTrainingTestOptionCommand Command = new DeleteTrainingTestOptionCommand(TrainingTestOption.Id);
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
