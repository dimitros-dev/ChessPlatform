using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChessPlatform.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsStarted { get; set; }
        [BindNever]
        public string CreatedById { get; set; }
        [BindNever]
        public ApplicationUser CreatedBy { get; set; }
    }
}
