using System;
namespace iva_grp7_backend.Models
{
	public class PostAdd
	{
		public string UserId { get; set; }
		public string Text { get; set; }
		public List<byte[]>? Files { get; set; }
	}
}

