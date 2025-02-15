using FinShark.API.DTOs.Stock;
using FinShark.API.Interfaces;
using FinShark.API.Mappers;
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
        var stocks = await _stockRepo.GetAllAsync();
        var stockDtos = stocks.Select(s => s.ToStockDto()).ToList();
        return Ok(stockDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockRepo.GetByIdAsync(id);
        return stock != null ?
            Ok(stock.ToStockDto()) :
            NotFound();
    }

    [HttpPost()]
    public async Task<IActionResult> Create([FromBody] CreateStockDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
        return stockModel != null ?
            Ok(stockModel.ToStockDto()) :
            NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockRepo.DeleteAsync(id);
        return stockModel != null ?
            NoContent() :
            NotFound();
    }
}
