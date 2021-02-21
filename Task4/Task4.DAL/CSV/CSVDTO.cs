using System;

namespace Task4.DAL.Csv
{
    public class CSVDTO
    {
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public decimal Cost { get; set; }
        public string SecondName { get; set; }
    }
}