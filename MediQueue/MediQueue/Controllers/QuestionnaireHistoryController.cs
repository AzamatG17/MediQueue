using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "AllQuestionnaire")]
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

    [HttpGet]
    public async Task<ActionResult> GetQuestionnaireHistoriesAsync()
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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuestionnaireHistoryByIdAsync(int id)
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGroup(int id)
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
