using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Adapters;
using uNhAddIns.Example.MonoRailConversationUsage.Entities;

namespace uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects {
    [PersistenceConversational]
    public class ProductCategoryService : IProductCategoryService  {
        readonly IDao<Category> categoryDao;
        readonly IProductDao productDao;

        public ProductCategoryService() {
            categoryDao = (IDao<Category>) ServiceLocator.Current.GetInstance(typeof(IDao<Category>));
            productDao = (IProductDao)ServiceLocator.Current.GetInstance(typeof(IProductDao));
        }

        [PersistenceConversation]
        public Category SaveCategory(Category entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            return categoryDao.MakePersistent(entity);
        }

     [PersistenceConversation]
        public Product SaveProduct(Product entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            return productDao.MakePersistent(entity);
        }

        [PersistenceConversation]
        public void DeleteCategory(Category entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            categoryDao.MakeTransient(entity);
        }

      
        [PersistenceConversation]
        public void DeleteProduct(Product entity) {
            if (entity == null) {
                throw new ArgumentNullException("entity");
            }
            productDao.MakeTransient(entity);
        }


        [PersistenceConversation(ConversationEndMode = EndMode.CommitAndContinue)]
        public void Commit() {
        }

        [PersistenceConversation(ConversationEndMode = EndMode.End)]
        public void AcceptChanges() {
        }

        [PersistenceConversation(ConversationEndMode = EndMode.Abort)]
        public void RejectChanges() { }

        [PersistenceConversation]
        public IList<Category> FindAllCategoriesByCpBT() {
            return categoryDao.FindAll();
        }

        [PersistenceConversation(ConversationEndMode = EndMode.End)]
        public IList<Category> FindAllCategoriesBySpR() {
            return categoryDao.FindAll();
        }

        [PersistenceConversation]
        public Category FindCategoryById(int id) {
            return categoryDao.TryFind(id);
        }

        // To simulate "Session Per Request" pattern
        [PersistenceConversation(ConversationEndMode = EndMode.End)]
        public Category FindCategoryByIdSpR(int id) {
            return categoryDao.TryFind(id);
        }

        [PersistenceConversation]
        public IList<Category> FindAllCategories() {
            return categoryDao.FindAll();
        }

        [PersistenceConversation]
        public IList<Product> FindProductByName(string name) {
            return productDao.FindByName(name);
        }

    }
}