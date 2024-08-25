using System.ComponentModel.DataAnnotations;

namespace WebApi.DAL
{
    public class SendMessageModel
    {
        [Required]
        public int Index { get; set; }

        [Required]
        [StringLength(128)]
        public string Text { get; set; }
    }
}
