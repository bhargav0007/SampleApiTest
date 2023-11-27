using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime EntryDate { get; set; }

        [MaxLength(100)]
        public string? DescriptionName { get; set; }

        public bool Status { get; set; } = true; // Setting default value as true
    }
}
