using System.ComponentModel.DataAnnotations;

namespace Books.ViewModels.Book
{
    public class IndexBookViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AverageRating { get; set; }


        [Display(Name = "Picture")]
        public string Image { get; set; }
    }
}
