using System;

namespace Task4.Model.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public int Cost { get; set; }
        public virtual Manager Manager { get; set; }
        public Guid? Session { get; set; }
    }
}