using MediatR;

namespace TrackerApi.Features.Analysis.Delete
{
    public record DeleteRecordCommand(string Id) : IRequest;
}