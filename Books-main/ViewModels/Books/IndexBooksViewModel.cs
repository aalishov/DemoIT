using Books.ViewModels.Users;

namespace Books.ViewModels.Book
{
    public class IndexBooksViewModel : PagingViewModel
    {
        public IndexBooksViewModel() : base(10)
        {

        }
        public IndexBooksViewModel(int elementsCount, int itemsPerPage = 5, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }

        public string FilterByName { get; set; }

        public bool IsAsc { get; set; } = true;

        public ICollection<IndexBookViewModel> Bars { get; set; } = new List<IndexBookViewModel>();
        public List<IndexBookViewModel> Book { get; internal set; }
    }
}
