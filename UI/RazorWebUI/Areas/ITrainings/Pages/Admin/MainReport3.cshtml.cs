using Application.Queries.TrainingCategoryQueries;
using Application.Queries.TrainingQueries;
using Domain.DTOs;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWebUI.Areas.ITrainings.Pages.Admin
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class MainReport3Model : PageModel
    {


        private readonly ILogger<MainReport3Model> _logger;
        private readonly IMediator _mediator;
 
        public MainReport3Model(ILogger<MainReport3Model> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public List<ParticipantAndFacilitatorByCategoryDto> Datas { get; private set; }

        public async Task OnGetAsync()
        {

            var query = new GetParticipantFacilitatorByTrainingQuery();
            Datas = await _mediator.Send(query);
             
        }

    }
}
