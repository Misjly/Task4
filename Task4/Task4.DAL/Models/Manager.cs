using System.Collections.Generic;

namespace Task4.DAL.Models
{
    public class Manager
    {
        public Manager()
        {
            Sales = new HashSet<Sale>();
        }
        public int Id { get; set; }
        public string SecondName { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}