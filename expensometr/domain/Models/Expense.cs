using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace expense_service.domain.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; init; }
        [Required]
        public ExpenseType Type { get; init; }
        [Required]
        public int Value { get; set; }
        [Required]
        public int RelatedUserId { get; init; }
        [Required]
        public DateTime Created { get; init; }
        public string? Description { get; set; }


        public Expense(int id, ExpenseType type, int value, string? description, int relatedUserId, DateTime created)
        {
            this.Id = id;
            this.Type = type;
            this.Value = value;
            this.Description = description;
            this.RelatedUserId = relatedUserId;
            this.Created = created;
        }
    }
}
