namespace ProductsInventory.Persistence.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string GetAll = Base + "/users";
            public const string Update = Base + "/users";
            public const string Delete = Base + "/users/{userId}";
            public const string Get = Base + "/users/{userId}";
            public const string GetByUserEmail = Base + "/users/getbyemail/{email}";
            public const string Create = Base + "/users";
        }

        public static class Account
        {
            public const string Login = Base + "/account/login";
            public const string Logout = Base + "/account/logout";       
            public const string Register = Base + "/account/register";       
        }
    }
}
