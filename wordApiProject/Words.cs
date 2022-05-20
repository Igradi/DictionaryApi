using System.ComponentModel.DataAnnotations.Schema;

namespace wordApiProject
{
    public class Words
    {

        public int Id { get; set; }
        public string WordName { get; set; } = String.Empty;
        [ForeignKey("Has")]
        public int HasId { get; set; }
    }
}