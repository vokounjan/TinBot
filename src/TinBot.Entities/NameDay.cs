using System;
using System.ComponentModel.DataAnnotations;

namespace TinBot.Entities
{
    public class NameDay
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
