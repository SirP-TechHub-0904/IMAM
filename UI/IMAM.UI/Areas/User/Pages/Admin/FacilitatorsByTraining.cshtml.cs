using Application.Dtos;
using Application.Queries.TrainingFacilitatorQueries;
using Domain.DTOs;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IMAM.UI.Areas.User.Pages.Admin
{
     [Microsoft.AspNetCore.Authorization.Authorize]

    public class FacilitatorsByTrainingModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public FacilitatorsByTrainingModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IEnumerable<FacilitatorInTrainingDTo> Datas { get; private set; }
        public Training Training { get; private set; }

        public List<FacilitatorWithTrainingsDTO> Facilitators { get; private set; }

        public async Task<IActionResult> OnGetAsync(string name)
        {

            var query = new ListTrainingFacilitatorQuery();
            var facilitators = await _mediator.Send(query);

            // Group trainings by facilitator
            Facilitators = facilitators
                .GroupBy(f => new { f.Id, f.FullnameX, f.Email, f.PhoneNumber, f.Position, f.Role, f.UserStatus })
                .Select(group => new FacilitatorWithTrainingsDTO
                {
                    Id = group.Key.Id,
                    Fullname = group.Key.FullnameX,
                    Email = group.Key.Email,
                    PhoneNumber = group.Key.PhoneNumber,
                    Position = group.Key.Position,
                    Role = group.Key.Role, 
                    Trainings = group.Select(t => t.TrainingInformation).Distinct().ToList()
                })
                .ToList();

            return Page();
        }
    }
}
