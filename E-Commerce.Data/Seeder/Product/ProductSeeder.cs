namespace E_Commerce.Data
{
    public class ProductSeeder : ISeeder
    {
        public async Task SeedAsync(ECommerceDbContext dbContext, IServiceProvider service)
        {
            if (dbContext.Products.Any()) return;

            var products = new List<Product>
            {
                new Product() {Name = "Asus Tuf A16", Description = "Asus Tuf A16 Gaming Laptop, AMD Ryzen 7 260w, 16GB RAM, 512GB SSD, NVIDIA GeForce RTX 5050, 15.6” FHD 165Hz Display, Windows 11 Home, Eclipse Gray", Price = 55000m, Count = 10, CategoryId = 1},
                new Product() {Name = "PS5", Description = "Sony PlayStation 5 Console, 825GB SSD, Ultra-High-Speed SSD, Ray Tracing, 3D Audio, Backward Compatibility, DualSense Wireless Controller, White", Price = 50000m, Count = 15, CategoryId = 1},
                new Product() {Name = "CocaCola", Description = "Coca-Cola Classic, 12 fl oz (Pack of 12)", Price = 20m, Count = 100, CategoryId = 2},
                new Product() {Name = "Almrai Milk", Description = "Almarai Full Cream Milk, 1 Liter (Pack of 4)", Price = 15m, Count = 50, CategoryId = 2},
                new Product() {Name = "Tank Top", Description= "White Tank Top For Men", Price = 300m, Count = 30, CategoryId = 3},
                new Product() {Name = "Jeans", Description = "Blue Jeans", Price = 500m, Count = 20, CategoryId = 3},
                new Product() {Name = "The Prince", Description = "The Prince is a political treatise by the Italian diplomat and political theorist Niccolò Machiavelli. It was written in 1513 and published posthumously in 1532. The book is a guide for rulers on how to maintain power and control over their states.", Price = 500m, Count = 100, CategoryId = 4},
                new Product() {Name = "The Great Gatsby", Description = "The Great Gatsby is a novel by American author F. Scott Fitzgerald. It was published in 1925 and is set in the Jazz Age on Long Island, near New York City. The story primarily concerns the young and mysterious millionaire Jay Gatsby and his quixotic passion for the beautiful Daisy Buchanan.", Price = 400m, Count = 80, CategoryId = 4},
                new Product() {Name = "It Takes Two", Description = "It Takes Two is a cooperative action-adventure game developed by Hazelight Studios and published by Electronic Arts. The game was released in 2021 for Microsoft Windows, PlayStation 4, PlayStation 5, Xbox One, and Xbox Series X/S. It Takes Two is designed to be played in split-screen co-op mode, either locally or online.", Price = 200m, Count = 50, CategoryId = 5},
                new Product() {Name = "FIFA 23", Description = "FIFA 23 is a football simulation video game developed by EA Vancouver and EA Romania, and published by Electronic Arts. It is the 30th installment in the FIFA series and was released on September 30, 2022, for Microsoft Windows, PlayStation 4, PlayStation 5, Xbox One, Xbox Series X/S, and Stadia.", Price = 250m, Count = 60, CategoryId = 5},
            };

            dbContext.Products.AddRange(products);
            await dbContext.SaveChangesAsync();
        }
    }
}
