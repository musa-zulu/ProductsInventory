namespace ProductsInventory.Persistence.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Categories
        {
            public const string GetAll = Base + "/categories";
            public const string Update = Base + "/categories";
            public const string Delete = Base + "/categories/{categoryId}";
            public const string Get = Base + "/categories/{categoryId}";            
            public const string Create = Base + "/categories";
        }

        public static class Account
        {
            public const string Login = Base + "/account/login";
            public const string Logout = Base + "/account/logout";       
            public const string Register = Base + "/account/register";       
        }
    }
}
