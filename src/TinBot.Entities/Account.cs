using System.ComponentModel.DataAnnotations;

namespace TinBot.Entities
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public int AccountTypeId { get; set; }
        public int SchedulerId { get; set; }
        public AccountType AccountType { get; set; }
        public Scheduler Scheduler { get; set; }
    }
}
