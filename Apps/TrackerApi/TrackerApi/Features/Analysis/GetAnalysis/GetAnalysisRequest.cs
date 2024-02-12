using MediatR;
using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.Analysis.GetAnalysis
{
    public record GetAnalysisRequest() : IRequest<GetAnalysisResponse>;
    public record GetAnalysisResponse(List<ViolationAnalysis> Violations);
    public record ViolationAnalysis(string Id, string DeviceId, string ViolatedZone, List<TimeCoordsMap> RecordedPositionMap, DateTime StartViolationTs, DateTime EndViolationTs);
}