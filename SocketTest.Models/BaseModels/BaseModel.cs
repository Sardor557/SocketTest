using System;

namespace SocketTest.Models.BaseModels
{
    public class BaseModel
    {
        public int CreateUser { get; set; }
        public int? UpdateUser { get; set; }
        public DateTime CreateDate {  get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
