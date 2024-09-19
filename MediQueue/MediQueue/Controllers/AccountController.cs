using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/accounts")]
//[EnableCors("AllowSpecificOrigins")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }

    [PermissionAuthorize(1, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(1, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _accountService.GetAccountByIdAsync(id);

            if (account is null)
                return NotFound($"Account with id: {id} does not exist.");

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(1, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] AccountForCreateDto accountForCreateDto)
    {
        if (accountForCreateDto == null)
        {
            return BadRequest("Account data is null.");
        }

        try
        {
            var createdAccount = await _accountService.CreateAccountAsync(accountForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(1, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto accountForUpdateDto)
    {
        if (accountForUpdateDto == null)
        {
            return BadRequest("Account data is null.");
        }

        if (id != accountForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {accountForUpdateDto.Id}.");
        }

        try
        {
            var updatedAccount = await _accountService.UpdateAccountAsync(accountForUpdateDto);
            return Ok(updatedAccount);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(1, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _accountService.DeleteAccountAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
