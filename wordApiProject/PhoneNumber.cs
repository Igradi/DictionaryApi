using System.ComponentModel.DataAnnotations.Schema;

namespace wordApiProject
{
    [Keyless]
    public class PhoneNumber
    {
        [ForeignKey("User")]
        public int UserID { get; set; }
        public string Number { get; set; } = string.Empty;

    }
}
