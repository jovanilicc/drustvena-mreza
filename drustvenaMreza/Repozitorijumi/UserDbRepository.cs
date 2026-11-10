using drustvenaMreza.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvenaMreza.Repozitorijumi
{
    public class UserDbRepository
    {

        public List<Korisnik> GetAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/drustvenamreza.db");
                connection.Open();
                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);
                using SqliteDataReader reader = command.ExecuteReader();

                List<Korisnik> results = new List<Korisnik>();
                while (reader.Read())
                {
                    //Username, Name, Surname, Birthday
                    Korisnik korisnik = new Korisnik
                    (
                        Convert.ToInt32(reader["Id"]),
                        reader["Username"].ToString(),
                        reader["Name"].ToString(),
                        reader["Surname"].ToString(),
                        DateTime.Parse(reader["Birthday"].ToString())

                    );
                    results.Add(korisnik);
                }
                return results;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Greška pri radu sa bazom podataka.";
                return null;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Greška u formatu podataka iz baze.";
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Neispravna operacija nad bazom podataka.";
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return "Došlo je do neočekivane greške.";
                return null;
            }

        }
        public Korisnik GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=database/drustvenamreza.db");
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);
                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
            
                    Korisnik korisnik = new Korisnik
                   (
                       Convert.ToInt32(reader["Id"]),
                       reader["Username"].ToString(),
                       reader["Name"].ToString(),
                       reader["Surname"].ToString(),
                       DateTime.Parse(reader["Birthday"].ToString())

                   );
                    return korisnik;
                }
                return null; // Dešava se ako ne postoji korisnik sa datim id
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Greška pri radu sa bazom podataka.";
                return null;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Greška u formatu podataka iz baze.";
                return null;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                //return "Neispravna operacija nad bazom podataka.";
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return "Došlo je do neočekivane greške.";
                return null;
            }
        }
    }
    



    }
