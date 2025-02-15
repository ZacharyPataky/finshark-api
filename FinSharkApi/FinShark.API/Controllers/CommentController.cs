﻿using FinShark.API.DTOs;
using FinShark.API.DTOs.Comment;
using FinShark.API.Helpers;
using FinShark.API.Interfaces;
using FinShark.API.Mappers;
using FinShark.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.API.Controllers;

[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly IStockRepository _stockRepo;

    public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
    {
        _commentRepo = commentRepo;
        _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var comments = await _commentRepo.GetAllAsync();
        var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();
        return Ok(ApiResponse<List<CommentDto>>.SuccessResponse(commentDtos));
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

        var comment = await _commentRepo.GetByIdAsync(id);
        return comment != null ?
            Ok(ApiResponse<CommentDto>.SuccessResponse(comment.ToCommentDto())) :
            NotFound(ApiResponse<CommentDto>.FailureResponse("The comment does not exist."));
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        if (!await _stockRepo.StockExists(stockId))
            return BadRequest(ApiResponse<CommentDto>.FailureResponse("The stock does not exist."));

        var commentModel = commentDto.ToCommentFromCreateDto(stockId);
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ValidationHelpers.GetValidationErrors(ModelState);
            return BadRequest(ApiResponse<Dictionary<string, List<string>>>
                .FailureResponse("Invalid validation", errors));
        }

        var commentModel = await _commentRepo.UpdateAsync(id, updateDto.ToCommentFromUpdateDto());
        return commentModel != null ?
            Ok(ApiResponse<CommentDto>.SuccessResponse(commentModel.ToCommentDto())) :
            NotFound(ApiResponse<CommentDto>.FailureResponse("The comment does not exist."));
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

        var commentModel = await _commentRepo.DeleteAsync(id);
        return commentModel != null ?
            NoContent() :
            NotFound(ApiResponse<Comment>.FailureResponse("The comment does not exist."));
    }
}
