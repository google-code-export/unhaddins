using System;
using System.Collections.Generic;
using uNhAddIns.Example.AspNetMVCConversationUsage.Entities;


namespace uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects
{
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