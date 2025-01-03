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
        var accounts = await _paymentService.GetAllPaymentsAsync();

        return Ok(accounts);
    }

    [PermissionAuthorize(7, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _paymentService.GetPaymentByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(7, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] PaymentServiceHelperDto paymentServiceHelperDto)
    {
        if (paymentServiceHelperDto == null)
        {
            return BadRequest(CreateErrorResponse("Payment data is null."));
        }

        await _paymentService.CreatePaymentAsync(paymentServiceHelperDto);

        return Ok(CreateSuccessResponse("Payment successfully created."));
    }

    [PermissionAuthorize(7, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] PaymentServiceForUpdateDto paymentServiceForUpdateDto)
    {
        if (paymentServiceForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Payment data is null."));
        }

        if (id != paymentServiceForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {paymentServiceForUpdateDto.Id}."));
        }

        await _paymentService.UpdatePaymentAsync(paymentServiceForUpdateDto);

        return Ok(CreateSuccessResponse("Payment successfully updated."));
    }

    [PermissionAuthorize(7, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _paymentService.DeletePaymentAsync(id);

        return Ok(CreateSuccessResponse("Payment successfully deleted."));
    }
}
