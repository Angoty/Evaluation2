using Npgsql;
using System;

namespace Course.Models
{
    public class Connect
    {
        public static NpgsqlConnection connectDB()
        {
            var server = "localhost";
            var port = "5432";
            var database = "course_a_pied3";
            var username = "postgres";  
            var password = "mdpprom15";

            var connString = $"Host={server};Port={port};Database={database};Username={username};Password={password}";
            NpgsqlConnection conn = new NpgsqlConnection(connString);

            try{
                Console.WriteLine("Ouverture de la connexion...");
                conn.Open();
                Console.WriteLine("Connexion réussie !");
            }
            catch (Exception e){
                throw e;
            }
            
            return conn;
        }
    }
}
