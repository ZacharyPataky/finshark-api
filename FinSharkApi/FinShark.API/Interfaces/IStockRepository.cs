using FinShark.API.DTOs.Stock;
using FinShark.API.Helpers;
using FinShark.API.Models;

namespace FinShark.API.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
