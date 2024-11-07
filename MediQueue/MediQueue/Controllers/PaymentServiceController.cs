using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/payment")]
//[EnableCors("AllowSpecificOrigins")]
public class PaymentServiceController : BaseController
{
    private readonly IPaymentServiceService _paymentService;

    public PaymentServiceController(IPaymentServiceService paymentService)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    [PermissionAuthorize(7, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _paymentService.GetAllPaymentsAsync();

            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(7, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _paymentService.GetPaymentByIdAsync(id);

            if (account is null)
                return NotFound(CreateErrorResponse($"Payment with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Payment not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(7, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] PaymentServiceHelperDto paymentServiceHelperDto)
    {
        if (paymentServiceHelperDto == null)
        {
            return BadRequest(CreateErrorResponse("Payment data is null."));
        }

        try
        {
            var createdAccount = await _paymentService.CreatePaymentAsync(paymentServiceHelperDto);
            return Ok(CreateSuccessResponse("Payment successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(7, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] PaymentServiceForUpdateDto paymentServiceForUpdateDto)
    {
        if (paymentServiceForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Payment data is null."));
        }

        if (id != paymentServiceForUpdateDto.id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {paymentServiceForUpdateDto.id}."));
        }
        try
        {
            var updatedAccount = await _paymentService.UpdatePaymentAsync(paymentServiceForUpdateDto);
            return Ok(CreateSuccessResponse("Payment successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Payment not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(7, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _paymentService.DeletePaymentAsync(id);
            return Ok(CreateSuccessResponse("Payment successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", Payment not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
