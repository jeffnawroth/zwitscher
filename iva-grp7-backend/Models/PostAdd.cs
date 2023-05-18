using System;
namespace iva_grp7_backend.Models
{
	public class PostAdd
	{
		public int UserId { get; set; }
		public string text { get; set; }
		public List<string> Files { get; set; }
	}
}

