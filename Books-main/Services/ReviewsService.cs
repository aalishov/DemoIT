using Books.Data;
using Books.Data.Models;
using Books.Services.Contracts;
using Books.ViewModels.Books;
using Books.ViewModels.Reviews;
using Microsoft.EntityFrameworkCore;
using System;

namespace Books.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly ApplicationDbContext context;

        public object Reviews { get; private set; }

        public ReviewsService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<string> CreateReviewAsync(CreateReviewViewModel model, string userId)
        {
            Review review = new Review()
            {
                Rating = Math.Round(model.Rating,2),
                UserId = userId,
                ReviewText = model.ReviewText,
                BookId = model.BookId
            };

            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();

            return review.Id;
        }

        public async Task<IndexReviewsUserViewModel> GetAllReviewsAsync(IndexReviewsUserViewModel model)
        {
            if (model == null)
            {
                model = new IndexReviewsUserViewModel(10);
            }
            IQueryable<Review> reviewsData = context.Reviews;

            model.ElementsCount = await reviewsData.CountAsync();

            model.UserReviews = await reviewsData
              .Skip((model.Page - 1) * model.ItemsPerPage)
              .Take(model.ItemsPerPage)
              .Select(x => new IndexReviewViewModel()
              {
                  ReviewId = x.Id,
                  BookName = x.Book.Name,
                  Book = Math.Round(x.Rating,2),
                  ReviewText = x.ReviewText
              })
              .ToListAsync();

            return model;
        }

        public async Task<IndexReviewsUserViewModel> GetUserReviewsAsync(IndexReviewsUserViewModel model, string userId)
        {
            if (model == null)
            {
                model = new IndexReviewsUserViewModel(10);
            }
            IQueryable<Review> reviewsData = context.Reviews.Where(x => x.UserId == userId);

            model.ElementsCount = await reviewsData.CountAsync();

            model.UserReviews = await reviewsData
              .Skip((model.Page - 1) * model.ItemsPerPage)
              .Take(model.ItemsPerPage)
              .Select(x => new IndexReviewViewModel()
              {
                  ReviewId = x.Id,
                  BookName = x.Book.Name,
                  UserId = userId,
                  Book= Math.Round(x.Rating, 2),
                  ReviewText = x.ReviewText
              })
              .ToListAsync();

            return model;
        }

        public async Task SeedReviewsAsync()
       {
          
            List<Book> bars = context.Books.Where(x => !x.Reviews.Any()).ToList();
            foreach (var bar in bars)
            {
                for (int i = 0; i < 3; i++)
                {
                    double rating = GenerateRandomNumber();
                    User user = context.Users.FirstOrDefault();
                    bar.Reviews.Add(new Review()
                    {
                        Rating = rating,
                        ReviewText = $"Seed review {i} - {rating}",
                        User = user
                    });
                }
            }

            context.Books.UpdateRange(bars);
            await context.SaveChangesAsync();

        }

        public double GenerateRandomNumber(double minValue = 0.0, double maxValue = 10.0)
        {
            Random random = new Random();
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

       
    }
}
