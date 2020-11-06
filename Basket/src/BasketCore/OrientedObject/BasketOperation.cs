using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.OrientedObject
{
    public class BasketOperation
    {
        private readonly IArticleRepository _articleRepository;

        public BasketOperation(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }
        
        public async Task<double> GetAmountTotal(IList<BasketLineArticle> basketLineArticles)
        {
            var amountTotal = 0D;
            foreach (var basketLineArticle in basketLineArticles)
            {
                var id = basketLineArticle.Id;
                var amount = 0;
                ArticleDatabase article;
                try
                {
                    article = await _articleRepository.GetArticleDatabaseAsync(id);
                }
                catch(Exception ex)
                {
                    // TODO logger
                    article =new ArticleDatabase()
                    {
                        Category = "food",
                        Price = 20
                    };
                }
                var price = article.Price;
                switch (article.Category)
                {
                    case "food":
                        amount += price * 100 + price * 12;
                        break;
                    case "electronic":
                        amount += price * 100 + price * 20 + 4;
                        break;
                    case "desktop":
                        amount += price * 100 + price * 20;
                        break;
                }
              

                amountTotal += amount * basketLineArticle.Number;
            }

            return amountTotal;
        }
    }
}