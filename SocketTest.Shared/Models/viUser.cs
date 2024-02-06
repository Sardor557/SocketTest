namespace SocketTest.Shared.Models
{
    public sealed class viUser
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public viUser(int id, string token)
        {
            Id = id;
            Token = token;
        }

        public viUser()
        {
            
        }
    }
}
