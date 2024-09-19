using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/questionnaire")]
//[EnableCors("AllowSpecificOrigins")]
public class QuestionnaireController : ControllerBase
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService ?? throw new ArgumentNullException(nameof(questionnaireService));
    }

    [PermissionAuthorize(9, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
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

    [PermissionAuthorize(9, 2)]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
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

    [PermissionAuthorize(9, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireForCreateDto questionnaireForCreateDto)
    {
        try
        {
            var createdAccount = await _questionnaireService.CreateOrGetBId(questionnaireForCreateDto);
            return Ok(createdAccount);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [PermissionAuthorize(9, 4)]
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

    [PermissionAuthorize(9, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
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
