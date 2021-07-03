using System.Collections.Generic;
using GameCenter.DTOs;
using GameCenter.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controller
{
    [Route("api")]
    //[HttpHeaderIsPresent("x-version", "1")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<Link>> Get()
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(href: Url.Link("getRoot", new { }), rel: "self", method: "GET"));
            links.Add(new Link(href: Url.Link("createUser", new { }), rel: "create-user", method: "POST"));
            links.Add(new Link(href: Url.Link("Login", new { }), rel: "login", method: "POST"));
            links.Add(new Link(href: Url.Link("getGenres", new { }), rel: "get-genres", method: "GET"));
            links.Add(new Link(href: Url.Link("getPeople", new { }), rel: "get-people", method: "GET"));

            return links;
        }
    }
}