using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogApp.Models
{
    [PrimaryKey(propertyName: "PostId")]
    public class BlogPost
    {
        [JsonPropertyName("post_id")]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}