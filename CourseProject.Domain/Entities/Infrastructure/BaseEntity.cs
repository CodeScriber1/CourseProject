using System.ComponentModel.DataAnnotations;
using System;

namespace CourseProject.Domain.Entities.Infrastructure
{
    public  class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
