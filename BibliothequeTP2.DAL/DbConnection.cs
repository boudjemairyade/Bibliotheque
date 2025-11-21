
using System.Data.SqlClient;   


using System.Data.SqlClient;


namespace BibliothequeTP2.DAL
{
    public class DbConnection
    {
        private readonly string _connectionString =
     "Server=localhost\\RIYADE;Database=BibliothequeDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection CreateConnection()
        {
            return new System.Data.SqlClient.SqlConnection(_connectionString);
        }

        
        public bool TestConnection()
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
                return false;
            }
        }
    }
}