
namespace DomainLayer.Responses
{
    public class RecipecResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseImage { get; set; }
        public List<string> Ingredient { get; set; }
        public string Steps { get; set; }
        public int CookTime { get; set; }
        public string CategoryName { get; set; }
        public string ChefName { get; set; }
        public List<ImageResponse> Images { get; set; }
        public float AverageRating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
