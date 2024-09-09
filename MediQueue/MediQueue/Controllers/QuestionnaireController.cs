using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

//[Authorize(Policy = "Admin")]
[ApiController]
[Route("api/questionnaire")]
[EnableCors("AllowSpecificOrigins")]
public class QuestionnaireController : ControllerBase
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService ?? throw new ArgumentNullException(nameof(questionnaireService));
    }

    [HttpGet]
    public async Task<ActionResult> GetQuestionnairesAsync()
    {
        try
        {
            var accounts = await _questionnaireService.GetAllQuestionnairesAsync();
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetQuestionnaireByIdAsync(int id)
    {
        try
        {
            var account = await _questionnaireService.GetQuestionnaireByIdAsync(id);

            if (account is null)
                return NotFound($"Questionnaire with id: {id} does not exist.");

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
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireForCreateDto questionnaireForCreateDto)
    {
        if (questionnaireForCreateDto == null)
        {
            return BadRequest("Questionnaire data is null.");
        }

        try
        {
            var createdAccount = await _questionnaireService.CreateQuestionnaireAsync(questionnaireForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] QuestionnaireForUpdateDto questionnaireForUpdateDto)
    {
        if (questionnaireForUpdateDto == null)
        {
            return BadRequest("Questionnaire data is null.");
        }

        if (id != questionnaireForUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {questionnaireForUpdateDto.Id}.");
        }
        try
        {
            var updatedAccount = await _questionnaireService.UpdateQuestionnaireAsync(questionnaireForUpdateDto);
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
            await _questionnaireService.DeleteQuestionnaireAsync(id);
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
