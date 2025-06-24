using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;

namespace Services
{
    public class BusyScheduleService
    {
        private readonly IBusyScheduleRepository _busyScheduleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticService _elasticService;

        public BusyScheduleService(
            IBusyScheduleRepository busyScheduleRepository,
            IUnitOfWork unitOfWork,
            IElasticService elasticService)
        {
            _busyScheduleRepository = busyScheduleRepository;
            _unitOfWork = unitOfWork;
            _elasticService = elasticService;
        }

        public async Task<List<BusyScheduleDTO>> GetAllBusySchedulesAsync()
        {
            try
            {
                List<BusySchedule> schedules = await _busyScheduleRepository.GetsAsync();
                return schedules.Select(s => new BusyScheduleDTO
                {
                    ScheduleId = s.ScheduleId,
                    ServiceId = s.ServiceId,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Content = s.Content
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<BusyScheduleDTO?> GetBusyScheduleByIdAsync(int id)
        {
            try
            {
                BusySchedule schedule = await _busyScheduleRepository.GetAsyncById(id);
                if (schedule == null) return null;

                return new BusyScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    ServiceId = schedule.ServiceId,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    Content = schedule.Content
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BusyScheduleDTO>?> GetBusySchedulesByServiceIdAsync(int serviceId)
        {
            try
            {
                List<BusySchedule> schedules = await _busyScheduleRepository.GetByServiceIdAsync(serviceId);
                if (schedules == null) return null;

                return schedules.Select(schedule => new BusyScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    ServiceId = schedule.ServiceId,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    Content = schedule.Content
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateBusyScheduleAsync(BusyScheduleDTO busyScheduleDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                BusySchedule schedule = new()
                {
                    ServiceId = busyScheduleDto.ServiceId,
                    StartDate = busyScheduleDto.StartDate,
                    EndDate = busyScheduleDto.EndDate,
                    Content = busyScheduleDto.Content
                };

                await _busyScheduleRepository.CreateAsync(schedule);
                await _unitOfWork.CommitAsync();
                var scheduleDTO = new BusyScheduleDTO
                {
                    ScheduleId = schedule.ScheduleId,
                    ServiceId = schedule.ServiceId,
                    StartDate = schedule.StartDate,
                    EndDate = schedule.EndDate,
                    Content = schedule.Content
                };

                bool result = await _elasticService.AddOrUpdateBusyScheduleAsync(scheduleDTO);
                if (!result)
                {
                    throw new Exception($"Failed to sync BusySchedule with ID {schedule.ScheduleId} to Elasticsearch");
                }
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateBusyScheduleAsync(int id, BusyScheduleDTO busyScheduleDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                BusySchedule existingSchedule = await _busyScheduleRepository.GetAsyncById(id);
                if (existingSchedule == null) return false;

                existingSchedule.ServiceId = busyScheduleDto.ServiceId;
                existingSchedule.StartDate = busyScheduleDto.StartDate;
                existingSchedule.EndDate = busyScheduleDto.EndDate;
                existingSchedule.Content = busyScheduleDto.Content;

                await _busyScheduleRepository.UpdateAsync(id, existingSchedule);
                await _unitOfWork.CommitAsync();
                var scheduleDTO = new BusyScheduleDTO
                {
                    ScheduleId = existingSchedule.ScheduleId,
                    ServiceId = existingSchedule.ServiceId,
                    StartDate = existingSchedule.StartDate,
                    EndDate = existingSchedule.EndDate,
                    Content = existingSchedule.Content
                };

                bool result = await _elasticService.AddOrUpdateBusyScheduleAsync(scheduleDTO);
                if (!result)
                {
                    throw new Exception($"Failed to sync updated BusySchedule with ID {existingSchedule.ScheduleId} to Elasticsearch");
                }

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteBusyScheduleAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                BusySchedule existingSchedule = await _busyScheduleRepository.GetAsyncById(id);
                if (existingSchedule == null) return false;

                await _busyScheduleRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();

                bool result = await _elasticService.RemoveBusyScheduleAsync(id, existingSchedule.ServiceId);
                if (!result)
                {
                    throw new Exception($"Failed to remove BusySchedule with ID {id} from Elasticsearch");
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<BusyScheduleDTO>> GetBusySchedulesBySupplierIdAsync(int supplierId)
        {
            List<BusySchedule> schedules = await _busyScheduleRepository.GetBySupplierIdAsync(supplierId);

            return schedules.Select(s => new BusyScheduleDTO
            {
                ScheduleId = s.ScheduleId,
                ServiceId = s.ServiceId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Content = s.Content
            }).ToList();
        }
    }
}
