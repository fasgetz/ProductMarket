using AutoMapper;
using ProductMarketModels;
using ProductMarketModels.ViewModels.Admin.DiscountController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceProductMarket.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddDiscountViewModel, DiscountProduct>();
            CreateMap<EditDiscountViewModel, DiscountProduct>();
        }
    }
}
