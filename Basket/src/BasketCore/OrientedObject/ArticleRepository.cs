
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Basket.OrientedObject
{
    public class ArticleRepository : IArticleRepository
    {
        public ArticleRepository()
        {
        }

        public async Task<ArticleDatabase> GetArticleDatabaseAsync(string id)
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
    }
}