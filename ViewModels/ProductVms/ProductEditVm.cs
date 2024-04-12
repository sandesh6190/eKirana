using eKirana.Constants;
using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductVms;
public class ProductEditVm
{
    // public long Id { get; set; }
    public string? Name { get; set; }
    public IFormFile? Photo { get; set; }
    public long? CategoryId { get; set; }
    public List<Category>? Categories;
    public SelectList CategorySelectList()
    {
        return new SelectList(
            Categories,
            nameof(Category.Id),
            nameof(Category.Item),
            CategoryId
        );
    }
    public long? BrandId { get; set; }
    public List<Brand> Brands;
    //select list of brand
    public SelectList BrandSelectList()
    {
        return new SelectList(
            Brands,
            nameof(Brand.Id),
            nameof(Brand.BrandName),
            BrandId
        );
    }

    public string ProductVATorNOT { get; set; }
    public SelectList ProductVATorNOTSelectList()
    {
        return new SelectList(
            ProductVATorNOTConstants.ProductTypeList,
            ProductVATorNOT
        );
    }
}
