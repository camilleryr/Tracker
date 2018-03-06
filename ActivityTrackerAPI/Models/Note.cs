
using System.ComponentModel.DataAnnotations;

namespace TrackerAPI.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        public string Content { get; set; }

        //foreign key
        public int ActivityId { get; set; }
        //navigation property
        public Activity Activity { get; set; }


    }
}