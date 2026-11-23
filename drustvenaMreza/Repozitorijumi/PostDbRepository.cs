using drustvenaMreza.Model;
using Microsoft.Data.Sqlite;

namespace drustvenaMreza.Repozitorijumi
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        
        public List<Objava> GetAll()
        {
           
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = @"SELECT Posts.Id, Posts.Content, Posts.Date, Posts.UserId, Users.Username, Users.Name, Users.Surname, Users.Birthday FROM Posts LEFT JOIN Users ON Posts.UserId = Users.Id;";
                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                List<Objava> objave = new List<Objava>();
                while (reader.Read())
                {
                    
                    Objava objava = new Objava
                    (
                        Convert.ToInt32(reader["Id"]),
                        reader["Content"].ToString(),
                        DateTime.Parse(reader["Date"].ToString()),
                        Convert.ToInt32(reader["UserId"])

                    );
                    objave.Add(objava);
                    
                    if (reader["UserId"] != DBNull.Value)
                    {
                        Korisnik korisnik = new Korisnik
                        (
                        Convert.ToInt32(reader["UserId"]),
                        reader["UserName"].ToString(),
                        reader["Name"].ToString(),
                        reader["Surname"].ToString(),
                        DateTime.Parse(reader["Date"].ToString())
                    );
                        objava.User = korisnik;
                    }
                    

                }
                
                return objave;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Greška pri radu sa bazom podataka. " + ex.Message);

                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Greška u formatu podataka iz baze. " + ex.Message);

                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Neispravna operacija nad bazom podataka. " + ex.Message);

                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Došlo je do neočekivane greške. " + ex.Message);

                throw;
            }

        }
    }
}
