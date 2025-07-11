using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public abstract class BaseApiControllers : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? throw new InvalidOperationException("IMediatR not found.");

    internal Guid UserId
    {
        get
        {
            var identity = User.Identity;

            if (identity == null || !identity.IsAuthenticated)
            {
                return Guid.Empty;
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return Guid.Empty;
            }

            return Guid.Parse(userIdClaim.Value);
        }
    }
}
