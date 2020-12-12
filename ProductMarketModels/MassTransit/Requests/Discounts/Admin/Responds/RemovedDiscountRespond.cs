using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMarketModels.MassTransit.Requests.Discounts.Admin.Responds
{

    /// <summary>
    /// Удаление акции
    /// </summary>
    public partial class RemovedDiscountRespond
    {
        public bool HasRemoved { get; set; }
    }
}
