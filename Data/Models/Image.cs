using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discovery.Data
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Part Part { get; set; }
        public int? PartId { get; set; }
        public DefectCategory Category { get; set; }
        public int? CategoryId { get; set; }
        public Defect Defect { get; set; }
        public int? DefectId { get; set; }

        public DateTime Timestamp { get; set; }
        public string ImageData { get; set; }
    }
}