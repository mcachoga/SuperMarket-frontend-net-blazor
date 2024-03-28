namespace SuperMarket.Client.Services.Endpoints
{
    public class ApiEndpoints
    {
        public string BaseApiUrl { get; set; }

        public UserEndpoints UserEndpoints { get; set; }

        public RoleEndpoints RoleEndpoints { get; set; }

        public AuthEndpoints AuthEndpoints { get; set; }

        public ProductEndpoints ProductEndpoints { get; set; }

        public OrderEndpoints OrderEndpoints { get; set; }

        public MarketEndpoints MarketEndpoints { get; set; }
    }

    public class UserEndpoints
    {
        public string Register { get; set; }
        public string GetById { get; set; }
        public string GetAll { get; set; }
        public string Update { get; set; }
        public string ChangePassword { get; set; }
        public string ChangeStatus { get; set; }
        public string GetRoles { get; set; }
        public string UpdateRoles { get; set; }

    }

    public class RoleEndpoints
    {
        public string Create { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string GetById { get; set; }
        public string GetAll { get; set; }
        public string GetPermissions { get; set; }
        public string UpdatePermissions { get; set; }
    }

    public class AuthEndpoints
    {
        public string GetToken { get; set; }
        public string GetRefreshToken { get; set; }
    }

    public class OrderEndpoints
    {
        public string Create { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string GetById { get; set; }
        public string GetAll { get; set; }
    }

    public class ProductEndpoints
    {
        public string Create { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string GetById { get; set; }
        public string GetAll { get; set; }
    }

    public class MarketEndpoints
    {
        public string Create { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }
        public string GetById { get; set; }
        public string GetAll { get; set; }
    }
}