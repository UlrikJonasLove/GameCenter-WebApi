using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GameCenter.DTOs
{
    public interface IGenerateHATEOASLinks
    {
        void GenerateLinks(IUrlHelper urlHelper);
        ResourceCollection<T> GenerateLinksCollection<T>(List<T> dtos, IUrlHelper urlHelper);
    }
}