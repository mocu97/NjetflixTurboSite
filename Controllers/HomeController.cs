using CMStest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using static CMStest.Models.UmbracoModel;

namespace CMStest.Controllers
{
    public class HomeController : RenderMvcController
    {
        public ActionResult Index(ContentModel model)
        {
            UmbracoModel umbracoModel = new UmbracoModel(model.Content)
            {
            };
            umbracoModel.Categories = GetAllCategories(model.Content);
            umbracoModel.Movies = GetAllMovies(model.Content);
            umbracoModel.Taggs = GetTagg(model.Content.Siblings());

            return CurrentTemplate(umbracoModel);

        }
        public List<string> GetAllCategories(IPublishedContent publishedContent)
        {
            var categories = new List<string>();
            foreach (var item in publishedContent.Children)
            {
                categories.Add(item.Name);
            }
            return categories;
        }
        public List<MoviesModel> GetAllMovies(IPublishedContent publishedContent)
        {
            var movies = new List<MoviesModel>();
            foreach (var category in publishedContent.Children)
            {
                foreach (var movie in category.Children)
                {
                    var tagList = new List<Tagg>();
                    var tagshlpr = movie.Value<IEnumerable<IPublishedContent>>("Tags");
                    var mediahlpr = movie.Value<IPublishedElement>("PickMedia");
                    var value = mediahlpr.Value<IEnumerable<MediaWithCrops>>("Image");
                    var value2 = mediahlpr.Value("Yturl");
                    var value3 = mediahlpr.Value<MediaWithCrops>("UserVid");
                    var yturl = string.Empty;
                    var imageList = new List<string>();
                    var videourl = string.Empty;
                    if (value != null)
                    {
                        foreach (var item in value)
                        {
                            imageList.Add(item.LocalCrops.Src);
                        }
                    }
                    if (value2 != null)
                    {
                        yturl = value2.ToString();
                        yturl = yturl.Split('=')[1];
                    }
                    if (value3 != null)
                    {
                        videourl = value3.LocalCrops.Src;
                    }
                    foreach (var onetag in tagshlpr)
                    {
                        var imagetag = onetag.Value<MediaWithCrops>("image");
                        tagList.Add(new Tagg()
                        {
                            Id = movie.Id,
                            Title= onetag.Value("Title").ToString(),
                            Description = onetag.Value("Descritpion").ToString(), 
                            Image = imagetag.LocalCrops.Src
                        });
                    }
                    movies.Add(new MoviesModel
                    {
                        Id = movie.Id,
                        Title = (string)movie.Value("Title"),
                        Description = (string)movie.Value("Description"),
                        PubDate = (DateTime)movie.Value("PubDate"),
                        MovieTag = tagList,
                        PickMedia = new MediaModel
                        {
                            Name = mediahlpr.ContentType.Alias.ToString(),
                            ImageUrl = imageList,
                            YtUrl = yturl,
                            VideoUrl = videourl
                        }
                    });
                }
            }
            return movies;
        }
        public static List<Tagg> GetTagg(IEnumerable<IPublishedContent> publishedContent)
        {
            var tagg = new List<Tagg>();
            foreach (var item in publishedContent)
            {
                foreach (var item2 in item.Children)
                {
                    var hlpr = item2.Value<MediaWithCrops>("Image");
                    tagg.Add(new Tagg
                    {
                        Id = item2.Id,
                        Title = item2.Value("Title").ToString(),
                        Description = item2.Value("Descritpion").ToString(),
                        Image = hlpr.LocalCrops.Src
                    });
                }
            }
            return tagg;
        }
    }
}