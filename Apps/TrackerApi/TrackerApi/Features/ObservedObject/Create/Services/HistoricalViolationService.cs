using TrackerApi.Infrastructure.Mongo;
using TrackerApi.Infrastructure.Mongo.Documents;
using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Features.ObservedObject.Create.Services
{
    public class HistoricalViolationService : IHistoricalViolationService
    {
        private IDataRepository _repository;
        public HistoricalViolationService(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<Task> Create(ObservedObjectDocument observedObject)
        {            
            var newViolations = observedObject.ViolatedZoneIds.Where(zone => !activeViolations.Select(x => x.ViolatedZoneId).Contains(zone)).ToList();
            foreach (var zoneId in newViolations)
            {
                await _repository.Create(new AnalysisDocument()
                {
                    Id = Guid.NewGuid().ToString(),
                    DeviceId = observedObject.DeviceId,
                    StartViolationTs = observedObject.RecordedAt,
                    RecordedPositionMap = new List<TimeCoordsMap> { new TimeCoordsMap(observedObject.RecordedAt, observedObject.Coordinates) },
                    ViolatedZoneId = zoneId
                });
            }
            return Task.CompletedTask;
        }

        public async Task<Task> Finish(ObservedObjectDocument observedObject)
        {
            var activeViolations = (await _repository.GetAll<AnalysisDocument>()).Where(r => r.DeviceId == observedObject.DeviceId).Where(r => r.EndViolationTs == default).ToList();
            var recordsToFinish = activeViolations.Where(r => !observedObject.ViolatedZoneIds.Contains(r.ViolatedZoneId)).ToList();
            foreach (var record in recordsToFinish)
            {
                record.EndViolationTs = record.RecordedPositionMap.LastOrDefault().Timestamp;
                await _repository.Update(record);
            }
            return Task.CompletedTask;
        }

        public async Task<Task> Update(ObservedObjectDocument observedObject)
        {
            var activeViolations = (await _repository.GetAll<AnalysisDocument>()).Where(r => r.DeviceId == observedObject.DeviceId).Where(r => r.EndViolationTs == default).ToList();
            var recordsToExtend = activeViolations.Where(r => observedObject.ViolatedZoneIds.Contains(r.ViolatedZoneId)).ToList();

            foreach (var record in recordsToExtend)
            {
                record.RecordedPositionMap.Add(new TimeCoordsMap (observedObject.RecordedAt, observedObject.Coordinates));
                await _repository.Update(record);
            }
            return Task.CompletedTask;
        }
    }
}
