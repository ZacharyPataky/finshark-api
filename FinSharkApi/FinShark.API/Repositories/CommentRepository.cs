using FinShark.API.Data;
using FinShark.API.Interfaces;
using FinShark.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.API.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task<Comment> CreateAsync(Comment commentModel)
    //{
    //    await _context.Comments.AddAsync(commentModel);
    //    await _context.SaveChangesAsync();
    //    return commentModel;
    //}

    //public async Task<Comment?> DeleteAsync(int id)
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FindAsync(id);
    }

    //public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto)
    //{
    //    throw new NotImplementedException();
    //}
}
