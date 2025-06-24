using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using MassTransit;
using MimeKit;
using Repository.IRepositories;
using Services.Email;
using WeddingWonderAPI.Models;
using Message = Services.Email.Message;

namespace Services
{
    public class ServiceManager
    {
        private readonly IPhotographServiceRepository _photographServiceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IEventOrganizerServiceRepository _EventOrganizerServiceRepository;
        private readonly IMakeUpServiceRepository _MakeUpServiceRepository;
        private readonly IInvitationServiceRepository _invitationServiceRepository;
        private readonly IClothesServiceRepository _ClothesServiceRepository;
        private readonly IRestaurantServiceRepository _RestaurantServiceRepository;
        private readonly IEmailService _emailService;
        private readonly IServiceImageRepository _imageRepository;
        private readonly IBusyScheduleRepository _busyScheduleRepository;
        private readonly IElasticService _elasticService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint;

        public ServiceManager(
            IPhotographServiceRepository photographServiceRepository,
            IServiceRepository serviceRepository,
            IUnitOfWork unitOfWork,
            IEventOrganizerServiceRepository EventOrganizerServiceRepository,
            IMakeUpServiceRepository makeUpServiceRepository,
            IInvitationServiceRepository invitationServiceRepository,
            IClothesServiceRepository clothesServiceRepository,
            IRestaurantServiceRepository restaurantServiceRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IServiceImageRepository imageRepository,
            IElasticService elasticService,
            IBusyScheduleRepository busyScheduleRepository,
            IPublishEndpoint publishEndpoint
            )
        {
            _photographServiceRepository = photographServiceRepository;
            _serviceRepository = serviceRepository;
            _EventOrganizerServiceRepository = EventOrganizerServiceRepository;
            _MakeUpServiceRepository = makeUpServiceRepository;
            _invitationServiceRepository = invitationServiceRepository;
            _unitOfWork = unitOfWork;
            _ClothesServiceRepository = clothesServiceRepository;
            _RestaurantServiceRepository = restaurantServiceRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _elasticService = elasticService;
            _busyScheduleRepository = busyScheduleRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<ServiceDTO>> GetServicesByUserIdAndTypeAsync(int userId, int serviceTypeId)
        {
            try
            {
                List<Service> services = await _serviceRepository.GetsAsyncByUserIdAndType(userId, serviceTypeId);
                return await ConvertListServiceDTO(services, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<ServiceDTO>> GetAllServicesAsync(int userId)
        {
            try
            {
                List<Service> services = await _serviceRepository.GetsAsync();
                return await ConvertListServiceDTO(services, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<ServiceDTO>> GetServicesByTypeId(int typeId, int userId)
        {
            try
            {
                List<Service> services = await _serviceRepository.GetServicesByType(typeId);
                return await ConvertListServiceDTO(services, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ServiceDTO> GetServiceByIdAsync(int id)
        {
            try
            {
                //return await _elasticService.GetServiceByIdFromElasticAsync(id);
                Service service = await _serviceRepository.GetAsyncById(id);
                return await ConvertServiceDTO(service);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteServiceAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Service service = await _serviceRepository.GetAsyncById(id);
                if (service == null)
                {
                    return false;
                }

                await _serviceRepository.DeleteAsync(id);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateServiceAsync(ServiceDTO serviceDto, ISpecificService specificService, int supplierId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Service service = await CreateBaseService(serviceDto, supplierId);
                await CreateSpecificService(service, specificService);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateServiceAsync<T>(ServiceDTO serviceDto, T specificServiceData, int supplierId) where T : class
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Service service = await CreateBaseService(serviceDto, supplierId);
                await _unitOfWork.CommitAsync();
                await CreateSpecificService(service, specificServiceData);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<Service> CreateBaseService(ServiceDTO serviceDto, int supplierId)
        {
            Service service = new()
            {
                ServiceName = serviceDto.ServiceName,
                SupplierId = supplierId,
                ServiceTypeId = serviceDto.ServiceTypeId,
                StarNumber = 0,
                City = serviceDto.City,
                District = serviceDto.District,
                Ward = serviceDto.Ward,
                Address = serviceDto.Address,
                Description = serviceDto.Description,
                VisitWebsiteLink = serviceDto.VisitWebsiteLink,
                IsActive = 2
            };

            await _serviceRepository.CreateAsync(service);
            return service;
        }

        private async Task CreateSpecificService<T>(Service baseService, T specificServiceData) where T : class
        {
            switch ((ServiceType)baseService.ServiceTypeId)
            {
                case ServiceType.Makeup:
                    MakeUpService makeupService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(makeupService, specificServiceData);
                    await _MakeUpServiceRepository.CreateAsync(makeupService);
                    break;

                case ServiceType.Photograph:
                    PhotographService photographService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(photographService, specificServiceData);
                    await _photographServiceRepository.CreateAsync(photographService);
                    break;

                case ServiceType.Invitation:
                    InvitationService weddingCardService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(weddingCardService, specificServiceData);
                    await _invitationServiceRepository.CreateAsync(weddingCardService);
                    break;

                case ServiceType.EventOrganizer:
                    EventOrganizerService eventOrganizerService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(eventOrganizerService, specificServiceData);
                    await _EventOrganizerServiceRepository.CreateAsync(eventOrganizerService);
                    break;

                case ServiceType.Clothes:
                    ClothesService clothesService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(clothesService, specificServiceData);
                    await _ClothesServiceRepository.CreateAsync(clothesService);
                    break;

                case ServiceType.Restaurant:
                    RestaurantService restaurantService = new()
                    {
                        ServiceId = baseService.ServiceId
                    };
                    MapProperties(restaurantService, specificServiceData);
                    await _RestaurantServiceRepository.CreateAsync(restaurantService);
                    break;

                default:
                    throw new ArgumentException($"Invalid service type: {baseService.ServiceTypeId}");
            }
        }

        private void MapProperties<TDestination, TSource>(TDestination destination, TSource source)
            where TDestination : class
            where TSource : class
        {
            foreach (System.Reflection.PropertyInfo sourceProp in source.GetType().GetProperties())
            {
                System.Reflection.PropertyInfo? destinationProp = destination.GetType().GetProperty(sourceProp.Name);
                if (destinationProp != null && destinationProp.CanWrite)
                {
                    destinationProp.SetValue(destination, sourceProp.GetValue(source));
                }
            }
        }

        public async Task<bool> UpdateServiceAsync(int id, ServiceDTO serviceDto, int supplierId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Service existingService = await _serviceRepository.GetAsyncById(id);
                if (existingService == null || existingService.SupplierId != supplierId)
                {
                    return false;
                }

                await UpdateBaseService(existingService, serviceDto);
                await UpdateSpecificService(existingService, serviceDto);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error updating service: {ex.Message}", ex);
            }
        }

        private async Task UpdateBaseService(Service existingService, ServiceDTO serviceDto)
        {
            existingService.ServiceName = serviceDto.ServiceName;
            existingService.Description = serviceDto.Description;
            existingService.VisitWebsiteLink = serviceDto.VisitWebsiteLink;
            existingService.City = serviceDto.City;
            existingService.District = serviceDto.District;
            existingService.Address = serviceDto.Address;

            await _serviceRepository.UpdateAsync(existingService.ServiceId, existingService);
        }

        private async Task UpdateSpecificService(Service existingService, ServiceDTO serviceDto)
        {
            switch ((ServiceType)existingService.ServiceTypeId)
            {
                case ServiceType.Makeup:
                    await _MakeUpServiceRepository.UpdateAsync(existingService.ServiceId, new MakeUpService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for MakeUpService from serviceDto
                    });
                    break;

                case ServiceType.Photograph:
                    await _photographServiceRepository.UpdateAsync(existingService.ServiceId, new PhotographService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for PhotographService from serviceDto
                    });
                    break;

                case ServiceType.Invitation:
                    await _invitationServiceRepository.UpdateAsync(existingService.ServiceId, new InvitationService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for InvitationService from serviceDto
                    });
                    break;

                case ServiceType.EventOrganizer:
                    await _EventOrganizerServiceRepository.UpdateAsync(existingService.ServiceId, new EventOrganizerService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for EventOrganizerService from serviceDto
                    });
                    break;

                case ServiceType.Clothes:
                    await _ClothesServiceRepository.UpdateAsync(existingService.ServiceId, new ClothesService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for ClothesService from serviceDto
                    });
                    break;

                case ServiceType.Restaurant:
                    await _RestaurantServiceRepository.UpdateAsync(existingService.ServiceId, new RestaurantService
                    {
                        ServiceId = existingService.ServiceId,
                        // Update specific properties for RestaurantService from serviceDto
                    });
                    break;

                default:
                    throw new ArgumentException($"Invalid service type: {existingService.ServiceTypeId}");
            }
        }

        public async Task<bool> UpdateServiceAsync<T>(int id, ServiceDTO serviceDto, T specificServiceData, int supplierId) where T : class
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Service existingService = await _serviceRepository.GetAsyncById(id);
                if (existingService == null || existingService.SupplierId != supplierId)
                {
                    return false;
                }

                await UpdateBaseService(existingService, serviceDto);
                await UpdateSpecificService(existingService, specificServiceData);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task UpdateSpecificService<T>(Service existingService, T specificServiceData) where T : class
        {
            switch ((ServiceType)existingService.ServiceTypeId)
            {
                case ServiceType.Makeup:
                    MakeUpService? makeupService = await _MakeUpServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (makeupService == null)
                    {
                        makeupService = new MakeUpService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(makeupService, specificServiceData);
                    await _MakeUpServiceRepository.UpdateAsync(existingService.ServiceId, makeupService);
                    break;

                case ServiceType.Photograph:
                    PhotographService photographService = await _photographServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (photographService == null)
                    {
                        photographService = new PhotographService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(photographService, specificServiceData);
                    await _photographServiceRepository.UpdateAsync(existingService.ServiceId, photographService);
                    break;

                case ServiceType.Invitation:
                    InvitationService weddingCardService = await _invitationServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (weddingCardService == null)
                    {
                        weddingCardService = new InvitationService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(weddingCardService, specificServiceData);
                    await _invitationServiceRepository.UpdateAsync(existingService.ServiceId, weddingCardService);
                    break;

                case ServiceType.EventOrganizer:
                    EventOrganizerService eventOrganizerService = await _EventOrganizerServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (eventOrganizerService == null)
                    {
                        eventOrganizerService = new EventOrganizerService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(eventOrganizerService, specificServiceData);
                    await _EventOrganizerServiceRepository.UpdateAsync(existingService.ServiceId, eventOrganizerService);
                    break;

                case ServiceType.Clothes:
                    ClothesService clothesService = await _ClothesServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (clothesService == null)
                    {
                        clothesService = new ClothesService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(clothesService, specificServiceData);
                    await _ClothesServiceRepository.UpdateAsync(existingService.ServiceId, clothesService);
                    break;

                case ServiceType.Restaurant:
                    RestaurantService restaurantService = await _RestaurantServiceRepository.GetAsyncById(existingService.ServiceId);
                    if (restaurantService == null)
                    {
                        restaurantService = new RestaurantService { ServiceId = existingService.ServiceId };
                    }
                    MapProperties(restaurantService, specificServiceData);
                    await _RestaurantServiceRepository.UpdateAsync(existingService.ServiceId, restaurantService);
                    break;

                default:
                    throw new ArgumentException($"Invalid service type: {existingService.ServiceTypeId}");
            }
        }

        public async Task<List<Service>> SearchServicesAsync(string keyword, int? serviceTypeId, string city)
        {
            try
            {
                return await _serviceRepository.SearchServices(keyword, serviceTypeId, city);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetPopularServicesAsync(int count)
        {
            try
            {
                return await _serviceRepository.GetPopularServices(count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<object> GetServiceStatisticsAsync()
        {
            try
            {
                return await _serviceRepository.GetServiceStatistics();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<object> GetStatisticsBySupplierIdAsync(int supplierId)
        {
            try
            {
                return await _serviceRepository.GetStatisticsBySupplierId(supplierId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetRelatedServicesAsync(int serviceId, int count)
        {
            try
            {
                return await _serviceRepository.GetRelatedServices(serviceId, count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetServicesBySupplierAsync(int supplierId)
        {
            try
            {
                return await _serviceRepository.GetServicesBySupplier(supplierId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Service>> GetPendingApprovalServicesAsync()
        {
            try
            {
                return await _serviceRepository.GetPendingApprovalServices();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ServiceDTO> AcceptServiceAsync(int serviceId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Service? service = await _serviceRepository.AcceptService(serviceId);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return await ConvertServiceDTO(service);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ServiceDTO> RejectServiceAsync(int userId, int serviceId, string reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Service service = await _serviceRepository.RejectService(serviceId);
                User user = await _userRepository.GetAsyncById(userId);
                if (user != null)
                {
                    string subject = "Service Creation Rejection Notice";
                    string content = $"Dear {user.FullName},<br><br>" +
                $"We regret to inform you that your service creation request (Service ID: {serviceId}) has been rejected for the following reason:<br><br>" +
                $"<strong>{reason}</strong><br><br>" +
                "If you have any questions or require further clarification, please feel free to contact our support team.<br><br>" +
                "Best regards,<br>" +
                "Wedding Wonder Team";

                    var emailEvent = new EmailNotificationEvent
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = subject,
                        Body = content,
                        TimeStamp = DateTimeOffset.Now
                    };

                    await _publishEndpoint.Publish(emailEvent);
                }

                await _elasticService.RemoveAsync(service.ServiceId);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return await ConvertServiceDTO(service);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error rejecting service: {ex.Message}", ex);
            }
        }

        private async Task<List<ServiceDTO>> ConvertListServiceDTO(List<Service> services, int userId)
        {
            List<ServiceDTO> serviceDTOs = new();
            var userTopics = await _userRepository.GetServiceTopicsByServiceId(userId);
            foreach (Service s in services)
            {
                ServiceImage images = await _imageRepository.GetFirstImageOfService(s.ServiceId);
                string avatarImage = images?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";
                var supplier = await _userRepository.GetAsyncById(s.SupplierId);
                var serviceTopics = await _userRepository.GetServiceTopicsByServiceId(s.ServiceId);
                int matchingTopicsCount = serviceTopics.Intersect(userTopics).Count();
                serviceDTOs.Add(new ServiceDTO
                {
                    ServiceId = s.ServiceId,
                    SupplierId = s.SupplierId,
                    ServiceTypeId = s.ServiceTypeId,
                    ServiceName = s.ServiceName,
                    Description = s.Description,
                    City = s.City,
                    District = s.District,
                    Ward = s.Ward,
                    Address = s.Address,
                    StarNumber = s.StarNumber,
                    AvatarImage = avatarImage,
                    VisitWebsiteLink = s.VisitWebsiteLink,
                    IsActive = s.IsActive,
                    IsVipSupplier = supplier?.IsVipSupplier ?? false,
                    serviceTopics = serviceTopics.ToList(),
                    MatchingTopicsCount = matchingTopicsCount
                });
            }
            // Sắp xếp serviceDTOs theo ưu tiên
            return serviceDTOs
                .OrderByDescending(s => s.IsVipSupplier && s.MatchingTopicsCount > 0) // VIP và có topic trùng khớp
                .ThenByDescending(s => s.IsVipSupplier) // VIP nhưng không có topic trùng khớp
                .ThenByDescending(s => s.MatchingTopicsCount) // Không VIP nhưng có topic trùng khớp
                .ThenBy(s => s.ServiceId) // Các dịch vụ còn lại
                .ToList();
        }

        private async Task<ServiceDTO> ConvertServiceDTO(Service service)
        {
            ServiceImage image = await _imageRepository.GetFirstImageOfService(service.ServiceId);
            string avatarImage = image?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";

            ServiceDTO serviceDTO = new()
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
                AvatarImage = avatarImage,
                VisitWebsiteLink = service.VisitWebsiteLink,
                IsActive = service.IsActive
            };

            List<ServiceImage> images = await _imageRepository.GetImagesByServiceId(service.ServiceId);
            serviceDTO.AllImage = images.Select(i => i.ImageText).ToList();

            return serviceDTO;
        }

        public async Task<bool> AddServiceTopicsAsync(int serviceId, List<int> topicIds)
        {
            try
            {
                return await _serviceRepository.AddServiceTopics(serviceId, topicIds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<ServiceDTO>> FilterServicesAsync(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers, int userId)
        {
            try
            {
                List<Service> services = await _serviceRepository.FilterServices(serviceTypeIds, city, freeScheduleDate, starNumbers);
                return await ConvertListServiceDTO(services, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> SyncToElasticsearchAsync()
        {
            try
            {
                List<Service> services = await _serviceRepository.GetsAsync();
                foreach (var service in services)
                {
                    var serviceDto = await this.MapToDTOAsync(service);
                    var response = await _elasticService.AddOrUpdateAsync(serviceDto);
                    if (!response)
                    {
                        throw new Exception($"Failed to index service with ID {service.ServiceId}");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error syncing to Elasticsearch: {ex.Message}", ex);
            }
        }
        public async Task<ServiceDTO> MapToDTOAsync(Service service)
        {
            ServiceImage image = await _imageRepository.GetFirstImageOfService(service.ServiceId);
            string avatarImage = image?.ImageText ?? "https://careplusvn.com/Uploads/t/de/default-image_730.jpg";

            List<ServiceImage> allImages = await _imageRepository.GetImagesByServiceId(service.ServiceId);
            List<string> allImageUrls = allImages.Select(img => img.ImageText).ToList();

            List<int> serviceTopics = (List<int>)await _userRepository.GetServiceTopicsByServiceId(service.ServiceId);

            var busySchedules = await _busyScheduleRepository.GetByServiceIdAsync(service.ServiceId);
            var busySchedulesData = busySchedules.Select(bs => new BusyScheduleDTO
            {
                ScheduleId = bs.ScheduleId,
                ServiceId = bs.ServiceId,
                StartDate = bs.StartDate,
                EndDate = bs.EndDate,
                Content = bs.Content
            }).ToList();

            var serviceDto = new ServiceDTO
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
                AvatarImage = avatarImage,
                BusySchedules = busySchedulesData,
                AllImage = allImageUrls,
                IsVipSupplier = service.Supplier?.IsVipSupplier ?? false,
            };
            return serviceDto;
        }
    }
}

public enum ServiceType
{
    Makeup = 1,
    Clothes = 2,
    Restaurant = 3,
    Photograph = 4,
    Invitation = 5,
    EventOrganizer = 6
}