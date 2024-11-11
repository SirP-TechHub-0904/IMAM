using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserTestQueries
{
    public sealed class ListUserTestByStateQuery : IRequest<List<UserTestListDto>>
    {
        public ListUserTestByStateQuery(string state)
        {
            State = state;
        }

        public string State { get; set; }

        

        public class ListUserTestByStateQueryHandler : IRequestHandler<ListUserTestByStateQuery, List<UserTestListDto>>
        {
            private readonly IUserTestRepository _repository;

            public ListUserTestByStateQueryHandler(IUserTestRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<UserTestListDto>> Handle(ListUserTestByStateQuery request, CancellationToken cancellationToken)
            {
                return await _repository.ListUserTestByState(request.State);

            }
        }
    }

}
