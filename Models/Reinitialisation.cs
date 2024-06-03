using Npgsql;
using System;
using System.Collections.Generic;

namespace Course.Models
{
    public class Reinitialisation
    {
        public static void reset(NpgsqlConnection con = null)
        {
            try
            {
                if (con == null)
                {
                    con = Connect.connectDB();
                }
                TruncateTable(con);
                DeleteRows("Utilisateur", "id_utilisateur", 1, con);
                List<string> sequences = new List<string> {
                    "utilisateur_id_utilisateur_seq", "profil_id_profil_seq"
                };
                foreach (string sequence in sequences){
                    ResetSequence(sequence, con);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public static void TruncateTable(NpgsqlConnection con)
        {
            string sql = $"TRUNCATE TABLE PointCoureur, TempsCoureur, CategorieCoureur"+
                        ", Categorie, Point, Etape, Coureur, RESTART IDENTITY";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRows(string tableName, string columnName, int value, NpgsqlConnection con)
        {
            string sql = $"DELETE FROM {tableName} WHERE {columnName} != :value";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("value", value);
                cmd.ExecuteNonQuery();
            }
        }

        public static void ResetSequence(string sequenceName, NpgsqlConnection con)
        {
            string sql = $"ALTER SEQUENCE {sequenceName} RESTART WITH 2";
            Console.WriteLine(sql);
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
