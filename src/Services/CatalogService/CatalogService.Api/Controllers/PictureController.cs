﻿using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly CatalogContext _catalogContext;

        public PictureController(IWebHostEnvironment env, CatalogContext catalogContext)
        {
            _env = env;
            _catalogContext = catalogContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("App and running");
        }

        [HttpGet]
        [Route("api/v1/catalog/items/{catalogItemId:int}/pic")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetImageAsync(int catalogItemId)
        {
            if (catalogItemId <= 0)
                return BadRequest();

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == catalogItemId);
            if (item != null)
            {
                var webRoot = _env.WebRootPath;
                var path = Path.Combine(webRoot, item.PictureFileName);

                string imageFileExtension = Path.GetExtension(item.PictureFileName);
                string mimeType = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

                var buffer = await System.IO.File.ReadAllBytesAsync(path);

                return File(buffer, mimeType);
            }

            return NotFound();
        }

        private string GetImageMimeTypeFromImageFileExtension(string extension)
        {
            string mimeType;

            switch (extension)
            {
                case ".png": mimeType = "image/png"; break;
                case ".jpg": mimeType = "image/jpeg"; break;
                case ".jpeg": mimeType = "image/jpeg"; break;
                default:
                    mimeType = string.Empty;
                    break;
            }

            return mimeType;
        }

    }
}
