using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.TrainingTestOptionQueries
{
    public sealed class ListTrainingTestOptionQuery : IRequest<List<TrainingTestOption>>
    {
        public class ListTrainingTestOptionQueryHandler : IRequestHandler<ListTrainingTestOptionQuery, List<TrainingTestOption>>
        {
            private readonly ITrainingTestOptionRepository _repository;

            public ListTrainingTestOptionQueryHandler(ITrainingTestOptionRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<TrainingTestOption>> Handle(ListTrainingTestOptionQuery request, CancellationToken cancellationToken)
            {
                var list = await _repository.GetAllAsync();
                return list.ToList();
            }
        }
    }

}
