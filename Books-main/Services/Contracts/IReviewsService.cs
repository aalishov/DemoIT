using Books.ViewModels.Reviews;

namespace Books.Services.Contracts
{
    public interface IReviewsService
    {
        public Task<IndexReviewsUserViewModel> GetAllReviewsAsync(IndexReviewsUserViewModel model);

        public Task SeedReviewsAsync();

        public Task<string> CreateReviewAsync(CreateReviewViewModel model, string userId);

        public Task<IndexReviewsUserViewModel> GetUserReviewsAsync(IndexReviewsUserViewModel model, string userId);
    }
}
