using Application.Queries.IdentityQueries;
using Application.Queries.TrainingCategoryQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace RazorWebUI.Areas.ITrainingCategorys.Pages.Category
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class IndexModel : PageModel
    {


        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IEnumerable<TrainingCategoryDto> Datas { get; private set; }
        public string? State { get; set; }
        public async Task OnGetAsync(string state = "")
        {

            var query = new ListTrainingCategoryQuery();
            var categories = await _mediator.Send(query);

            if (!string.IsNullOrEmpty(state))
            {
                // Filter the data and project into DTO
                Datas = categories
                    .Select(category => new TrainingCategoryDto
                    {
                        Id = category.Id,
                        Title = category.Title,
                        Description = category.Description,
                        TrainingCount = category.Training != null
                            ? category.Training.Count(t => string.IsNullOrEmpty(state) || t.State == state)
                            : 0
                    })
                    .ToList();

                State = state;
            }
            else
            {
                Datas = categories
                    .Select(category => new TrainingCategoryDto
                    {
                        Id = category.Id,
                        Title = category.Title,
                        Description = category.Description,
                        TrainingCount = category.Training.Count()
                    })
                    .ToList();
            }
        }

    }

    public class TrainingCategoryDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TrainingCount { get; set; } // This will hold the filtered count
    }
}
