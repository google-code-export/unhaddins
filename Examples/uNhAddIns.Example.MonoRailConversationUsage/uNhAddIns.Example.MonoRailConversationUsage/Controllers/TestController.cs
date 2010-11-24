using System;
using Castle.MonoRail.Framework;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Example.MonoRailConversationUsage.DatabaseAccessObjects;
using uNhAddIns.Example.MonoRailConversationUsage.Entities;
using uNhAddIns.Example.MonoRailConversationUsage.Utils;

namespace uNhAddIns.Example.MonoRailConversationUsage.Controllers {
    public class TestController : SmartDispatcherController {
        readonly IProductCategoryService service;
        ISampler sampler;

        public TestController(IProductCategoryService service, ISampler sample) {
            this.service = service;
            sampler = (ISampler)ServiceLocator.Current.GetInstance(typeof(ISampler));
        }


        [Layout("default")]
        public void Index(string message) {
            message = !string.IsNullOrEmpty(message) ? message : "no action";
            PropertyBag["message"] = message;
            PropertyBag["items"] = service.FindAllCategories();
        }
        
        public void Create() {
            // create sample category
            Category category = sampler.CreateSampleCategory();
            service.SaveCategory(category);
            string message = string.Format("Added {0} product", category.Name);
            RedirectToAction("index", "message=" + message);
        }

        public void Delete(int id) {
            Category categories = service.FindCategoryById(id);
            service.DeleteCategory(categories);
            service.Commit();
            RedirectToAction("index");
        }

        public void Commit() {
            service.Commit();
            RedirectToAction("index", "message=commiting.");
        }

        public void Accept() {
            service.AcceptChanges();
            RedirectToAction("index", "message=accept changes.");
        }

        public void Cancel() {
            service.RejectChanges();
            RedirectToAction("index", "message=reject changes.");
        }

        [Layout("default")]
        public void GetProducts(int categoryId) {
            service.AcceptChanges(); // for test purpose, we need to close session
            
            // can load objects lazily, for example, i can use "item.Products"
            Category category = service.FindCategoryById(categoryId);
            PropertyBag["item"] = category;
            RenderView("lazytest");
        }

        [Layout("default")]
        public void GetProductsWithLazyLoadException(int categoryId) {
            service.AcceptChanges(); // for test purpose, we needed that session be closed

            // can't load objects lazily, i get the exception here, because session was closed
            Category category = service.FindCategoryByIdSpR(categoryId);
            PropertyBag["item"] = category;
            RenderView("lazytest");
        }
    }
}