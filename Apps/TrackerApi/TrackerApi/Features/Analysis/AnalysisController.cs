using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrackerApi.Features.Analysis.GetAnalysis;
using TrackerApi.Features.Analysis.Delete;

namespace TrackerApi.Features.Analysis
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AnalysisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetAnalysisRequest());
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _mediator.Send(new DeleteRecordCommand(id));
            return Ok();
        }
    }
}
