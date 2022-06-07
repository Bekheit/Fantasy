namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
        public string PhoneNumber { get; set; }
        public PhotoDto Photo { get; set; }
    }
}
