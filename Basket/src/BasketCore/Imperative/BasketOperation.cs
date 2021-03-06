﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Basket.Imperative
{
    public class BasketOperation
    {
        public static async Task<double> GetAmountTotal(IList<BasketLineArticle> basketLineArticles)
        {
            var amountTotal = 0D;
            foreach (var basketLineArticle in basketLineArticles)
            {
                var id = basketLineArticle.Id;
#if DEBUG
                var article = await GetArticleDatabaseMockAsync(id);
#endif
#if RELEASE
                var article = await GetArticleDatabaseAsync(id);
#endif
                
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

            return amountTotal;
        }

        private static async Task<ArticleDatabase> GetArticleDatabaseAsync(string id)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var assemblyDirectory = Path.GetDirectoryName(path);
            var jsonPath = Path.Combine(assemblyDirectory, "article-database.json");
            var json = await File.ReadAllTextAsync(jsonPath);
            var articleDatabases =
                JsonConvert.DeserializeObject<List<ArticleDatabase>>(json);
            var article = articleDatabases.First(articleDatabase => articleDatabase.Id == id);
            return article;
        }

        public static Task<ArticleDatabase> GetArticleDatabaseMockAsync(string id)
        {
            switch (id)
            {
                case "1":
                    return Task.FromResult(new ArticleDatabase {Id = "1", Price = 1, Stock = 35, Label = "Banana", Category = "food"});
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
                    return Task.FromResult(new ArticleDatabase {Id = "3", Price = 49, Stock = 68, Label = "Chair", Category = "desktop"});
                default:
                    throw new NotImplementedException();
            }
        }
    }
}