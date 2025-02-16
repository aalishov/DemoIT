using Books.ViewModels.Book;

namespace Books.ViewModels.Books
{
    public class DetailsBookViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string AverageRating { get; set; }

        public ICollection<ReviewBooksViewModel> Reviews { get; set; } = new HashSet<ReviewBooksViewModel>();
    }
}
