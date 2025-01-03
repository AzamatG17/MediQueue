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
        var accounts = await _questionnaireHistoryService.GetAllQuestionnaireHistoriessAsync(questionnaireHistoryResourceParametrs);

        return Ok(accounts);
    }

    [PermissionAuthorize(10, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _questionnaireHistoryService.GetQuestionnaireHistoryByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(10, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireHistoryForCreateDto questionnaireHistoryForCreateDto)
    {
        if (questionnaireHistoryForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("QuestionnaireHistory data is null."));
        }

        await _questionnaireHistoryService.CreateQuestionnaireHistoryAsync(questionnaireHistoryForCreateDto);

        return Ok(CreateSuccessResponse("QuestionnaireHistory successfully created."));
    }

    [PermissionAuthorize(10, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] QuestionnaireHistoryForUpdateDto questionnaireHistoryForUpdateDto)
    {
        if (questionnaireHistoryForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("QuestionnaireHistory data is null."));
        }

        if (id != questionnaireHistoryForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {questionnaireHistoryForUpdateDto.Id}."));
        }

        await _questionnaireHistoryService.UpdateQuestionnaireHistoryAsync(questionnaireHistoryForUpdateDto);

        return Ok(CreateSuccessResponse("QuestionnaireHistory successfully updated."));
    }

    [PermissionAuthorize(10, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _questionnaireHistoryService.DeleteQuestionnaireHistoryAsync(id);

        return Ok(CreateSuccessResponse("QuestionnaireHistory successfully deleted."));
    }
}
