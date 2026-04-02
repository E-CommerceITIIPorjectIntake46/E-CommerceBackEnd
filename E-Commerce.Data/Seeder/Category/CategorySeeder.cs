namespace E_Commerce.Data
{
    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ECommerceDbContext dbContext, IServiceProvider service)
        {
            if (dbContext.Categories.Any()) return;

            var categories = new List<Category>()
            {
                new Category() { Name = "Electronics" },
                new Category() { Name = "Groceries" },
                new Category() { Name = "Clothing" },
                new Category() { Name = "Books" },
                new Category() { Name = "Video Games" }
            };

            dbContext.Categories.AddRange(categories);
            await dbContext.SaveChangesAsync();
        }
    }
}
