﻿using System;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Catalog;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public CatalogService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<CatalogItem> GetCatalogItemAsync(int id)
        {
            var client = httpClientFactory.CreateClient("catalog");
            var uri = client.BaseAddress + "/items/";
            var response = await client.GetResponseAsync<CatalogItem>(uri + id);

            return response;
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync(IEnumerable<int> ids)
        {
            string stringIds = string.Join(",", ids);

            var client = httpClientFactory.CreateClient("catalog");
            var uri = client.BaseAddress + "/items/";
            var response = await client.GetResponseAsync<List<CatalogItem>>(uri + stringIds);

            return response;
        }
    }
}
