using FinShark.API.Interfaces;
using FinShark.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/controller")]
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
}
