using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;



namespace Tetris
{
    public class ClasificationDB
    {
        public static void AddScoreToDb(string name, int score)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=clasification.db; Version=3;"))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand(@"insert into CLASIFICATION(CLA_Name, CLA_Score) values('" + name + "'," + score + ");", conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void DeleteLastScoreFromDb()
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=clasification.db; Version=3;"))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM CLASIFICATION;", conn);

                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {

                    while (sdr.Read())
                    {

                        if (Convert.ToInt32(sdr[0]) >= 10)
                        {
                            cmd = new SQLiteCommand("delete from CLASIFICATION where CLA_Score=" + GetWorstScoreResul() + ";", conn);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                conn.Close();
            }
        }

        public static BindingSource GetAllClasification()
        {
            BindingSource bSource = new BindingSource();
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=clasification.db; Version=3;"))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("select CLA_Name as Imie, CLA_Score as Punkty from CLASIFICATION order by CLA_Score desc", conn);

                SQLiteDataAdapter sda = new SQLiteDataAdapter();

                sda.SelectCommand = cmd;

                DataTable dbdataset = new DataTable();
                sda.Fill(dbdataset);


                bSource.DataSource = dbdataset;


                conn.Close();
            }

            return bSource;
        }

        public static int GetWorstScoreResul()
        {
            List<int> scoreList = new List<int>();
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=clasification.db; Version=3;"))
            {
                conn.Open();

                SQLiteCommand cmd = new SQLiteCommand("select CLA_Score from CLASIFICATION", conn);

                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {

                    while (sdr.Read())
                    {
                        scoreList.Add(Convert.ToInt32(sdr[0]));
                    }
                }
                conn.Close();
            }

            if (scoreList.Count < 10)
                return 0;

            return scoreList.Min();

        }

    }
}
