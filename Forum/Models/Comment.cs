namespace Forum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [Key]
        public int id_comment { get; set; }

        public int id_post { get; set; }

        public int id_user { get; set; }

        public DateTime? created_at { get; set; }

        [StringLength(4000)]
        public string content_comment { get; set; }

        [StringLength(100)]
        public string status { get; set; }

        public int? like { get; set; }

        public virtual Post Post { get; set; }

        public virtual Post Post1 { get; set; }

        public virtual Post Post2 { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public virtual User User2 { get; set; }
    }
}
