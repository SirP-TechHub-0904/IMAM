using Application.Queries.IdentityQueries;
using Application.Queries.TrainingCategoryQueries;
using Application.Queries.TrainingQueries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;

namespace RazorWebUI.Areas.ITrainings.Pages.Admin
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
        public TrainingCategory TrainingCategory { get; set; }
        public IEnumerable<Training> Datas { get; private set; }
        public List<StateGroup> StateGroups { get; set; }
        public string? StateName { get; set; }
        public async Task OnGetAsync(long id, string activestate = "")
        {
            var getTrainingCategory = new GetByIdTrainingCategoryQuery(id);
            var result = await _mediator.Send(getTrainingCategory);
            if (result == null)
            {
                id = 0;
            }
            else
            {
                TrainingCategory = result;
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string state = "";
            string stateUserId = null;
            var usercommand = new GetUserByIdQuery(userId);
            var userinfo = await _mediator.Send(usercommand);
            if(userinfo != null)
            {
                state = userinfo.State;
            }
            if(User.IsInRole("Admin") || User.IsInRole("mSuperAdmin"))
            {
                state = "all";
            }
            if (User.IsInRole("TrainingAdmin"))
            {
                stateUserId = userinfo.Id;
            }
            var query = new ListTrainingByCategoryQuery(state, id, stateUserId);
           var activeDatas = await _mediator.Send(query);

            if (!activestate.IsNullOrEmpty())
            {
                Datas = activeDatas.Where(x=>x.State == activestate);
                StateName = activestate;
            }
            else { 
            Datas = activeDatas;
            }
            // Group data by state
            StateGroups = activeDatas.GroupBy(x => x.State)
                               .Select(g => new StateGroup { State = g.Key, Count = g.Count() })
                               .ToList();
        }

    }
    // Model to hold grouped data
    public class StateGroup
    {
        public string State { get; set; }
        public int Count { get; set; }
    }
}
