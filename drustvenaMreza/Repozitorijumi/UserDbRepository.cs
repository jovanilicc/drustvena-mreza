using drustvenaMreza.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvenaMreza.Repozitorijumi
{
    public class UserDbRepository
    {
        private readonly string connectionString;

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
        public List<Korisnik> GetAll()
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
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
        public Korisnik GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
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
                Console.WriteLine("Greška pri radu sa bazom podataka. " + ex.Message);
              
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Greška u formatu podataka iz baze. " +ex.Message);
             
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Neispravna operacija nad bazom podataka. " +ex.Message);
                
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Došlo je do neočekivane greške. " + ex.Message);
               
                throw;
            }
        }

        public Korisnik Create(Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                //Username, Name, Surname, Birthday
                // Upit za umetanje novog korisnika i istovremeno vraćanje id-ja poslednjeg unetog reda
                string query = @"INSERT INTO Users (Username,Name,Surname,Birthday) VALUES (@Username, @Name,@Surname,@Birthday); SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                // Dodavanje parametara za korisnika
                command.Parameters.AddWithValue("@Username", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Name", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Surname", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@Birthday", noviKorisnik.DatumRodjenja);

                // Izvršavanje oba upita, gde ExecuteScalar vraća id poslednjeg ubačenog reda
                int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());  // Izvršava oba upita i vraća id
                Console.WriteLine($"ID poslednjeg unetog korisnika: {lastInsertedId}");
                noviKorisnik.Id = lastInsertedId;
                return noviKorisnik;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Greška pri radu sa bazom podataka. " +ex.Message);
                
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Greška u formatu podataka iz baze. " +ex.Message);
                
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Neispravna operacija nad bazom podataka. " +ex.Message);
               
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Došlo je do neočekivane greške. " +ex.Message);
                
                throw;
            }
        }
        public Korisnik Update(int id, Korisnik noviKorisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                //Username, Name, Surname, Birthday
                // Upit za menjanje novog korisnika i istovremeno vraćanje id-ja poslednjeg unetog reda
                string query = @"UPDATE Users 
                 SET Username = @Username,
                     Name = @Name,
                     Surname = @Surname,
                     Birthday = @Birthday
                 WHERE Id = @Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                // Dodavanje parametara za korisnika
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Username", noviKorisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@Name", noviKorisnik.Ime);
                command.Parameters.AddWithValue("@Surname", noviKorisnik.Prezime);
                command.Parameters.AddWithValue("@Birthday", noviKorisnik.DatumRodjenja);

                
                int rowsAffected = command.ExecuteNonQuery();  
                
                if(rowsAffected == 0)
                {
                    Console.WriteLine("Operacija izmene nije uspela." + rowsAffected);
                    return null;
                }
                noviKorisnik.Id = id;
                return noviKorisnik;
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

        public int Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                //Username, Name, Surname, Birthday
                // Upit za brisanje novog korisnika
                string query = @"DELETE FROM Users WHERE Id = @Id";

                

                using SqliteCommand command = new SqliteCommand(query, connection);

                

                // Dodavanje parametara za korisnika
                command.Parameters.AddWithValue("@Id", id);
           


                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("Operacija brisanja nije uspela." + rowsAffected);
                    return 0;
                }
               
                return 1;
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
