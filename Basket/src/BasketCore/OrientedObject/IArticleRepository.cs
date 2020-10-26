using System.Threading.Tasks;

namespace Basket.OrientedObject
{
    public interface IArticleRepository
    {
        Task<ArticleDatabase> GetArticleDatabaseAsync(string id);
    }
}