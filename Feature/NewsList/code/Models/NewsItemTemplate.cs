using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor.RichTextEditor;

namespace Demo.Feature.NewsList.Models
{

    // Example Model to show what Fields would be on a News Item Template
    public class NewsItemTemplate
    {
        //Title String
        public string Title { get; set; }

        //Rich Text Description
        public RichTextValue Description { get; set; }

        // Date Published
        public DateTime PublishDate { get; set; }

        //DropLink for category item
        public Item Category { get; set; }

        // Author
        public string Author { get; set; }

        // Image Field
        public ImageField HeaderImage { get; set; }

        //Checkbox Field to Include in Search, Standard Values will set it to true by default
        public CheckboxField IncludeInSearch { get; set; }
    }
}
