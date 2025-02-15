using FinShark.API.DTOs;
using FinShark.API.DTOs.Stock;
using FinShark.API.Helpers;
using FinShark.API.Interfaces;
using FinShark.API.Mappers;
using FinShark.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/stock")]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepo;

    public StockController(IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if(!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var stocks = await _stockRepo.GetAllAsync();
        var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();
        return Ok(ApiResponse<List<StockDto>>.SuccessResponse(stockDtos));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var stock = await _stockRepo.GetByIdAsync(id);
        return stock != null ?
            Ok(ApiResponse<StockDto>.SuccessResponse(stock.ToStockDto())) :
            NotFound(ApiResponse<StockDto>.FailureResponse("The stock does not exist."));
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var stockModel = stockDto.ToStockFromCreateDto();
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
        return stockModel != null ?
            Ok(ApiResponse<StockDto>.SuccessResponse(stockModel.ToStockDto())) :
            NotFound(ApiResponse<StockDto>.FailureResponse("The stock does not exist."));
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var stockModel = await _stockRepo.DeleteAsync(id);
        return stockModel != null ?
            NoContent() :
            NotFound(ApiResponse<Stock>.FailureResponse("The stock does not exist."));
    }
}
