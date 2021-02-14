using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task4.Model.Models
{
    public class Manager
    {
        public Manager()
        {
            Sales = new HashSet<Sale>();
        }
       // [Key]
        public int Id { get; set; }
        public string SecondName { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}