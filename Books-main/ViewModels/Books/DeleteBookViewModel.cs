namespace Books.Services.Contracts
{
    public class DeleteBookViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string AverageRating { get; set; }
        public object AverageBook { get; internal set; }
    }
}