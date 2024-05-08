using eKirana.Constants;
using eKirana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace eKirana.ViewModels.StockQuantityVms;
public class StockQuantityIndexVm
{
    public long? ProductId { get; set; }
    public List<Product> Products;
    public SelectList ProductSelectList()
    {
        return new SelectList(
            Products,
            nameof(Product.Id),
            nameof(Product.Name),
            ProductId
            );
    }
    public long? AdminId { get; set; }
    public List<Admin>? Admins;
    public SelectList AdminSelectList()
    {
        return new SelectList(
        Admins,
        nameof(Admin.Id),
        nameof(Admin.UserName),
        AdminId
        );
    }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Remarks { get; set; }
    public SelectList StockQuantityRemarksList()
    {
        return new SelectList(
            StockQuantityRemarksConstants.StockQuantityRemarksList,
            Remarks
        );
    }
    public List<StockQuantityInfoVm>? StockQuantityInfoVms { get; set; }
}
