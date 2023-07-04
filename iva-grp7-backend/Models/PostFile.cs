using System.ComponentModel.DataAnnotations;

namespace iva_grp7_backend.Models;

    public class PostFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public byte[] Data { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
