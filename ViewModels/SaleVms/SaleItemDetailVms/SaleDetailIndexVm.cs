using eKirana.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.SaleVms.SaleItemDetailVms;
public class SaleDetailIndexVm
{
    public long SaleId { get; set; }
    public long? ProductId { get; set; }
    public List<Product>? Products { get; set; }
    public SelectList ProductSelectList()
    {
        return new SelectList(
            Products,
            nameof(Product.Id),
            nameof(Product.Name),
            ProductId
        );
    }
    public List<SaleDetailInfoVm>? SaleDetailInfoVms;
}
