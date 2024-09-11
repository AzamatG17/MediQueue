using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "AllAccountPermission")]
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

    [HttpGet]
    public async Task<ActionResult> GetAccountsAsync()
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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetAccountByIdAsync(int id)
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccount(int id)
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
