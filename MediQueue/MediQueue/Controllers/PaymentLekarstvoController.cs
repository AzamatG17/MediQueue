using MediQueue.Domain.DTOs.PaymentLekarstvo;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/paymentlekarstvo")]
//[EnableCors("AllowSpecificOrigins")]
public class PaymentLekarstvoController : BaseController
{
    private readonly IPaymentLekarstvoService _paymentLekarstvoService;

    public PaymentLekarstvoController(IPaymentLekarstvoService paymentLekarstvoService)
    {
        _paymentLekarstvoService = paymentLekarstvoService ?? throw new ArgumentNullException(nameof(paymentLekarstvoService));
    }

    [PermissionAuthorize(16, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] PaymentLekarstvoHelperDto paymentLekarstvoHelperDto)
    {
        if (paymentLekarstvoHelperDto == null)
        {
            return BadRequest(CreateErrorResponse("Payment data is null."));
        }

        try
        {
            var createdAccount = await _paymentLekarstvoService.CreatePaymentLekarstvoAsync(paymentLekarstvoHelperDto);
            return Ok(CreateSuccessResponse("Payment successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
