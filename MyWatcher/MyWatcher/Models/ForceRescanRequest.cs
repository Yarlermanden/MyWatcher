using MyWatcher.Models.Enums;

namespace MyWatcher.Models
{
    public class ForceRescanRequest
    {
        public int UserId { get; set; }
        public Service Service {get;set;}
    }
}