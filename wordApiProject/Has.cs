using System.ComponentModel.DataAnnotations.Schema;

namespace wordApiProject
{
    public class Has
    {

        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Word")]    
        public int WordId { get; set; }
    }
}
