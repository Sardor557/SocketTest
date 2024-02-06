using SocketTest.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocketTest.Models
{
    public class tbMessage : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }
                
        public int UserId { get; set; }
        public virtual tbUser User { get; set; }
    }
}
