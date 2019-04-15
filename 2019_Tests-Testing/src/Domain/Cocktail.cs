using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Cocktail
    {
        public Cocktail(int commentsCount)
        {
            Comments = new List<string>(commentsCount);
        }
        public List<string> Comments { get; set; }
        public Cocktail() { }
        public string Name { get; set; }
        public double Size { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string Instructions { get; set; }        
    }
}
