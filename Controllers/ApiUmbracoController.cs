using CMStest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Umbraco.Web.WebApi;
using Umbraco.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Mvc;

namespace CMStest.Controllers
{
    [PluginController("Api")]
    public class ApiUmbracoController : UmbracoApiController
    {
        [HttpGet]
        public IHttpActionResult GetData()
        {
            var homeUmb = Umbraco.ContentAtRoot().FirstOrDefault();
            var taggUmb = Umbraco.ContentAtRoot().Skip(1).FirstOrDefault();

            var apiData = new ApiModel()
            {
                ApiCategory = GetCategoryAndMovie(homeUmb),
                ApiTaggs = GetTag(taggUmb)
            };
            return Json(apiData);
        }

        #region ApiEndpoints
        public IHttpActionResult GetMovies()
        {
            var model = Umbraco.ContentAtRoot().FirstOrDefault();
            var movies = new List<MoviesModel>();
            foreach (var parent in model.Children)
            {
                foreach (var children in parent.Children)
                {
                    var tagList = new List<Tagg>();
                    var tagshlpr = children.Value<IEnumerable<IPublishedContent>>("Tags");
                    var mediahlpr = children.Value<IPublishedElement>("PickMedia");
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
                            Id = children.Id,
                            Title = onetag.Value("Title").ToString(),
                            Description = onetag.Value("Descritpion").ToString(),
                            Image = imagetag.LocalCrops.Src
                        });
                    }
                    movies.Add(new MoviesModel
                    {
                        Id = children.Id,
                        Title = (string)children.Value("Title"),
                        Description = (string)children.Value("Description"),
                        PubDate = (DateTime)children.Value("PubDate"),
                        MovieTag = tagList,
                        Category = parent.Name,
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
            return Json(movies);
        }
        public IHttpActionResult GetTags()
        {
            var listtags = Umbraco.ContentAtRoot().Skip(1).FirstOrDefault();
            return Json(GetTag(listtags));
        }
        public IHttpActionResult GetCategories()
        {
            var cats = Umbraco.ContentAtRoot().FirstOrDefault();
            var categories = new List<string>();
            foreach (var category in cats.Children)
            {
                categories.Add(category.Name);
            }
            return Json(categories);
        }
        #endregion

        public List<CatAndMovies> GetCategoryAndMovie(IPublishedContent catUmb)
        {
            var categories = new List<CatAndMovies>();
            var movies = new List<MoviesModel>();

            foreach (var parent in catUmb.Children)
            {
                foreach (var children in parent.Children)
                {
                    var tagList = new List<Tagg>();
                    var tagshlpr = children.Value<IEnumerable<IPublishedContent>>("Tags");
                    var mediahlpr = children.Value<IPublishedElement>("PickMedia");
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
                            Id = children.Id,
                            Title = onetag.Value("Title").ToString(),
                            Description = onetag.Value("Descritpion").ToString(),
                            Image = imagetag.LocalCrops.Src
                        });
                    }
                    movies.Add(new MoviesModel
                    {
                        Id = children.Id,
                        Title = (string)children.Value("Title"),
                        Description = (string)children.Value("Description"),
                        PubDate = (DateTime)children.Value("PubDate"),
                        MovieTag = tagList,
                        Category = parent.Name,
                        PickMedia = new MediaModel
                        {
                            Name = mediahlpr.ContentType.Alias.ToString(),
                            ImageUrl = imageList,
                            YtUrl = yturl,
                            VideoUrl = videourl
                        }
                    });
                }
                categories.Add(new CatAndMovies()
                {
                    Name = parent.Name,
                    Movies = movies
                });
            }

            return categories;
        }
        public List<Tagg> GetTag(IPublishedContent tagUmb)
        {
            var taggs = new List<Tagg>();
            foreach (var item in tagUmb.Children)
            {
                var hlpr = item.Value<MediaWithCrops>("Image");
                taggs.Add(new Tagg()
                {
                    Id = item.Id,
                    Title = item.Value("Title").ToString(),
                    Description = item.Value("Descritpion").ToString(),
                    Image = hlpr.LocalCrops.Src
                });
            }
            return taggs;
        }
    }
}