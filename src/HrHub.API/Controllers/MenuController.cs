using HrHub.Abstraction.Result;
using HrHub.Application.Managers.MenuManagers;
using HrHub.Core.Controllers;
using HrHub.Domain.Contracts.Dtos.MenuDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HrHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenuController : ApiControllerBase
    {
        private readonly IMenuManager _menuManager;

        public MenuController(IMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        [HttpGet("my-menu")]
        public async Task<Response<UserMenuResponseDto>> GetMyMenu()
        {
            return await _menuManager.GetCurrentUserMenuAsync();
        }
    }
}