using System.ComponentModel.DataAnnotations;

namespace SampleAPI.Entities
{
    public class AddOrderDto
    {
        public DateTime EntryDate { get; set; }

        [MaxLength(100)]
        public string? DescriptionName { get; set; }
    }
}
