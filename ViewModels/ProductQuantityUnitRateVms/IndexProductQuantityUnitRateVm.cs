﻿using eKirana.Models.SetUp;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eKirana.ViewModels.ProductQuantityUnitRateVms;
public class IndexProductQuantityUnitRateVm
{
    //this vm is especially for searching only
    //for searching through unit
    public long ProductId { get; set; }
    public long? UnitId { get; set; } //unitko list js bata fetch garne

    //for listing producQuantityUnitRate
    public List<InfoProductQuantityUnitRateVm>? InfoProductQuantityUnitRateVms; //yaha list of model garda pan hunthyo but would be bad practise
}
