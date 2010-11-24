using System;
using uNhAddIns.Example.MonoRailConversationUsage.Entities;

namespace uNhAddIns.Example.MonoRailConversationUsage.Utils {
    class Sampler  : ISampler {
        public Category CreateSampleCategory() {
            var category = new Category { CreatedOn = DateTime.Now, Name = NameHelper.GetRandomNameWithLength(8) };
            AddSampleProducts(category);
            return category;
        }

        static void AddSampleProducts(Category category) {
            for (int i = 0; i < 5; i++) {
                var product = new Product {
                                              Name =
                                                  string.Format("Product {0} {1} category",
                                                                NameHelper.GetRandomNameWithLength(5), category.Name),
                                              CreatedOn = DateTime.Now,
                                              Price = i * 100
                                          };
                category.AddProduct(product);
            }
        }
    }
}