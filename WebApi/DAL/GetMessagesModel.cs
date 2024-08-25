using System.ComponentModel.DataAnnotations;

namespace WebApi.DAL
{
    public class GetMessagesModel
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
