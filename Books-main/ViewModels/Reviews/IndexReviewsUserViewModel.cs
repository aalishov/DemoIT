﻿namespace Books.ViewModels.Reviews
{
    public class IndexReviewsUserViewModel : PagingViewModel
    {
        public IndexReviewsUserViewModel():base(10)
        {
            
        }
        public IndexReviewsUserViewModel(int elementsCount, int itemsPerPage = 10, string action = "Index") : base(elementsCount, itemsPerPage, action)
        {
        }

        public ICollection<IndexReviewViewModel> UserReviews { get; set; } = new HashSet<IndexReviewViewModel>();
    }
}
