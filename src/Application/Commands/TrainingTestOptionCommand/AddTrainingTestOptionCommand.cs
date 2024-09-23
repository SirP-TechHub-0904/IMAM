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
    public sealed class AddTrainingTestOptionCommand : IRequest
    {
        public AddTrainingTestOptionCommand(TrainingTestOption trainingTest)
        {
            TrainingTestOption = trainingTest;
        }

        public TrainingTestOption TrainingTestOption { get; set; }


    }

    public class AddTrainingTestOptionCommandHandler : IRequestHandler<AddTrainingTestOptionCommand>
    {
        private readonly ITrainingTestOptionRepository _TrainingTestOptionRepository;

        public AddTrainingTestOptionCommandHandler(ITrainingTestOptionRepository TrainingTestOptionRepository)
        {
            _TrainingTestOptionRepository = TrainingTestOptionRepository;
        }

        public async Task Handle(AddTrainingTestOptionCommand request, CancellationToken cancellationToken)
        {

            await _TrainingTestOptionRepository.AddAsync(request.TrainingTestOption);


        }
    }
}
