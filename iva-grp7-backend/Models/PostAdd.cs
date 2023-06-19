using System;
namespace iva_grp7_backend.Models
{
	public class PostAdd
	{
		public string UserId { get; set; }
		public string text { get; set; }
		public List<byte[]>? Files { get; set; }
	}
}

