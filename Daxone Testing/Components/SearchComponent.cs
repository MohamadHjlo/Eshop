using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Daxone_Testing.Components
{
    public class SearchComponent:ViewComponent
    {
        private IGroupRepository _groupRepository;

        public SearchComponent(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Components/Search/Search.cshtml", _groupRepository.getGroupForShowCategory());
        }
    }
}
