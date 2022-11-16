using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Data.Data.Interfaces;


public interface IBlogRepository
{
    Task<IdentityResult> CreatePostAsync(PostVM model);
    Task<List<PostVM>?> GetPostWithTagsAndFilterAsync(,GetPostVM getPostVm);
    
    Task<List<PostVM>?> GetPostAsync(int modelPage);
    Task<List<PostVM>?> GetPostWithTagsAsync(int modelPage, List<string> modelTags);
    Task<List<PostVM>?> GetPostWithFilterAsync(int modelPage, string? modelFind);
}