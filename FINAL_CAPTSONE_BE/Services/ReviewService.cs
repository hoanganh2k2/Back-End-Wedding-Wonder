using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;

namespace Services
{
    public class ReviewService
    {
        private readonly ICustomerReviewRepository _reviewRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IServiceImageRepository _serviceImageRepositor;
        

        public ReviewService(
            IUnitOfWork unitOfWork,
            ICustomerReviewRepository reviewRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            IServiceImageRepository serviceImageRepositor
            )
        {
            _unitOfWork = unitOfWork;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _serviceImageRepositor = serviceImageRepositor;
            _serviceRepository = serviceRepository;
        }

        public async Task<List<CustomerReviewDTO>> GetAllReviews()
        {
            try
            {
                var reviews = await _reviewRepository.GetsAsync();
               List<CustomerReviewDTO> reviewDTOs = new(); 

                 foreach (var r in reviews)
                {
                    var user = await _userRepository.GetAsyncById(r.UserId);
                    var service = await _serviceRepository.GetAsyncById(r.ServiceId);
                    var avatarImage = await _serviceImageRepositor.GetFirstImageOfService(service.ServiceId); 
                    reviewDTOs.Add(new CustomerReviewDTO
                    {
                        ReviewId = r.ReviewId,
                        Content = r.Content,
                        CreatedAt = r.CreatedAt,
                        Reply = r.Reply,
                        StarNumber = r.StarNumber,
                        UserId = r.UserId,
                        ServiceId = r.ServiceId,
                        CanEdit = r.CanEdit,
                        FullName = user?.UserName, 
                        UserImage = user?.UserImage, 
                        ServiceName = service?.ServiceName, 
                        ServiceImage = avatarImage.ImageText 
                    });
                }


                return reviewDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<CustomerReviewDTO> GetReviewById(int reviewId)
        {
            try
            {
                CustomerReview review = await _reviewRepository.GetAsyncById(reviewId);
                var user = await _userRepository.GetAsyncById(review.UserId);
                var service = await _serviceRepository.GetAsyncById(review.ServiceId);
                CustomerReviewDTO reviewDTO = new()
               {
            ReviewId = review.ReviewId,
            Content = review.Content,
            CreatedAt = review.CreatedAt,
            Reply = review.Reply,
            StarNumber = review.StarNumber,
            UserId = review.UserId,
            ServiceId = review.ServiceId,
            CanEdit = review.CanEdit,
            FullName = user.UserName,
            UserImage = user.UserImage,
            ServiceName = service.ServiceName
               };


                return reviewDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReviewDTO>> GetReviewsOfStore(int id)
        {
            try
            {
                List<CustomerReview> reviews = await _reviewRepository.GetReviewsByServiceId(id);
                List<CustomerReviewDTO> reviewDTOs = new();
                foreach (var r in reviews)
        {
            var user = await _userRepository.GetAsyncById(r.UserId);
             var service = await _serviceRepository.GetAsyncById(r.ServiceId);
               reviewDTOs.Add(new CustomerReviewDTO
            {
                ReviewId = r.ReviewId,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                Reply = r.Reply,
                StarNumber = r.StarNumber,
                UserId = r.UserId,
                ServiceId = r.ServiceId,
                CanEdit = r.CanEdit,
                FullName = user.UserName,
                UserImage = user.UserImage,
                ServiceName = service.ServiceName
            });
        }

                return reviewDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReviewDTO>> GetReviewsForSupplier(int supplierId)
        {
            try
            {
               List<CustomerReview> reviews = await _reviewRepository.GetReviewsBySupplierId(supplierId);
                List<CustomerReviewDTO> reviewDTOs = new();
                foreach (var r in reviews)
        {
            var user = await _userRepository.GetAsyncById(r.UserId);
             var service = await _serviceRepository.GetAsyncById(r.ServiceId);
               reviewDTOs.Add(new CustomerReviewDTO
            {
                ReviewId = r.ReviewId,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                Reply = r.Reply,
                StarNumber = r.StarNumber,
                UserId = r.UserId,
                ServiceId = r.ServiceId,
                CanEdit = r.CanEdit,
                FullName = user.UserName,
                UserImage = user.UserImage,
                ServiceName = service.ServiceName
            });
        }

                return reviewDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReviewDTO>> GetHistory(int userId)
        {
            try
            {
                List<CustomerReview> reviews = await _reviewRepository.GetHistoryReviews(userId);
                List<CustomerReviewDTO> reviewDTOs = new();
                foreach (var r in reviews)
        {
            var user = await _userRepository.GetAsyncById(r.UserId);
             var service = await _serviceRepository.GetAsyncById(r.ServiceId);
               reviewDTOs.Add(new CustomerReviewDTO
            {
                ReviewId = r.ReviewId,
                Content = r.Content,
                CreatedAt = r.CreatedAt,
                Reply = r.Reply,
                StarNumber = r.StarNumber,
                UserId = r.UserId,
                ServiceId = r.ServiceId,
                CanEdit = r.CanEdit,
                FullName = user.UserName,
                UserImage = user.UserImage,
                ServiceName = service.ServiceName
            });
        }


                return reviewDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateReview(CustomerReviewDTO reviewDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                CustomerReview review = new()
                {
                    Content = reviewDTO.Content,
                    CreatedAt = DateTime.Now,
                    StarNumber = reviewDTO.StarNumber ?? 0,
                    UserId = reviewDTO.UserId ?? 0,
                    ServiceId = reviewDTO.ServiceId ?? 0,
                };

                bool status = await _reviewRepository.CreateAsync(review);
                if (status) await UpdateStar(review.ServiceId, review.StarNumber, 1);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return status;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UserDeleteReview(int id, int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                CustomerReview review = await _reviewRepository.GetAsyncById(id);
                if (review.UserId != userId)
                    throw new UnauthorizedAccessException("You are not authorized to delete this review.");

                await _reviewRepository.DeleteAsync(id);
                await UpdateStar(review.ServiceId, -review.StarNumber, -1);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AdminDeleteReview(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                CustomerReview review = await _reviewRepository.GetAsyncById(id);

                await _reviewRepository.DeleteAsync(id);
                await UpdateStar(review.ServiceId, -review.StarNumber, -1);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateReview(int id, CustomerReviewDTO reviewDTO, int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                CustomerReview review = await _reviewRepository.GetAsyncById(id);

                if (review.UserId != userId)
                    throw new UnauthorizedAccessException("You are not authorized to edit this review.");

                if (!review.CanEdit)
                    throw new Exception("This review cannot be edited. Each review can only be edited once.");

                int oldStar = review.StarNumber;
                review.Content = reviewDTO.Content;
                review.StarNumber = reviewDTO.StarNumber ?? 0;

                await _reviewRepository.UpdateAsync(id, review);

                await UpdateStar(review.ServiceId, review.StarNumber - oldStar, 0);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task Reply(int id, string reply, int supplierId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                CustomerReview review = await _reviewRepository.GetAsyncById(id);
                Service service = await _serviceRepository.GetAsyncById(review.ServiceId);

                if (service.SupplierId != supplierId)
                    throw new UnauthorizedAccessException("You are not authorized to respond this review.");

                review.Reply = reply;

                await _reviewRepository.UpdateAsync(id, review);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task UpdateStar(int serviceId, int newStar, int weight)
        {
            await _serviceRepository.UpdateStar(serviceId,
                await _reviewRepository.GetNewAverage(serviceId, newStar, weight));
        }
    }
}
