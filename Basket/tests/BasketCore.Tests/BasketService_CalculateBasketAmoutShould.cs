using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Basket;
using Basket.OrientedObject;
using Newtonsoft.Json;
using Xunit;
using BasketOperation = Basket.Imperative.BasketOperation;

namespace BasketCore.Tests
{
    public class BasketService_CalculateBasketAmoutShould
    {
        
        public class BasketTest
        {
            public IList<BasketLineArticle> BasketLineArticles { get; set; }
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
            var amountTotal = await  BasketOperation.GetAmountTotal(basketLineArticles);
            Assert.Equal(basketTest.ExpectedPrice, amountTotal);
        }
        
        [Theory]
        [MemberData(nameof(Baskets))]
        public async Task OrientedObject_ReturnCorrectAmoutGivenBasket(BasketTest basketTest)
        {
            var basketLineArticles = basketTest.BasketLineArticles;
            var basketOperation = new Basket.OrientedObject.BasketOperation(new ArticleRepositoryMock());
            var amountTotal = await  basketOperation.GetAmountTotal(basketLineArticles);
            Assert.Equal(basketTest.ExpectedPrice, amountTotal);
        }
        
        [Theory]
        [MemberData(nameof(Baskets))]
        public async Task Declarative_ReturnCorrectAmoutGivenBasket(BasketTest basketTest)
        {
            var basketLineArticles = basketTest.BasketLineArticles;
            var databaseFunc = BasketCore.Declarative.BasketOperation.RegleMetier(BasketCore.Declarative.BasketOperation
                .GetArticleDatabaseMockAsync);
            var basketOperation =
                BasketCore.Declarative.BasketOperation.GetAmountTotal(databaseFunc);

            var amountTotal = await basketOperation(basketLineArticles);
            Assert.Equal(basketTest.ExpectedPrice, amountTotal);
        }

    }
}