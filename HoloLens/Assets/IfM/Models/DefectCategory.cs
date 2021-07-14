using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.IfM.Models
{
    public class DefectCategory
    {
        public int Id { get; set; }
        public Part Part { get; set; }
        public int PartId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
