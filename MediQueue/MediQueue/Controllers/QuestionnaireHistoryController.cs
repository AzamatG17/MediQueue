using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/questionnairehistory")]
//[EnableCors("AllowSpecificOrigins")]
public class QuestionnaireHistoryController : ControllerBase
{
    private readonly IQuestionnaireHistoryService _questionnaireHistoryService;

    public QuestionnaireHistoryController(IQuestionnaireHistoryService questionnaireHistoryService)
    {
        _questionnaireHistoryService = questionnaireHistoryService ?? throw new ArgumentNullException(nameof(questionnaireHistoryService));
    }

    [PermissionAuthorize(10, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        try
        {
            var accounts = await _questionnaireHistoryService.GetAllQuestionnaireHistoriessAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(10, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        try
        {
            var account = await _questionnaireHistoryService.GetQuestionnaireHistoryByIdAsync(id);

            if (account is null)
                return NotFound($"QuestionnaireHistory with id: {id} does not exist.");

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

    [PermissionAuthorize(10, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        try
        {
            var createdAccount = await _questionnaireHistoryService.CreateQuestionnaireHistoryAsync(questionnaireHistoryForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(10, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto)
    {
        if (questionnaireHistoryForUpdateDto == null)
        {
            return BadRequest("QuestionnaireHistory data is null.");
        }

        if (id != questionnaireHistoryForUpdateDto.id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {questionnaireHistoryForUpdateDto.id}.");
        }
        try
        {
            var updatedAccount = await _questionnaireHistoryService.UpdateQuestionnaireHistoryAsync(questionnaireHistoryForUpdateDto);
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

    [PermissionAuthorize(10, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _questionnaireHistoryService.DeleteQuestionnaireHistoryAsync(id);
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
