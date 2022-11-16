using AutoMapper;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models.ViewModels;
using Dashboard.Data.Validation;

using Microsoft.Extensions.Configuration;

namespace Dashboard.Services;

public class BlogService
{
    
    private readonly IBlogRepository _blogRepository;

    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<ServiceResponse> CreatePostAsync(PostVM model)
    {
        var validator = new BlogValidator();
        var validationResult = await validator.ValidateAsync(model);
        if (validationResult.IsValid)
        {
            var result = await _blogRepository.CreatePostAsync(model);
            if (result.Succeeded) 
            {
                return new ServiceResponse
                {
                    IsSuccess = true,
                    Message = "Post successfully created"
                };
            }
            return new ServiceResponse
            {
                Message = "Post create error created",
                IsSuccess = false
            };
        }
        else
        {
            return new ServiceResponse
            {
                Message = "Some validation error occured : ",
                Errors = validationResult.Errors.Select(i => i.ErrorCode + "  :  " + i.ErrorMessage),
                IsSuccess = false
            };
        }
    }

    public async Task<ServiceResponse> GetPostAsync(GetPostVM model)
    {
        List<PostVM>? result;

        try
        {
            if (model.Tags != null)
            {
                if (model.Find != null)
                {
                    //search with tags and filter
                    result   =await _blogRepository.GetPostWithTagsAndFilterAsync(model);
                }
                else
                {
                    //search with tags 
                    result   =await _blogRepository.GetPostWithTagsAsync(model.page, model.Tags);
                }
            }
            else
            {
                if (model.Find != null)
                {
                    //search with filter 
                    result   =await _blogRepository.GetPostWithFilterAsync(model.page, model.Find);
                }
            
            }
            result   =await _blogRepository.GetPostAsync(model.page);
            return new ServiceResponse()
            {
                Message = "Posts received",
                IsSuccess = true,
                Payload = result
            };
        }
        catch (Exception e)
        {

            return new ServiceResponse()
            {
                Message = "Some error occured",
                IsSuccess = false
            };
        }
        
    }
}