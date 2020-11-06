using System;
using System.Threading.Tasks;

namespace Basket.OrientedObject
{
    public class ArticleRepositoryMock : IArticleRepository
    {
    public ArticleRepositoryMock()
    {
    }

    public Task<ArticleDatabase> GetArticleDatabaseAsync(string id)
    {
        switch (id)
        {
            case "1":
                return Task.FromResult(new ArticleDatabase
                    {Id = "1", Price = 1, Stock = 35, Label = "Banana", Category = "food"});
            case "2":
                return Task.FromResult(new ArticleDatabase
                {
                    Id = "2",
                    Price = 500,
                    Stock = 20,
                    Label = "Fridge electrolux",
                    Category = "electronic"
                });
            case "3":
                return Task.FromResult(new ArticleDatabase
                    {Id = "3", Price = 49, Stock = 68, Label = "Chair", Category = "desktop"});
            default:
                throw new NotImplementedException();
        }
    }
    }
}