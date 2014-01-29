using System;
using System.ComponentModel.DataAnnotations;

namespace TodoListWeb.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}