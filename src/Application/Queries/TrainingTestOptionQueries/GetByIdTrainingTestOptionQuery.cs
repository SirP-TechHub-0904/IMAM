using Application.Validators;
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
    public sealed class GetByIdTrainingTestOptionQuery : IRequest<TrainingTestOption>
    {
        public GetByIdTrainingTestOptionQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }

        // Handler
        public class GetByIdTrainingTestOptionQueryHandler : IRequestHandler<GetByIdTrainingTestOptionQuery, TrainingTestOption>
        {

            private readonly ITrainingTestOptionRepository _repository;

            public GetByIdTrainingTestOptionQueryHandler(ITrainingTestOptionRepository repository)
            {
                _repository = repository;
            }
            public async Task<TrainingTestOption> Handle(GetByIdTrainingTestOptionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                TrainingTestOption data = await _repository.GetByIdAsync(request.Id);

                return data;
            }
        }
    }

}
