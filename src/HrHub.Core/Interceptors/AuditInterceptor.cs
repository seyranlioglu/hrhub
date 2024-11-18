using HrHub.Abstraction.Data.Context;
using HrHub.Core.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Core.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            var context = eventData.Context;

            if (context != null)
            {
                SetAuditFields(context);
            }

            return base.SavingChanges(eventData, result);
        }

        private void SetAuditFields(DbContext context)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var entries = context.ChangeTracker.Entries()
    .Where(entry => entry.Entity.GetType().IsGenericType &&
                    (entry.Entity.GetType().GetGenericTypeDefinition() == typeof(CardEntity<>) ||
                     entry.Entity.GetType().GetGenericTypeDefinition() == typeof(TypeCardEntity<>)));

                if (entries.Any()) 
                {
                    var userId = GetCurrentUserId();

                    foreach (var entry in entries)
                    {
                        dynamic entity = entry.Entity;
                        switch (entry.State)
                        {
                            case EntityState.Modified:
                                entity.UpdateDate = DateTime.UtcNow;
                                entity.UpdateUserId = userId;
                                break;
                            case EntityState.Added:
                                entity.CreatedDate = DateTime.UtcNow;
                                entity.CreateUserId = userId;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = Convert.ToInt64(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return userIdClaim;
        }
    }
}
