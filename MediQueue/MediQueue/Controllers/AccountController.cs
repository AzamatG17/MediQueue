﻿using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/accounts")]
//[EnableCors("AllowSpecificOrigins")] 
public class AccountController : BaseController
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
        var accounts = await _accountService.GetAllAccountsAsync();

        return Ok(accounts);
    }
    
    [PermissionAuthorize(1, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _accountService.GetAccountByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(1, 3)]
    [HttpPost]
    public async Task<ActionResult<ReturnResponse>> PostAsync([FromBody] AccountForCreateDto accountForCreateDto)
    {
        if (accountForCreateDto == null)
            return BadRequest(CreateErrorResponse("Account data is null."));

        await _accountService.CreateAccountAsync(accountForCreateDto);

        return Ok(CreateSuccessResponse("Account successfully created."));
    }

    [PermissionAuthorize(1, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<ReturnResponse>> PutAsync(int id, [FromBody] AccountForUpdateDto accountForUpdateDto)
    {
        if (accountForUpdateDto == null)
            return BadRequest(CreateErrorResponse("Account data is null."));

        if (id != accountForUpdateDto.Id)
            return BadRequest(CreateErrorResponse($"Route id: {id} does not match with parameter id: {accountForUpdateDto.Id}."));

        await _accountService.UpdateAccountAsync(accountForUpdateDto);

        return Ok(CreateSuccessResponse("Account successfully updated."));
    }

    [PermissionAuthorize(1, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<ReturnResponse>> DeleteAsync(int id)
    {
        await _accountService.DeleteAccountAsync(id);

        return Ok(CreateSuccessResponse("Account successfully deleted."));
    }
}
