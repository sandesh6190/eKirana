﻿using System.ComponentModel.DataAnnotations.Schema;
using eKirana.Constants;
using eKirana.Models.SetUp;


namespace eKirana.Models;
public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Photo { get; set; }
    public long? BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public string ProductVATorNOT { get; set; } = ProductVATorNOTConstants.NoVATProduct;
    // public decimal? Stock_Quantity { get; set; }
    // public long? UnitId { get; set; }
    // public virtual Unit? Unit { get; set; }
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; }
    // public decimal? PurchaseRateAmt{get; set;} //for only displaying on product index
    // public long? ProductPurchaseRateId { get; set; }
    // //[ForeignKey("ProductPurchaseRateId")]
    // public virtual ProductPurchaseRate? ProductPurchaseRate { get; set; }
    // public long ProductSaleRateId { get; set; }
    // public virtual ProductSaleRate ProductSaleRate { get; set; }
}
