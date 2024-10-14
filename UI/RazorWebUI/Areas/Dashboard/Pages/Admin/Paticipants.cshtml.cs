using Application.Queries.TrainingCategoryQueries;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Areas.Dashboard.Pages.Admin
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class DataDto
    {
        public string DataColor { get; set; }
        public int DataCount { get; set; }
        public string DataTitle { get; set; }

        // Add these properties for the category and training counts
        public int CategoryCount { get; set; } // Number of training categories
        public int TrainingCount { get; set; } // Number of trainings
    }



    public class StateModel : PageModel
    {
        private readonly IMediator _mediator; // Inject mediator or service to fetch data
        public StateModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public List<DataDto> DataListDto { get; set; }

        public async Task OnGetAsync()
        {
            DataListDto = new List<DataDto>
    {
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Abia" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Adamawa" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "AkwaIbom" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Anambra" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Bauchi" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Bayelsa" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Benue" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Borno" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Cross River" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Delta" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Ebonyi" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Edo" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Ekiti" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Enugu" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "FCT" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Gombe" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Imo" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Jigawa" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Kaduna" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Kano" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Katsina" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Kebbi" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Kogi" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Kwara" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Lagos" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Nasarawa" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Niger" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Ogun" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Ondo" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Osun" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Oyo" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Plateau" },
        new DataDto { DataColor = "bg-success", DataCount = 0, DataTitle = "Rivers" },
        new DataDto { DataColor = "bg-warning", DataCount = 0, DataTitle = "Sokoto" },
        new DataDto { DataColor = "bg-info", DataCount = 0, DataTitle = "Taraba" },
        new DataDto { DataColor = "bg-danger", DataCount = 0, DataTitle = "Yobe" },
        new DataDto { DataColor = "bg-primary", DataCount = 0, DataTitle = "Zamfara" }

            };



            // Fetch training data grouped by state
            var query = new ListTrainingCategoryQuery();
            var trainingCategories = await _mediator.Send(query);

            // Map the training data into the DataDto list
            foreach (var dto in DataListDto)
            {
                // Count the number of categories and trainings for each state
                var categoriesForState = trainingCategories
                    .Where(tc => tc.Training.Any(t => t.State == dto.DataTitle))
                    .ToList();

                dto.CategoryCount = categoriesForState.Count;
                dto.TrainingCount = categoriesForState.Sum(tc => tc.Training.Count(t => t.State == dto.DataTitle));
            }
        }
    }
}
