using Sitecore.Data.Items;

namespace Demo.Feature.NewsList.Models
{
    public class NewsListDataModel
    {
        public List<Item> NewsItems { get; set; }

        public List<Item> Categories { get; set; }

    }
}
