using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace GloryS.LinqSample.DAL.DataEntities
{
    public class CourseRes
    {
        [Key]
        [Column(Order = 1)]
        public int CourseID { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "int")]
        public Culture Culture { get; set; }

        public string Title { get; set; }

        public virtual Course Course { get; set; }
    }
}
