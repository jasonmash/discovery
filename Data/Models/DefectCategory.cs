using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Discovery.Data
{
    public class DefectCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public Part Part { get; set; }
        
        public int PartId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}