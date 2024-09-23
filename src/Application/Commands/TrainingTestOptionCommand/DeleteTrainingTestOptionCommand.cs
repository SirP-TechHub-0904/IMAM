using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.TrainingTestOptionCommand
{
      public sealed class DeleteTrainingTestOptionCommand : IRequest
    {
        public DeleteTrainingTestOptionCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }

    }

    public class DeleteTrainingTestOptionCommandHandler : IRequestHandler<DeleteTrainingTestOptionCommand>
    {
        private readonly ITrainingTestOptionRepository _repository;

        public DeleteTrainingTestOptionCommandHandler(ITrainingTestOptionRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteTrainingTestOptionCommand request, CancellationToken cancellationToken)
        {

            var movie = await _repository.GetByIdAsync(request.Id);

            await _repository.RemoveAsync(movie);

        }
    }
}
