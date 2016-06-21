namespace AuthenticationAuthorization.Models.Viewmodels
{
    public class PrincipalModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
    }
}