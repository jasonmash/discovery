using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discovery.Data
{
    public class Defect
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Part Part { get; set; }
        public int PartId { get; set; }
        public DefectCategory Category { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }
    }
}