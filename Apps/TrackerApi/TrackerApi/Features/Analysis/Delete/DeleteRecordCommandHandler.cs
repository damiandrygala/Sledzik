using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Analysis.Delete
{
    public class DeleteRecordCommandHandler : IRequestHandler<DeleteRecordCommand>
    {
        private readonly IDataRepository _repository;
        public DeleteRecordCommandHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteRecordCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(request));
            var repoRecord = await _repository.Get<AnalysisDocument>(request.Id);
            if (repoRecord is null) return Unit.Value;
            await _repository.Delete(repoRecord);
            return Unit.Value;
        }
    }
}
