using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodies.Models
{
	public class Branch
	{
		public int BranchId {  get; set; }
		[Required]
		public virtual Restaurant Restaurant { get; set; }
		[Required()]
		public string BranchArea { get; set; }
		[Required()]
		public string BranchLocation {  get; set; } //link
		[Column(TypeName = "time")]
		public string OpeningHour {  get; set; }//0-23
		[Column(TypeName = "time")]
		public string ClosingHour {  get; set; }	
	}
}
