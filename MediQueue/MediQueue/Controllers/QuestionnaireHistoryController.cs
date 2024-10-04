using MediQueue.Domain.DTOs.QuestionnaireHistory;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/questionnairehistory")]
//[EnableCors("AllowSpecificOrigins")]
public class QuestionnaireHistoryController : BaseController
{
    private readonly IQuestionnaireHistoryService _questionnaireHistoryService;

    public QuestionnaireHistoryController(IQuestionnaireHistoryService questionnaireHistoryService)
    {
        _questionnaireHistoryService = questionnaireHistoryService ?? throw new ArgumentNullException(nameof(questionnaireHistoryService));
    }

    [PermissionAuthorize(10, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] QuestionnaireHistoryResourceParametrs questionnaireHistoryResourceParametrs)
    {
        try
        {
            var accounts = await _questionnaireHistoryService.GetAllQuestionnaireHistoriessAsync(questionnaireHistoryResourceParametrs);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return HandleError(ex);
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
                return NotFound(CreateErrorResponse($"QuestionnaireHistory with id: {id} does not exist."));

            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", QuestionnaireHistory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(10, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        if (questionnaireHistoryForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("QuestionnaireHistory data is null."));
        }

        try
        {
            var createdAccount = await _questionnaireHistoryService.CreateQuestionnaireHistoryAsync(questionnaireHistoryForCreateDto);
            return Ok(CreateSuccessResponse("QuestionnaireHistory successfully created."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(10, 4)]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto)
    {
        if (questionnaireHistoryForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("QuestionnaireHistory data is null."));
        }

        if (id != questionnaireHistoryForUpdateDto.id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {questionnaireHistoryForUpdateDto.id}."));
        }
        try
        {
            var updatedAccount = await _questionnaireHistoryService.UpdateQuestionnaireHistoryAsync(questionnaireHistoryForUpdateDto);
            return Ok(CreateSuccessResponse("QuestionnaireHistory successfully updated."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", QuestionnaireHistory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    [PermissionAuthorize(10, 5)]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            await _questionnaireHistoryService.DeleteQuestionnaireHistoryAsync(id);
            return Ok(CreateSuccessResponse("QuestionnaireHistory successfully deleted."));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(CreateErrorResponse(ex.Message + ", QuestionnaireHistory not found."));
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }
}
