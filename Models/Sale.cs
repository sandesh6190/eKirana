﻿using System.ComponentModel.DataAnnotations.Schema;
using eKirana.Models;

namespace eKirana;
public class Sale
{
    public long Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerAddress { get; set; }
    public DateTime SaleDate { get; set; }
    public long SaleById { get; set; }
    [ForeignKey("SaleById")]
    public virtual Admin Admin { get; set; }
    public decimal TotalAmount { get; set; }
}
