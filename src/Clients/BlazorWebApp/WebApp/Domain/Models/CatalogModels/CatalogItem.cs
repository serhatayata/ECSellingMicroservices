namespace WebApp.Domain.Models.CatalogModels
{
    public class CatalogItem
    {
        /// <summary>
        /// Id of catalog item
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of catalog item
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of catalog item
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Selling price of catalog item
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Picture file name of catalog item
        /// </summary>
        public string PictureFileName { get; set; }
        /// <summary>
        /// Picture uri of catalog item
        /// </summary>
        public string PictureUri { get; set; }
        /// <summary>
        /// CatalogTypeId
        /// </summary>
        public int CatalogTypeId { get; set; }
        /// <summary>
        /// CatalogType
        /// </summary>
        public CatalogType CatalogType { get; set; }
        /// <summary>
        /// CatalogBrandId
        /// </summary>
        public int CatalogBrandId { get; set; }
        /// <summary>
        /// CatalogBrand
        /// </summary>
        public CatalogBrand CatalogBrand { get; set; }
        /// <summary>
        /// Available stock 
        /// </summary>
        public int AvailableStock { get; set; }
        /// <summary>
        /// On reorder
        /// </summary>
        public bool OnReorder { get; set; }

        public CatalogItem()
        {

        }
    }
}
