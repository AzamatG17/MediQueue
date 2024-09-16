using MediQueue.Domain.DTOs.PaymentService;
using MediQueue.Domain.DTOs.Role;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "AllRolePermission")]
[ApiController]
[Route("api/payment")]
//[EnableCors("AllowSpecificOrigins")]
public class PaymentServiceController : ControllerBase
{
    private readonly IPaymentServiceService _paymentService;

    public PaymentServiceController(IPaymentServiceService paymentService)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
    }

    [HttpGet]
    public async Task<ActionResult> GetPaymentsAsync()
    {
        try
        {
            var accounts = await _paymentService.GetAllPaymentsAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPaymentByIdAsync(int id)
    {
        try
        {
            var account = await _paymentService.GetPaymentByIdAsync(id);

            if (account is null)
                return NotFound($"Payment with id: {id} does not exist.");

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
    public async Task<ActionResult> PostAsync([FromBody] PaymentServiceHelperDto paymentServiceHelperDto)
    {
        if (paymentServiceHelperDto == null)
        {
            return BadRequest("Payment data is null.");
        }

        try
        {
            var createdAccount = await _paymentService.CreatePaymentAsync(paymentServiceHelperDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] PaymentServiceForUpdateDto paymentServiceForUpdateDto)
    {
        if (paymentServiceForUpdateDto == null)
        {
            return BadRequest("Payment data is null.");
        }

        if (id != paymentServiceForUpdateDto.id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {paymentServiceForUpdateDto.id}.");
        }
        try
        {
            var updatedAccount = await _paymentService.UpdatePaymentAsync(paymentServiceForUpdateDto);
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
    public async Task<ActionResult> DeletePayment(int id)
    {
        try
        {
            await _paymentService.DeletePaymentAsync(id);
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
