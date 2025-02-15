using FinShark.API.DTOs;
using FinShark.API.Extensions;
using FinShark.API.Interfaces;
using FinShark.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;

    public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo,
        IPortfolioRepository portfolioRepo)
    {
        _userManager = userManager;
        _stockRepo = stockRepo;
        _portfolioRepo = portfolioRepo;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
        return Ok(ApiResponse<List<Stock>>.SuccessResponse(userPortfolio));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepo.GetBySymbolAsync(symbol);

        if (stock == null)
            NotFound(ApiResponse<Stock>.FailureResponse("The stock does not exist."));

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            BadRequest(ApiResponse<Stock>.FailureResponse("The stock already exists in the portfolio."));

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id,
        };

        portfolioModel = await _portfolioRepo.CreateAsync(portfolioModel);

        return portfolioModel != null ?
            Ok(ApiResponse<Portfolio>.SuccessResponse(portfolioModel)) :
            BadRequest(ApiResponse<Portfolio>.FailureResponse("Failed to generate the portfolio"));
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredStock.Count == 1)
        {
            var portfolioModel = await _portfolioRepo.DeleteAsync(appUser, symbol);
            return Ok(ApiResponse<Portfolio>.SuccessResponse(portfolioModel));
        }

        return BadRequest(ApiResponse<Portfolio>.FailureResponse("The stock isn't in the portfolio."));

    }
}
