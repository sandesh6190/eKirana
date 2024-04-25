using eKirana.Models.SetUp;

namespace eKirana.ViewModels.ProductVms;
public class ProductInfoVm
{
    //In this Vm we can customize property to display on index page but right now here's no extra property to show thus, it has similar properties as product model 
    public long ProductId { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public string ProductVATorNOT { get; set; }
    public Category Category { get; set; } //yesma category model ko dherai property access garna sakincha but alik optimized nahuna skcha bhane..brandName ko ma brandnName matra tanera vmma pathauda alik optimized huna jancha mero bicharma
    public string? BrandName { get; set; }
}
