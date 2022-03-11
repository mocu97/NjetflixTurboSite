using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace CMStest.Models
{
    public class UmbracoModel : ContentModel
    {
        public UmbracoModel(IPublishedContent content) : base(content)
        {

        }
        public List<string> Categories { get; set; }
        public List<Tagg> Taggs { get; set; }
        public List<MoviesModel> Movies { get; set; }
    }

    public class Tagg
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
    public class MoviesModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime PubDate { get; set; }
        public MediaModel PickMedia { get; set; }
        public List<Tagg> MovieTag { get; set; }
    }
    public class MediaModel
    {
        public string Name { get; set; }
        public List<string> ImageUrl { get; set; }
        public string YtUrl { get; set; }
        public string VideoUrl { get; set; }
    }
    public class CatAndMovies
    {
        public string Name { get; set; }
        public List<MoviesModel> Movies { get; set; }
    }
}