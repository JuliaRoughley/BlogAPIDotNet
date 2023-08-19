using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/posts")]

    public partial class PostsController : ControllerBase
    {
        private readonly BlogApiContext _dbContext;

        public PostsController(BlogApiContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public IActionResult GetPosts(string? sort = null, string? direction = null)
        {

            IQueryable<BlogPost> posts = _dbContext.BlogPosts;
            if (!string.IsNullOrEmpty(sort))
            {

                if (sort == "title")
                {
                    if (direction == "desc")
                    {
                        posts = posts.OrderByDescending(p => p.Title);
                    }
                    else
                    {
                        posts = posts.OrderBy(p => p.Title);
                    }
                }
                else if (sort == "content")
                {
                    if (direction == "desc")
                    {
                        posts = posts.OrderByDescending(p => p.Content);
                    }
                    else
                    {
                        posts = posts.OrderBy(p => p.Content);
                    }
                }
            }
            return Ok(posts.ToList());
        }

        [HttpPost]
        public IActionResult AddPost(BlogPost newPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(newPost.Title) || string.IsNullOrEmpty(newPost.Content))
            {
                return BadRequest(new { error = "Title and content cannot be empty" });
            }

            var posts = _dbContext.BlogPosts;

            if (posts.Any(p => p.Title == newPost.Title && p.Content == newPost.Content))
            {
                return Conflict(new { error = "Post already exists" });
            }

            _dbContext.BlogPosts.Add(newPost);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetPosts), newPost);

        }

        [HttpPut("{post_id}")]
        public IActionResult UpdatePost(int post_id, BlogPost updatedPost)
        {
            var postToUpdate = _dbContext.BlogPosts.Find(post_id);

            if (postToUpdate == null)
            {
                return NotFound(new { error = "Post not found" });
            }

            postToUpdate.Title = updatedPost.Title;
            postToUpdate.Content = updatedPost.Content;
            _dbContext.SaveChanges();

            return Ok(new { message = $"Post with id {post_id} has been updated successfully." });
        }

        [HttpDelete("{post_id}")]
        public IActionResult DeletePost(int post_id)
        {

            var postToDelete = _dbContext.BlogPosts.Find(post_id);

            if (postToDelete == null)
            {
                return NotFound(new { error = "Post not found" });

            }

            _dbContext.BlogPosts.Remove(postToDelete);
            _dbContext.SaveChanges();

            return Ok(new { message = $"Post with id {post_id} has been successfully deleted." });
        }
    }
}