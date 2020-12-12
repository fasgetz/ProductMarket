using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Discounts.Admin.Responds
{
    public class GetDiscountsProductRespond
    {
        public List<DiscountProduct> data { get; set; }
    }
}
