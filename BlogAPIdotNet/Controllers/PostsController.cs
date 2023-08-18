using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApp.Controllers
{
    [ApiController]
    [Route("api/posts")]

    public partial class PostsController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public PostsController(IMemoryCache cache)
        {
            _cache = cache;
        }
        private List<BlogPost> GetPosts()
        {

            if (!_cache.TryGetValue("posts", out List<BlogPost> posts))
            {
                posts = new List<BlogPost>();
                _cache.Set("posts", posts, TimeSpan.FromMinutes(10));

            }
            return posts;
        }

        [HttpGet]
        public IActionResult GetPosts(string sort, string direction)
        {

            var posts = GetPosts();
            if (!string.IsNullOrEmpty(sort))
            {

                if (sort == "title")
                {
                    if (direction == "desc")
                    {
                        posts = posts.OrderByDescending(p => p.Title).ToList();
                    }
                    else
                    {
                        posts = posts.OrderBy(p => p.Title).ToList();
                    }
                }
                else if (sort == "content")
                {
                    if (direction == "desc")
                    {
                        posts = posts.OrderByDescending(p => p.Content)
                                     .ToList(); ;
                    }
                    else
                    {
                        posts = posts.OrderBy(p => p.Content).ToList();
                    }
                }
            }
            return Ok(posts);
        }

        [HttpPost]
        public IActionResult AddPost(BlogPost newPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var posts = GetPosts();
            if (posts.Any(p => p.Title == newPost.Title && p.Content == newPost.Content))
            {
                return Conflict(new { error = "Post already exists" });
            }

            var maxId = posts.Max(p => p.PostId);
            newPost.PostId = maxId + 1;
            posts.Add(newPost);
            _cache.Set("posts", posts);

            return CreatedAtAction(nameof(GetPosts), newPost);

        }

        [HttpPut("{post_id}")]
        public IActionResult UpdatePost(int post_id, BlogPost updatedPost)
        {
            var posts = GetPosts();
            var postToUpdate = posts.FirstOrDefault();

            if (postToUpdate == null)
            {
                return NotFound(new { error = "Post not found" });
            }

            postToUpdate.Title = updatedPost.Title;
            postToUpdate.Content = updatedPost.Content;
            _cache.Set("posts", posts);

            return Ok(new { message = $"Post with id {post_id} has been updated successfully." });
        }

        [HttpDelete("{postId")]
        public IActionResult DeletePost(int postId)
        {
            var posts = GetPosts();
            var postToDelete = posts.FirstOrDefault(p => p.PostId == postId);

            if (postToDelete == null)
            {
                return NotFound(new { error = "Post not found" });

            }

            posts.Remove(postToDelete);
            _cache.Set("posts", posts);

            return Ok(new { message = $"Post with id {postId} has been successfully deleted." });
        }
    }
}