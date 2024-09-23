using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.TrainingTestOptionCommand
{
     public sealed class UpdateTrainingTestOptionCommand : IRequest
    {
        public UpdateTrainingTestOptionCommand(TrainingTestOption movie)
        {
            TrainingTestOption = movie;
        }

        public TrainingTestOption TrainingTestOption { get; set; }


    }

    public class UpdateTrainingTestOptionCommandHandler : IRequestHandler<UpdateTrainingTestOptionCommand>
    {
        private readonly ITrainingTestOptionRepository _repository;

        public UpdateTrainingTestOptionCommandHandler(ITrainingTestOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateTrainingTestOptionCommand request, CancellationToken cancellationToken)
        {

            await _repository.UpdateAsync(request.TrainingTestOption);
        }
    }
}
