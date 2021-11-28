using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Data;
using Daxone_Testing.Data.Repositories;
using Daxone_Testing.Models;
using Microsoft.AspNetCore.Mvc;

namespace Daxone_Testing.Components
{
    public class ProductGroupsComponent : ViewComponent//جا نشین  پارشیال ویو ها
    {
        private IGroupRepository _groupRepository;

        public ProductGroupsComponent(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Components/Product/ProductGroupsComponent.cshtml", _groupRepository.getGroupForShowCategory());
        }

    }
}
