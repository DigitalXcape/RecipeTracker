namespace RecipeAPI.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? TimeToMake { get; set; }

        public string? ImageURl { get; set; }
    }
}
