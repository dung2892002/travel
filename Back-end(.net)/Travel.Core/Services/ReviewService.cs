using Microsoft.AspNetCore.Http;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Core.Services
{
    public class ReviewService(IUnitOfWork unitOfWork, IFirebaseStorageService firebaseStorageService) : IReviewService, IService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService = firebaseStorageService;
        public async Task Create(Review review, List<IFormFile> files)
        {
            review.CreatedAt = DateTime.Now;
            await _unitOfWork.Reviews.Create(review);
            if (files != null)
            {
                var filePaths = new List<string>();
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var path = DateTime.Now.ToString("dd-MM-yyyy") + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
                        using Stream stream = file.OpenReadStream();
                        var result = await _firebaseStorageService.UploadFile(stream, path);
                        if (!result) throw new Exception("Upload image error");

                        var image = new Image
                        {
                            Path = path,
                            ReviewId = review.Id,
                        };

                        await _unitOfWork.Images.AddImage(image);
                    }
                }
            }

            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> Delete(int id, Guid userId)
        {
            var review = await _unitOfWork.Reviews.GetById(id);
            if (review == null)
            {
               return false;
            }

            if (review.UserId != userId)
            {
                throw new UnauthorizedAccessException("Only can delete review of you");
            }

            foreach (var image in review.Image)
            {
                await _firebaseStorageService.Delete(image.Path);
            }

            await _unitOfWork.Reviews.Delete(review);
            var result = await _unitOfWork.CompleteAsync(); 
            return result > 0;
        }

        public async Task<IEnumerable<Review>> GetByDestination(int destinationId)
        {
            var reviews = await _unitOfWork.Reviews.GetByDestination(destinationId);

            return reviews;
        }

        public async Task<PagedResult<Review>> GetByHotel(Guid hoteId, int pageNumber)
        {
            var hotel = await _unitOfWork.Hotels.GetById(hoteId);
            if (hotel == null)
            {
                throw new ArgumentException("hotel not exist");
            }

            return await _unitOfWork.Reviews.GetByHotel(hoteId, pageNumber);
        }

        public async Task<PagedResult<Review>> GetByTour(Guid tourId, int pageNumber)
        {
            var tour = await _unitOfWork.Tours.GetById(tourId);
            if(tour == null)
            {
                throw new ArgumentException("tour not exist");
            }    
            return await _unitOfWork.Reviews.GetByTour(tourId, pageNumber);
        }
    }
}
