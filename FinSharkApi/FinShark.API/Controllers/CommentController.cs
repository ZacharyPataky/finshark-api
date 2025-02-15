using FinShark.API.Interfaces;
using FinShark.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
        _commentRepo = commentRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();
        var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();
        return Ok(commentDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);
        return comment != null ?
            Ok(comment.ToCommentDto()) :
            NotFound();
    }
}
