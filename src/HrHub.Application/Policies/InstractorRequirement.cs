using HrHub.Infrastructre.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HrHub.Application.Policies
{
    public class InstractorRequirement : IAuthorizationRequirement
    {


    }

    public class InstractorRequirementHandler : AuthorizationHandler<InstractorRequirement>
    {
        private readonly IInstructorRepository instructorRepository;
        public InstractorRequirementHandler(IInstructorRepository instructorRepository)
        {
            this.instructorRepository = instructorRepository;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, InstractorRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }
            long userId = Convert.ToInt64(context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var isInstructorExists =  instructorRepository.Count(p => p.UserId == userId);

            if (isInstructorExists>0)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }


        }
    }
}
