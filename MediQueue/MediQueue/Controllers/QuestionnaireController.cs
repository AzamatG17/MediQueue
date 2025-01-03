using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Domain.ResourceParameters;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/questionnaire")]
//[EnableCors("AllowSpecificOrigins")]
public class QuestionnaireController : BaseController
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService ?? throw new ArgumentNullException(nameof(questionnaireService));
    }

    [PermissionAuthorize(9, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] QuestionnaireResourceParameters questionnaireResourceParameters)
    {
        var accounts = await _questionnaireService.GetAllQuestionnairesAsync(questionnaireResourceParameters);

        return Ok(accounts);
    }

    [PermissionAuthorize(9, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var account = await _questionnaireService.GetQuestionnaireByIdAsync(id);

        return Ok(account);
    }

    [PermissionAuthorize(9, 3)]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] QuestionnaireForCreateDto questionnaireForCreateDto)
    {
        if (questionnaireForCreateDto == null)
        {
            return BadRequest(CreateErrorResponse("Questionnaire data is null."));
        }

        await _questionnaireService.CreateOrGetBId(questionnaireForCreateDto);

        return Ok(CreateSuccessResponse("Questionnaire successfully created."));
    }

    [PermissionAuthorize(9, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] QuestionnaireForUpdateDto questionnaireForUpdateDto)
    {
        if (questionnaireForUpdateDto == null)
        {
            return BadRequest(CreateErrorResponse("Questionnaire data is null."));
        }

        if (id != questionnaireForUpdateDto.Id)
        {
            return BadRequest(CreateErrorResponse(
                $"Route id: {id} does not match with parameter id: {questionnaireForUpdateDto.Id}."));
        }

        await _questionnaireService.UpdateQuestionnaireAsync(questionnaireForUpdateDto);

        return Ok(CreateSuccessResponse("Questionnaire successfully updated."));
    }

    [PermissionAuthorize(9, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _questionnaireService.DeleteQuestionnaireAsync(id);

        return Ok(CreateSuccessResponse("Questionnaire successfully deleted."));
    }
}
