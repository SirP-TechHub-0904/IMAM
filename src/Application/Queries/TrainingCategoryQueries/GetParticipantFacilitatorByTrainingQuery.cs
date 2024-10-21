using Domain.DTOs;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.TrainingCategoryQueries
{
        public sealed class GetParticipantFacilitatorByTrainingQuery : IRequest<List<ParticipantAndFacilitatorByCategoryDto>>
    {
        public class GetParticipantFacilitatorByTrainingQueryHandler : IRequestHandler<GetParticipantFacilitatorByTrainingQuery, List<ParticipantAndFacilitatorByCategoryDto>>
        {
            private readonly ITrainingCategoryRepository _repository;

            public GetParticipantFacilitatorByTrainingQueryHandler(ITrainingCategoryRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<ParticipantAndFacilitatorByCategoryDto>> Handle(GetParticipantFacilitatorByTrainingQuery request, CancellationToken cancellationToken)
            {
                var list = await _repository.GetParticipantFacilitatorByTrainingAll();
                return list.ToList();
            }
        }
    }

}
