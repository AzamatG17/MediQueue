using MediQueue.Domain.DTOs.AnalysisResult;
using MediQueue.Domain.Entities.Responses;
using MediQueue.Domain.Interfaces.Services;
using MediQueue.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediQueue.Controllers;

[Authorize(Policy = "HasPermission")]
[ApiController]
[Route("api/analysis")]
//[EnableCors("AllowSpecificOrigins")] 
public class AnalysisResultController : BaseController
{
    private readonly IAnalysisResultService _analysisResultService;

    public AnalysisResultController(IAnalysisResultService analysisResultService)
    {
        _analysisResultService = analysisResultService ?? throw new ArgumentNullException(nameof(analysisResultService));
    }

    [PermissionAuthorize(1, 1)]
    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
        var analysisResults = await _analysisResultService.GetAllAnalysisResultsAsync();

        return Ok(analysisResults);
    }

    [PermissionAuthorize(1, 2)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
        var analysisResult = await _analysisResultService.GetAnalysisResultByIdAsync(id);

        return Ok(analysisResult);
    }

    [PermissionAuthorize(1, 3)]
    [HttpPost]
    public async Task<ActionResult<ReturnResponse>> PostAsync([FromBody] AnalysisResultForCreateDto analysisResultForCreateDto)
    {
        var accountIdFromToken = User.FindFirst(ClaimTypes.Name)?.Value;

        if (accountIdFromToken != analysisResultForCreateDto.AccountId.ToString())
        {
            return BadRequest(CreateErrorResponse("You can only write a conclusion in your own name."));
        }

        if (analysisResultForCreateDto == null)
            return BadRequest(CreateErrorResponse("AnalysisResult data is null."));

        await _analysisResultService.CreateAnalysisResultAsync(analysisResultForCreateDto);

        return Ok(CreateSuccessResponse("AnalysisResult successfully created."));
    }

    [PermissionAuthorize(1, 4)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<ReturnResponse>> PutAsync(int id, [FromBody] AnalysisResultForUpdateDto analysisResultForUpdateDto)
    {
        if (analysisResultForUpdateDto == null)
            return BadRequest(CreateErrorResponse("AnalysisResult data is null."));

        if (id != analysisResultForUpdateDto.Id)
            return BadRequest(CreateErrorResponse($"Route id: {id} does not match with parameter id: {analysisResultForUpdateDto.Id}."));

        await _analysisResultService.UpdateAnalysisResultAsync(analysisResultForUpdateDto);

        return Ok(CreateSuccessResponse("AnalysisResult successfully updated."));
    }

    [PermissionAuthorize(1, 5)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<ReturnResponse>> DeleteAsync(int id)
    {
        await _analysisResultService.DeleteAnalysisResultAsync(id);

        return Ok(CreateSuccessResponse("AnalysisResult successfully deleted."));
    }
}
