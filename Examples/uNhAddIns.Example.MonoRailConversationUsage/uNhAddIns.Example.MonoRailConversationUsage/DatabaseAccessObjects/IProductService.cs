using System;
using System.Collections.Generic;
using uNhAddIns.Example.MonoRailConversationUsage.Entities;

namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
   public interface IProductCategoryService {
        Category SaveCategory(Category entity);
        void DeleteCategory(Category entity);
        void Commit();
        void AcceptChanges();
        void RejectChanges();
       Category FindCategoryById(int id);
       Category FindCategoryByIdSpR(int id);
       IList<Category> FindAllCategories();
   }
}