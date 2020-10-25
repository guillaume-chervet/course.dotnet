using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Basket.Tests
{
    public class BasketService_CalculateBasketAmoutShould
    {
        
        public class BasketTest
        {
            public List<BasketLineArticle> BasketLineArticles { get; set; }
            public int ExpectedPrice { get; set; }
        }
        

        public static IEnumerable<object[]> Baskets
        {
            get
            {
                return new[]
                {   new object[] {
                        new BasketTest(){ BasketLineArticles = new List<BasketLineArticle>
                            {
                                new BasketLineArticle {Id = "1", Number = 12, Label = "Banana"},
                                new BasketLineArticle {Id = "2", Number = 1, Label = "Fridge electrolux"},
                                new BasketLineArticle {Id = "3", Number = 4, Label = "Chair"}
                            },
                            ExpectedPrice = 84868}
                    },
                    new object[] {
                        new BasketTest(){ BasketLineArticles = new List<BasketLineArticle>
                            {
                                new BasketLineArticle {Id = "1", Number = 20, Label = "Banana"},
                                new BasketLineArticle {Id = "3", Number = 6, Label = "Chair"}
                            },
                            ExpectedPrice = 37520}
                    },
                    new object[] {
                        new BasketTest(){ BasketLineArticles = new List<BasketLineArticle>
                            {
                                new BasketLineArticle {Id = "3", Number = 2, Label = "Chair"},
                            },
                            ExpectedPrice = 11760}
                    },
                };
            }
        }
        
        [Theory]
        [MemberData(nameof(Baskets))]
        public async Task Imperative_ReturnCorrectAmoutGivenBasket(BasketTest basketTest)
        {
            var basketLineArticles = basketTest.BasketLineArticles;
            var amountTotal = 0D;
            foreach (var basketLineArticle in basketLineArticles)
            {
                var id = basketLineArticle.Id;
                
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                var assemblyDirectory = Path.GetDirectoryName(path);
                var jsonPath = Path.Combine(assemblyDirectory, "article-database.json");
                var json = await File.ReadAllTextAsync(jsonPath);
                var articleDatabases =
                    JsonConvert.DeserializeObject<List<ArticleDatabase>>(json);
                var article = articleDatabases.First(articleDatabase => articleDatabase.Id == id);

                var amount = 0;
                switch (article.Category)
                {
                    case "food":
                        amount += article.Price * 100 + article.Price * 12;
                        break;
                    case "electronic":
                        amount += article.Price * 100 + article.Price * 20 + 4;
                        break;
                    case "desktop":
                        amount += article.Price * 100 + article.Price * 20;
                        break;
                }
                
                amountTotal += amount * basketLineArticle.Number;
            }
            Assert.Equal(basketTest.ExpectedPrice, amountTotal);
        }

        /*public static ArticleDatabase GetArticleDatabaseMock(string id)
        {
            switch (id)
            {
                case "1":
                    return new ArticleDatabase {Id = "1", Price = 1, Stock = 35, Label = "Banana", Category = "food"};
                case "2":
                    return new ArticleDatabase
                    {
                        Id = "2",
                        Price = 500,
                        Stock = 20,
                        Label = "Fridge electrolux",
                        Category = "electronic"
                    };
                case "3":
                    return new ArticleDatabase {Id = "3", Price = 49, Stock = 68, Label = "Chair", Category = "desktop"};
                default:
                    throw new NotImplementedException();
            }
        }*/
    }
}