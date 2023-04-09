namespace Forum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_comment { get; set; }

        [StringLength(255)]
        public string reason { get; set; }

        [StringLength(10)]
        public string flag { get; set; }
    }
}
