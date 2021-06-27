
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCenter.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace GameCenter.Services
{
    public class LinksGenerator
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor actionContextAccessor;

        public LinksGenerator(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper GetUrlHelper()
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public async Task Generate<T>(ResultExecutingContext context, ResultExecutionDelegate next) where T: class, IGenerateHATEOASLinks, new()
        {
            var urlHelper = GetUrlHelper();
            var result = context.Result as ObjectResult;
            var model = result.Value as T;
            if(model == null)
            {
                var modelList = result.Value as List<T> ?? throw new ArgumentNullException($"Was expecting an instans of {typeof(T)}");
                modelList.ForEach(dto => dto.GenerateLinks(urlHelper));
                var individual = new T();
                result.Value = individual.GenerateLinksCollection(modelList, urlHelper);
                await next();
            }
            else
            {
                model.GenerateLinks(urlHelper);
                await next();
            }

        }
    }
}