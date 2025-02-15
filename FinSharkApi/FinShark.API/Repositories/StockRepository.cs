using FinShark.API.Data;
using FinShark.API.DTOs.Stock;
using FinShark.API.Helpers;
using FinShark.API.Interfaces;
using FinShark.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.API.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FindAsync(id);

        if (stockModel == null)
            return null;

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks
            .Include(c => c.Comments)
            .ThenInclude(a => a.AppUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

        if (!string.IsNullOrWhiteSpace(query.Symbol))
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                stocks = query.IsDescending ?
                    stocks.OrderByDescending(s => s.Symbol) :
                    stocks.OrderBy(s => s.Symbol);
            else if (query.SortBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                stocks = query.IsDescending ?
                    stocks.OrderByDescending(s => s.CompanyName) :
                    stocks.OrderBy(s => s.CompanyName);
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocks
            .Skip(skipNumber)
            .Take(query.PageSize)
            .ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks
            .Include(c => c.Comments)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto)
    {
        var stockModel = await _context.Stocks.FindAsync(id);

        if (stockModel == null)
            return null;

        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase = stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();
        return stockModel;
    }
}
