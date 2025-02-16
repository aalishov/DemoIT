using Books.Data;
using Books.Data.Models;
using Books.Services.Contracts;
using Books.ViewModels.Book;
using Books.ViewModels.Books;
using Microsoft.EntityFrameworkCore;

namespace Books.Services
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext context;

        public BookService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IndexBookViewModel> GetWorstBookAsync()
        {
            Book? bestBook = context.Books
                .Where(x => x.Reviews.Any())
                .OrderBy(x => x.Reviews.Average(x => x.Rating))
                .FirstOrDefault();
            if (bestBook != null)
            {
                return new IndexBookViewModel()
                {
                    Name = bestBook.Name,
                    AverageRating = Math.Round(bestBook.Reviews.Average(x => x.Rating),2).ToString(),
                    Image = bestBook.Image,
                };
            }
            return null;
        }

        public async Task<IndexBookViewModel> GetBestBookAsync()
        {
            Book? bestBook = context.Books
                .Where(x => x.Reviews.Any())
                .OrderBy(x => x.Reviews
                .Average(x => x.Rating)).LastOrDefault();
            if (bestBook != null)
            {
                return new IndexBookViewModel()
                {
                    Name = bestBook.Name,
                    AverageRating = Math.Round(bestBook.Reviews.Average(x => x.Rating),2).ToString(),
                    Image = bestBook.Image,
                };
            }
            return null;
        }

        public async Task DeleteBookByIdAsync(string BookId)
        {
            Book? Book = await context.Books.FindAsync(BookId);
            context.Books.Remove(Book);
            await context.SaveChangesAsync();
        }

        public async Task<DeleteBookViewModel> GetBookToDeleteByIdAsync(string BookId)
        {
            Book? Book = await context.Books.FindAsync(BookId);

            if (Book == null) { return null; }

            DeleteBookViewModel viewModel = new DeleteBookViewModel()
            {
                Id = BookId,
                Name = Book.Name,
                AverageRating = Book.Reviews.Any() ? Book.Reviews.Average(x => x.Rating).ToString() : "n/a"
            };
            return viewModel;
        }
        public async Task<DetailsBookViewModel> GetBookDetailsByIdAsync(string BookId)
        {
            Book? Book = await context.Books.FindAsync(BookId);

            if (Book == null) { return null; }

            DetailsBookViewModel viewModel = new DetailsBookViewModel()
            {
                Id = BookId,
                Name = Book.Name,
                Description = Book.Description,
                Image = Book.Image,
                AverageRating = Book.Reviews.Any() ? Book.Reviews.Average(x => x.Rating).ToString() : "n/a",
                Reviews = Book.Reviews.Any() ? Book.Reviews.Select(x => new ReviewBooksViewModel() { UserLasName = x.User.LastName, Rating = x.Rating, ReviewText = x.ReviewText }).ToList() : null,
            };

            return viewModel;
        }

        public async Task<string> UpdateBookAsync(EditBookViewModel model)
        {
            Book? Book = await context
                .Books
                .FindAsync(model.Id);

            Book.Name = model.Name;
            Book.Description = model.Description;
            Book.Image = await ImageToStringAsync(model.ImageFile);

            context.Books.Update(Book);
            await context.SaveChangesAsync();
            return Book.Id;
        }

        public async Task<EditBookViewModel> GetBookToEditAsync(string BookId)
        {
            Book? Book = await context
                .Books
                .FindAsync(BookId);

            return new EditBookViewModel()
            {
                Id = BookId,
                Name = Book.Name,
                Description = Book.Description,
            };
        }

        public async Task<string> CreateBookAsync(CreateBookViewModel model)
        {
            Book Book = new Book()
            {
                Name = model.Name,
                Description = model.Description,
                Image = await ImageToStringAsync(model.ImageFile),
            };
            await context.Books.AddAsync(Book);
            await context.SaveChangesAsync();

            return Book.Id;
        }

        public async Task<IndexBooksViewModel> GetBooksAsync(IndexBooksViewModel model)
        {
            if (model == null)
            {
                model = new IndexBooksViewModel(10);
            }
            IQueryable<Book> dataBooks = context.Books;

            if (!string.IsNullOrWhiteSpace(model.FilterByName))
            {
                dataBooks = dataBooks.Where(x => x.Name.Contains(model.FilterByName));
            }

            if (model.IsAsc)
            {
                model.IsAsc = false;
                dataBooks = dataBooks.OrderByDescending(x => x.Name);
            }
            else
            {
                model.IsAsc = true;
                dataBooks = dataBooks.OrderBy(x => x.Name);
            }

            model.ElementsCount = await dataBooks.CountAsync();

            model.Book = await dataBooks
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .Select(x => new IndexBookViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                })
                .ToListAsync();

            return model;
        }



        private async Task<string> ImageToStringAsync(IFormFile file)
        {
            List<string> imageExtensions = new List<string>() { ".JPG", ".BMP", ".PNG" };


            if (file != null) // check if the user uploded something
            {
                var extension = Path.GetExtension(file.FileName); //get file extension
                if (imageExtensions.Contains(extension.ToUpperInvariant()))
                {
                    using var dataStream = new MemoryStream();
                    await file.CopyToAsync(dataStream);
                    byte[] imageBytes = dataStream.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            return null;
        }

        Task<IndexBooksViewModel> IBookService.GetWorstBookAsync()
        {
            throw new NotImplementedException();
        }

        Task<IndexBookViewModel> IBookService.GetBestBookAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IndexBooksViewModel> GetBookAsync(IndexBooksViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IndexBookViewModel> GetBooksAsync(IndexBookViewModel books)
        {
            throw new NotImplementedException();
        }
    }
        
    }
