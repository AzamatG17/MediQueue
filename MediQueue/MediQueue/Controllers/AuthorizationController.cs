using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController : BaseController
{
    private readonly IAuthorizationService _authorizationService;
    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(AccountForLoginDto accountForLogin)
    {
        var token = await _authorizationService.Login(accountForLogin);

        if (token == null)
        {
            return Unauthorized(CreateErrorResponse("Invalid login or password"));
        }

        HttpContext.Response.Cookies.Append("mediks-cookies", token.Token);

        return Ok(token);
    }

    //[HttpPost("register")]
    //public async Task<ActionResult<string>> Register(AccountForCreateDto accountForLogin)
    //{
    //    var token = await _authorizationService.Register(accountForLogin);

    //    if (token == null)
    //    {
    //        return Unauthorized("Invalid login or password");
    //    }

    //    //HttpContext.Response.Cookies.Append("tasty-cookies", token);

    //    return Ok(token);
    //}
}
