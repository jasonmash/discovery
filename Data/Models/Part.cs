using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Discovery.Data
{
    public class Part
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PartRef { get; set; }
        public string Image { get; set; }
        public List<DefectCategory> DefectCategories { get; set; }
    }
}