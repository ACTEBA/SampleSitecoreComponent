using Demo.Feature.NewsList.Models;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Demo.Feature.NewsList.Controllers
{
    public class NewsListController : Controller
    {
        public static Database context = Sitecore.Context.Database;

        public IActionResult Index()
        {           
            NewsListDataModel model = new NewsListDataModel();
            
            List<Item> newsItems = FetchNewsItems();

            List<Item> categoryItems = FetchCategoryItems();

            model.NewsItems = newsItems;
            model.Categories = categoryItems;

            return View(model);
        }

        [HttpPost]
        [ActionName("FilterNewsItems")]
        public IActionResult FilterNewsItems(FilterRequestModel filterRequest)
        {
            NewsListDataModel model = new NewsListDataModel();

            string? categoryValue = null;
            DateTime? searchDate = null;

            // Check and Set Category Value for filtering
            if(!string.IsNullOrEmpty(filterRequest.CategoryValue))
                categoryValue = filterRequest.CategoryValue;

            // Check and Set Publish Date Value for filtering
            if (!string.IsNullOrEmpty(filterRequest.PublishedBefore))
            {
                DateTime parsedDate;

                if (DateTime.TryParse(filterRequest.PublishedBefore, out parsedDate))
                { 
                    searchDate = parsedDate;
                }
            }
               
            List<Item> newsItems = FetchNewsItems(categoryValue, searchDate);

            List<Item> categoryItems = FetchCategoryItems();

            model.NewsItems = newsItems;
            model.Categories = categoryItems;

            return View("Index", model);
        }

        public List<Item> FetchNewsItems(string category = "", DateTime? dateTime = null)
        {
            // Get Folder Content Item for News items, using configuration setting for ID.
            Item NewsFolder = context.GetItem(Sitecore.Configuration.Settings.GetSetting("NewsDataFolderItem"));

            // if no News folder, set the NewsFolder to search under the home item
            if (NewsFolder == null)
                NewsFolder = context.GetItem("sitecore/content/home");

            // Construct Query
            string newsQuery = "query:" + NewsFolder.Paths.FullPath + "/*[@@templateid='" + Constants.NewsTemplate.ID.ToString() + "']";

            // Fetch News Item based on Template then filter on custom property "Include in Search"
            List<Item> allNewsItems = context.SelectItems(newsQuery).Where(x => x.Fields["IncludeInSearch"].Value.Equals("1")).ToList();

            List<Item> filteredNewsItems = new List<Item>();

            // Filter items depending on passed properties
            if (!string.IsNullOrEmpty(category) && dateTime != null)
                filteredNewsItems = allNewsItems.Where(x => x.Fields["Category"].Value.Equals(category) && (Sitecore.DateUtil.IsoDateToDateTime(x.Fields["PublishDate"].Value) <= dateTime)).ToList();
            else if (!string.IsNullOrEmpty(category))
                filteredNewsItems = allNewsItems.Where(x => x.Fields["Category"].Value.Equals(category)).ToList();
            else if (dateTime != null)
                filteredNewsItems = allNewsItems.Where(x => Sitecore.DateUtil.IsoDateToDateTime(x.Fields["PublishDate"].Value) <= dateTime).ToList();
            else
                return allNewsItems;

          
            return filteredNewsItems;
        }

        public List<Item> FetchCategoryItems()
        {
            // Get Folder Content Item for News items, using configuration setting for ID.
            Item CategoryFolder = context.GetItem(Sitecore.Configuration.Settings.GetSetting("CategoryDataFolderItem"));

            // if no News folder search, set the NewsFolder to search under the home item
            if (CategoryFolder == null)
                CategoryFolder = context.GetItem("sitecore/content/home");

            // Construct Query
            string categoryQuery = "query:" + CategoryFolder.Paths.FullPath + "/*[@@templateid='" + Constants.CategoryTemplate.ID.ToString() + "']";

            List<Item> allCategories = context.SelectItems(categoryQuery).ToList();

            return allCategories;
        }
    }
}
