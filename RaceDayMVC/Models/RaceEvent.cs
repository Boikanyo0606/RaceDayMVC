using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class RaceEvent
    {
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; }

        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public int OrganiserID { get; set; }
        public string BannerUrl { get; set; }
    }
}