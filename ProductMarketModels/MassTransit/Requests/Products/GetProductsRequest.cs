﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Products
{
    public class GetSubcategoriesRequest
    {
        public short IdCategoryProduct { get; set; }
    }
}
