﻿using FinShark.API.DTOs.Comment;
using FinShark.API.Models;

namespace FinShark.API.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            CreatedBy = commentModel.AppUser.UserName,
            StockId = commentModel.StockId,
        };
    }

    public static Comment ToCommentFromCreateDto(this CreateCommentDto commentDto, int stockId)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content,
            StockId = stockId
        };
    }

    public static Comment ToCommentFromUpdateDto(this UpdateCommentDto commentDto)
    {
        return new Comment
        {
            Title = commentDto.Title,
            Content = commentDto.Content
        };
    }
}
