using System;
using MyWatcher.Models.Enums;

namespace MyWatcher.Models
{
    public class ForceRescanRequest
    {
        public Guid UserId { get; set; }
        public Service Service {get;set;}
    }
}