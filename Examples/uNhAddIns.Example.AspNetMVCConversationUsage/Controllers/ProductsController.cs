using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Example.AspNetMVCConversationUsage.DatabaseAccessObjects;
using uNhAddIns.Example.AspNetMVCConversationUsage.Entities;
using uNhAddIns.Example.AspNetMVCConversationUsage.Utils;
using MvcContrib.Filters;
using MvcContrib;

namespace uNhAddIns.Example.AspNetMVCConversationUsage.Controllers
{
    [HandleError]
    [PassParametersDuringRedirect]
    public class ProductsController : Controller
    {
       readonly IProductCategoryService service;
       ISampler sampler;

        public ProductsController(IProductCategoryService service, ISampler sample)
        {
            this.service = service;
            sampler = (ISampler)ServiceLocator.Current.GetInstance(typeof(ISampler));
        }

        public ActionResult Index(string message)
        {
            message = !string.IsNullOrEmpty(message) ? message : "no action";
            ViewData["message"] = message;
            ViewData.Model = service.FindAllCategories();
            return View();
        }

        public ActionResult Create()
        {
            // create sample category
            Category category = sampler.CreateSampleCategory();
            service.SaveCategory(category);
            string message = string.Format("Added {0} product", category.Name);
            return this.RedirectToAction(c => c.Index(message));
        }

        public ActionResult Delete(int id)
        {
            Category categories = service.FindCategoryById(id);
            service.DeleteCategory(categories);
            service.Commit();
            return this.RedirectToAction(c => c.Index(string.Format("deleted {0}",id)));
        }

        public ActionResult Commit() {
            service.Commit();
            return this.RedirectToAction(c => c.Index("commiting"));
        }

        public ActionResult Accept()
        {
            service.AcceptChanges();
            return this.RedirectToAction(c => c.Index("accept changes"));
        }

        public ActionResult Cancel()
        {
            service.RejectChanges();
            return this.RedirectToAction(c => c.Index("reject changes"));
        }


        public ActionResult GetProducts(int id)
        {
            service.AcceptChanges(); // for test purpose, we need to close session
            
            // can load objects lazily, for example, i can use "item.Products"
            Category category = service.FindCategoryById(id);
            ViewData.Model = category;
            return View("lazytest");
        }

        public ActionResult GetProductsWithLazyLoadException(int id)
        {
            service.AcceptChanges(); // for test purpose, we needed that session be closed

            // can't load objects lazily, i get the exception here, because session was closed
            Category category = service.FindCategoryByIdSpR(id);
            ViewData.Model = category;
            return View("lazytest");
        }
    }
}
