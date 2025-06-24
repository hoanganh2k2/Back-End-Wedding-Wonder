using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.Extensions.Options;
using Nest;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingWonderAPI.Models;

namespace Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticClient _client;
        private readonly ElasticSettings _elasticSettings;

        public ElasticService(IOptions<ElasticSettings> elasticSettings)
        {
            _elasticSettings = elasticSettings.Value;

            var settings = new ConnectionSettings(new Uri(_elasticSettings.Url))
                .DefaultIndex(_elasticSettings.DefaultIndex);

            _client = new ElasticClient(settings);
        }

        public async Task<bool> AddOrUpdateAsync(ServiceDTO service)
        {
            var searchResponse = await _client.SearchAsync<ServiceDTO>(s => s
                .Index(_elasticSettings.DefaultIndex)
                .Query(q => q.Term(t => t.Field(f => f.ServiceId).Value(service.ServiceId)))
            );

            string? documentId = searchResponse.Hits.FirstOrDefault()?.Id;

            if (documentId == null)
            {
                var indexResponse = await _client.IndexAsync(service, i => i
                    .Index(_elasticSettings.DefaultIndex)
                    .Refresh(Elasticsearch.Net.Refresh.True));

                if (!indexResponse.IsValid)
                {
                    throw new Exception($"Failed to index service with ID {service.ServiceId}: {indexResponse.ServerError?.Error?.Reason ?? indexResponse.OriginalException?.Message}");
                }

                return indexResponse.IsValid;
            }
            else
            {
                var updateResponse = await _client.UpdateAsync<object>(documentId, u => u
                    .Index(_elasticSettings.DefaultIndex)
                    .Doc(service)
                    .Refresh(Elasticsearch.Net.Refresh.True));

                if (!updateResponse.IsValid)
                {
                    throw new Exception($"Failed to update service with ID {service.ServiceId}: {updateResponse.ServerError?.Error?.Reason ?? updateResponse.OriginalException?.Message}");
                }

                return updateResponse.IsValid;
            }
        }

        public async Task<bool> AddOrUpdateBulkAsync(IEnumerable<ServiceDTO> services)
        {
            var response = await _client.BulkAsync(b => b
                .IndexMany(services, (bulk, service) => bulk.Index(_elasticSettings.DefaultIndex)));
            return response.IsValid;
        } 
        public async Task<ServiceDTO> GetServiceByIdFromElasticAsync(int serviceId)
        {
            var searchResponse = await _client.SearchAsync<ServiceDTO>(s => s
                .Index(_elasticSettings.DefaultIndex)
                .Query(q => q.Term(t => t.Field(f => f.ServiceId).Value(serviceId)))
                .Size(1)
            );

            if (!searchResponse.IsValid || !searchResponse.Documents.Any())
            {
                throw new Exception($"Service with ID {serviceId} not found in Elasticsearch.");
            }

            var service = searchResponse.Documents.First();

            return new ServiceDTO
            {
                ServiceId = service.ServiceId,
                SupplierId = service.SupplierId,
                ServiceTypeId = service.ServiceTypeId,
                ServiceName = service.ServiceName,
                Description = service.Description,
                City = service.City,
                District = service.District,
                Ward = service.Ward,
                Address = service.Address,
                StarNumber = service.StarNumber,
                VisitWebsiteLink = service.VisitWebsiteLink,
                IsActive = service.IsActive,
                AvatarImage = service.AvatarImage, 
                AllImage = service.AllImage,
            };
        }

        public async Task<IEnumerable<ServiceDTO>> SearchAsync(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers)
        {
            var mustQueries = new List<Func<QueryContainerDescriptor<ServiceDTO>, QueryContainer>>();
            var mustNotQueries = new List<Func<QueryContainerDescriptor<ServiceDTO>, QueryContainer>>();

            if (serviceTypeIds != null && serviceTypeIds.Any())
            {
                mustQueries.Add(m => m.Terms(t => t.Field(f => f.ServiceTypeId).Terms(serviceTypeIds)));
            }

            if (!string.IsNullOrEmpty(city))
            {
                mustQueries.Add(m => m.Term(t => t.Field(f => f.City).Value(city)));
            }

            if (starNumbers != null && starNumbers.Any())
            {
                mustQueries.Add(m => m.Terms(t => t.Field(f => f.StarNumber).Terms(starNumbers)));
            }

            if (freeScheduleDate.HasValue)
            {
                mustNotQueries.Add(n => n.Nested(nq => nq
                    .Path(p => p.BusySchedules)
                    .Query(nq => nq.Bool(nb => nb
                        .Must(
                            m => m.DateRange(r => r
                                .Field("busySchedules.startDate")
                                .LessThanOrEquals(freeScheduleDate.Value)),
                            m => m.DateRange(r => r
                                .Field("busySchedules.endDate")
                                .GreaterThanOrEquals(freeScheduleDate.Value))
                        )
                    ))
                ));
            }

            var searchResponse = await _client.SearchAsync<ServiceDTO>(s => s
                .Query(q => q.Bool(b => b
                    .Must(mustQueries)
                    .MustNot(mustNotQueries)
                ))
                .Sort(sort => sort
                .Descending(f => f.IsVipSupplier)   
        )
            );

            if (!searchResponse.IsValid)
            {
                throw new Exception($"Elasticsearch query failed: {searchResponse.OriginalException?.Message}");
            }

            return searchResponse.Documents.Select(doc => new ServiceDTO
            {
                ServiceId = doc.ServiceId,
                SupplierId = doc.SupplierId,
                ServiceTypeId = doc.ServiceTypeId,
                ServiceName = doc.ServiceName,
                Description = doc.Description,
                City = doc.City,
                District = doc.District,
                Ward = doc.Ward,
                Address = doc.Address,
                StarNumber = doc.StarNumber,
                VisitWebsiteLink = doc.VisitWebsiteLink,
                IsActive = doc.IsActive,
                AvatarImage = doc.AllImage?.FirstOrDefault() ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg",
                AllImage = doc.AllImage ?? new List<string>(),
                BusySchedules = doc.BusySchedules.Select(bs => new BusyScheduleDTO
                {
                    ScheduleId = bs.ScheduleId,
                    ServiceId = bs.ServiceId,
                    StartDate = bs.StartDate,
                    EndDate = bs.EndDate,
                    Content = bs.Content
                }).ToList()
            });
        }

        public async Task<bool> RemoveAsync(int serviceId)
        {
            var response = await _client.DeleteAsync<Service>(serviceId);
            return response.IsValid;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllServicesAsync()
        {
            var response = await _client.SearchAsync<ServiceDTO>(s => s
                .Query(q => q.MatchAll()).Size(100).Sort(sort => sort
                .Descending(f => f.IsVipSupplier)
        ));

            if (!response.IsValid)
            {
                throw new Exception($"Elasticsearch query failed: {response.OriginalException?.Message}");
            }

            return response.Documents.Select(doc => new ServiceDTO
            {
                ServiceId = doc.ServiceId,
                SupplierId = doc.SupplierId,
                ServiceTypeId = doc.ServiceTypeId,
                ServiceName = doc.ServiceName,
                Description = doc.Description,
                City = doc.City,
                District = doc.District,
                Ward = doc.Ward,
                Address = doc.Address,
                StarNumber = doc.StarNumber,
                VisitWebsiteLink = doc.VisitWebsiteLink,
                IsActive = doc.IsActive,
                AvatarImage = doc.AllImage?.FirstOrDefault() ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg",
                AllImage = doc.AllImage ?? new List<string>(),
                BusySchedules = doc.BusySchedules.Select(bs => new BusyScheduleDTO
                {
                    ScheduleId = bs.ScheduleId,
                    ServiceId = bs.ServiceId,
                    StartDate = bs.StartDate,
                    EndDate = bs.EndDate,
                    Content = bs.Content
                }).ToList()
            });
        }

        public async Task<bool> AddOrUpdateBusyScheduleAsync(BusyScheduleDTO schedule)
        {
            var searchResponse = await _client.SearchAsync<ServiceDTO>(s => s
                .Index("services")
                .Query(q => q.Term(t => t.Field(f => f.ServiceId).Value(schedule.ServiceId)))
            );

            if (!searchResponse.Documents.Any())
            {
                throw new Exception($"Không tìm thấy {schedule.ServiceId}.");
            }

            var documentId = searchResponse.Hits.First().Id;

            var scriptParams = new Dictionary<string, object>
            {
                { "schedule", new
                    {
                        scheduleId = schedule.ScheduleId,
                        serviceId = schedule.ServiceId,
                        startDate = schedule.StartDate.HasValue
                            ? schedule.StartDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")
                            : null,
                        endDate = schedule.EndDate.HasValue
                            ? schedule.EndDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")
                            : null,
                        content = schedule.Content
                    }
                }
            };

            var updateResponse = await _client.UpdateAsync<object>(documentId, u => u
                .Index("services")
                .Script(s => s
                    .Source(@"
                if (ctx._source.busySchedules == null) {
                    ctx._source.busySchedules = [];
                }
                boolean exists = false;
                for (int i = 0; i < ctx._source.busySchedules.size(); i++) {
                    if (ctx._source.busySchedules[i].scheduleId == params.schedule.scheduleId) {
                        ctx._source.busySchedules[i] = params.schedule;
                        exists = true;
                        break;
                    }
                }
                if (!exists) {
                    ctx._source.busySchedules.add(params.schedule);
                }
                ")
                    .Params(scriptParams)
                )
            );

            if (!updateResponse.IsValid)
            {
                throw new Exception($"Lỗi khi update BusySchedule cho Service ID {schedule.ServiceId}: {updateResponse.ServerError?.Error?.Reason ?? updateResponse.OriginalException?.Message}");
            }
            return updateResponse.IsValid;
        }

        public async Task<bool> RemoveBusyScheduleAsync(int scheduleId, int serviceId)
        {
            var searchResponse = await _client.SearchAsync<ServiceDTO>(s => s
                .Index("services")
                .Query(q => q.Term(t => t.Field(f => f.ServiceId).Value(serviceId)))
            );

            if (!searchResponse.Documents.Any())
            {
                throw new Exception($"Ko tìm thấy {serviceId}.");
            }

            var documentId = searchResponse.Hits.First().Id;

            var scriptParams = new Dictionary<string, object>
            {
                { "scheduleId", scheduleId }
            };

            var updateResponse = await _client.UpdateAsync<object>(documentId, u => u
                .Index("services")
                .Script(s => s
                    .Source(@"
                        if (ctx._source.busySchedules != null) {
                            ctx._source.busySchedules.removeIf(bs -> bs.scheduleId == params.scheduleId);
                        }
                    ")
                    .Params(scriptParams)
                )
            );

            if (!updateResponse.IsValid)
            {
                throw new Exception($"Lỗi khi xóa ID {scheduleId} cho Service ID {serviceId}: {updateResponse.ServerError?.Error?.Reason ?? updateResponse.OriginalException?.Message}");
            }

            return updateResponse.IsValid;
        }
    }
}
