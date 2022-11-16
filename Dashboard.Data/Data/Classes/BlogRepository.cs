using Dashboard.Data.Data.Context;
using Dashboard.Data.Data.Interfaces;
using Dashboard.Data.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dashboard.Data.Data.Classes;

public class BlogRepository : IBlogRepository
{
    public async Task<IdentityResult> CreatePostAsync(PostVM model)
    {
        await using var context = new AppDbContext();
        var modelTags = model.Tags;
        List<Tag> tags = new List<Tag>();
        foreach (var tag in modelTags)
        {
            Tag? select = await context.Tags.FirstOrDefaultAsync(i => i.Name == tag) ?? null;
            if (select == null)
            {
                select = new Tag() { Name = tag };
                await context.Tags.AddAsync(select);
                await context.SaveChangesAsync();
            }

            tags.Add(select);
        }

        Post post = new Post
        {
            Title = model.Title,
            ShortDescription = model.ShortDescription,
            Description = model.Description,
            Body = model.Body,
            Author = model.Author,
            PostedOn = DateTime.Now,
            Tags = tags
        };

        try
        {
            var result = await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            return IdentityResult.Failed();
        }
    }

    public async  Task<List<PostVM>?> GetPostWithTagsAndFilterAsync(GetPostVM getPostVm)
    { 
      var res1 = await  GetPostWithFilterAsync(getPostVm.page, getPostVm.Find);
      var result = new List<PostVM>();
      foreach (var post in res1)
      {
          int count = getPostVm.Tags.Count;
          foreach (var tag in post.Tags)
          {
              if ( getPostVm.Tags.Contains(tag))
              {
                  count--;
              }

              if (count == 0)
              {
                  result.Add(post);
              }
          }
      }
    }

    public List<PostVM> GetPostsFromResult(List<Post> result, int modelPage)
    {
        int start = (modelPage > 0 ? modelPage - 1 : 0) * 5;
        List<PostVM> posts = new List<PostVM>();
        foreach (var post in result.Skip(start).Take(5))
        {
            posts.Add(new PostVM
            {
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Description = post.Description,
                Body = post.Body,
                Author = post.Author,
               
                Tags = post.Tags.Select(i => i.Name).ToList()
            });
        }

        return posts;
    }

    public async Task<List<PostVM>?> GetPostAsync(int modelPage)
    {
       


        using var context = new AppDbContext();
        var res =await  context.Posts.AsNoTracking().ToListAsync();
        return GetPostsFromResult(res, modelPage);
    }

    public async Task<List<PostVM>?> GetPostWithTagsAsync(int modelPage, List<string> modelTags)
    {
      
        using var context = new AppDbContext();
        List<Tag> tags = new List<Tag>();
        foreach (var tag in modelTags)
        {
            Tag? select = await context.Tags.FirstOrDefaultAsync(i => i.Name == tag) ?? null;
            if (select != null)
            {
                tags.Add(select);
            }
        }

        List<Post> posts = new List<Post>();
      await context.Posts.ForEachAsync(post =>
          {
              int count = tags.Count;
              foreach (var tag in post.Tags)
              {
                  if (tags.Contains(tag))
                  {
                      count--;
                  }

                  if (count == 0)
                  {
                      posts.Add(post);
                  }
              }
          }
    );

      return GetPostsFromResult(posts, modelPage);
    }

    public async Task<List<PostVM>?> GetPostWithFilterAsync(int modelPage, string? modelFind)
    {
        using var context = new AppDbContext();
       var res = await context.Posts.Where(i =>i.Author.Contains(modelFind)|| i.Title.Contains(modelFind) ).ToListAsync();
       return GetPostsFromResult(res, modelPage);
    }
}