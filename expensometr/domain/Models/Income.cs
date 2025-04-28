using expense_service.domain.enums;
using System.ComponentModel.DataAnnotations;

namespace expense_service.domain.Models
{
    public class Income
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public IncomeType Type { get; init; }
        [Required]
        public int Value { get; set; }
        [Required]
        public int RelatedUserId { get; init; }
        [Required]
        public DateTime Created {  get; init; }
        public string? Description { get; set; }


        public Income(int id, IncomeType type, int value, string? description, int RelatedId, DateTime created)
        {
            this.Id = id;
            this.Type = type;
            this.Value = value;
            this.Description = description;
            this.RelatedUserId = RelatedId;
            this.Created = created;
        }
    }
}
