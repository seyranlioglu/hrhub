using HrHub.Abstraction.Consts; // Policies ve Roles constantları için
using HrHub.Abstraction.Data.EfCore.Repository;
using HrHub.Abstraction.Result;
using HrHub.Core.Base;
using HrHub.Core.Data.Repository;
using HrHub.Domain.Contracts.Dtos.MenuDtos;
using HrHub.Domain.Entities.SqlDbEntities;
using HrHub.Identity.Entities;
using HrHub.Infrastructre.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HrHub.Application.Managers.MenuManagers
{
    public class MenuManager : ManagerBase, IMenuManager
    {
        private readonly Repository<SysMenu> _menuRepository;
        private readonly UserManager<AppUser> _userManager;

        public MenuManager(IHttpContextAccessor httpContextAccessor,
                           IHrUnitOfWork unitOfWork,
                           UserManager<AppUser> userManager) : base(httpContextAccessor)
        {
            _menuRepository = unitOfWork.CreateRepository<SysMenu>();
            _userManager = userManager;
        }

        public async Task<Response<UserMenuResponseDto>> GetCurrentUserMenuAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());

            // 1. Kullanıcının ROLLERİNİ al (Admin, User, SA)
            var userRoles = await _userManager.GetRolesAsync(user);

            // 2. Kullanıcının POLİTİKALARINI (Claims/Durum) belirle
            var userPolicies = new List<string>();

            if (IsMainUser())
                userPolicies.Add(HrHub.Abstraction.Consts.Policies.MainUser); // "MainUser" string değeri

            if (IsInstructor())
                userPolicies.Add(HrHub.Abstraction.Consts.Policies.Instructor); // "Instructor" string değeri

            // 3. Veritabanı Sorgusu (OR Mantığı)
            // Kural: Menü (Kullanıcının Rollerindan birine sahipse) VEYA (Kullanıcının Politikalarından birine sahipse) GETİR.
            IEnumerable<SysMenu> allMenus = null;
            try
            {
                allMenus = await _menuRepository.GetListAsync(
                    predicate: m => m.IsActive,
                    // Hem Rolleri hem Politikaları Include ediyoruz
                    include: i => i.Include(x => x.MenuRoles).ThenInclude(r => r.Role)
                                   .Include(x => x.MenuPolicies),
                    orderBy: o => o.OrderBy(x => x.OrderNo)
                );
            }
            catch (Exception)
            {

                throw;
            }

            var filteredMenus = new List<SysMenu>();

            if (IsSuperAdmin())
            {
                // SuperAdmin her şeyi görür (Opsiyonel: SA için de rol tanımlıysa if'i kaldırabilirsin)
                filteredMenus = allMenus.ToList();
            }
            else
            {
                filteredMenus = allMenus.Where(m =>
                    // 1. Rol Kontrolü: Menünün rollerinden biri kullanıcının rollerinde var mı?
                    (m.MenuRoles.Any() && m.MenuRoles.Any(mr => userRoles.Contains(mr.Role.Name)))
                    ||
                    // 2. Politika Kontrolü: Menünün politikalarından biri kullanıcının politikalarında var mı?
                    (m.MenuPolicies.Any() && m.MenuPolicies.Any(mp => userPolicies.Contains(mp.PolicyName)))
                    ||
                    // 3. Herkese Açık: Hiçbir kısıtlama yoksa göster
                    (!m.MenuRoles.Any() && !m.MenuPolicies.Any())
                ).ToList();
            }

            // 4. Tree Yapısını Oluştur
            var menuTree = GenerateTree(filteredMenus, null);

            var response = new UserMenuResponseDto
            {
                MenuItems = menuTree,
                IsInstructor = IsInstructor()

            };

            return ProduceSuccessResponse(response);
        }

        private List<UserMenuDto> GenerateTree(List<SysMenu> allMenus, long? parentId)
        {
            return allMenus
                .Where(c => c.ParentId == parentId)
                .Select(c => new UserMenuDto
                {
                    Title = c.Title,
                    Path = c.Path,
                    Icon = c.Icon,
                    Children = GenerateTree(allMenus, c.Id)
                })
                .ToList();
        }
    }
}