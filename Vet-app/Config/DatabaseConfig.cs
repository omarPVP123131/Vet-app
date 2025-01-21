using System.Configuration;

namespace VeterinaryManagementSystem.Config
{
    public static class DatabaseConfig
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["VeterinaryDB"]?.ConnectionString ??
            "Data Source=VeterinaryDB.db;Version=3;";
    }

}
