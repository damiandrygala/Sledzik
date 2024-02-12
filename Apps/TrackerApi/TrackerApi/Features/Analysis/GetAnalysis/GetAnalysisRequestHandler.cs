using MediatR;
using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;

namespace TrackerApi.Features.Analysis.GetAnalysis
{
    public class GetAnalysisRequestHandler : IRequestHandler<GetAnalysisRequest, GetAnalysisResponse>
    {
        private readonly IDataRepository _repository;
        public GetAnalysisRequestHandler(IDataRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetAnalysisResponse> Handle(GetAnalysisRequest request, CancellationToken cancellationToken)
        {
            var analysisData = (await _repository.GetAll<AnalysisDocument>()).ToList();
            var analysisResponse = analysisData.Select(d => new ViolationAnalysis(d.Id, d.DeviceId, d.ViolatedZoneId, d.RecordedPositionMap, d.StartViolationTs, d.EndViolationTs)).ToList();
            return new GetAnalysisResponse(analysisResponse);
        }
    }
}
