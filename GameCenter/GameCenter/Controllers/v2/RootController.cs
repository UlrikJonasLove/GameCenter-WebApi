/*using System.Collections.Generic;
using GameCenter.DTOs;
using GameCenter.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.Controller.v2
{
    [Route("api")]
    [HttpHeaderIsPresent("x-version", "2")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "getRoot")]
        public ActionResult<IEnumerable<Link>> Get()
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(href: Url.Link("getRoot", new { }), rel: "self", method: "GET"));

            return links;
        }
    }
} */