using SocketTest.Models.BaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace SocketTest.Models
{
    public class tbUser : BaseModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50), IndexColumn(IsUnique = true)]
        public string Login {  get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public virtual List<tbMessage> Messages { get; set; }
    }
}
