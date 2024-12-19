using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                // Tabloları oluşturmak için metot çağrısı
                CreateTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Tablo oluşturulurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        static void CreateTables()
        {
            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Tarifler tablosunu kontrol et ve oluştur
                string checkTariflerTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Tarifler';";
                using (SQLiteCommand cmd = new SQLiteCommand(checkTariflerTableQuery, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result == null) // Tarifler tablosu yoksa
                    {
                        string createTariflerTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Tarifler (
                            TarifID INTEGER PRIMARY KEY AUTOINCREMENT,
                            TarifAdi VARCHAR(255),
                            Kategori VARCHAR(255),
                            HazirlamaSuresi REAL,
                            Talimatlar TEXT,
                            Maliyet REAL
                        );";

                        using (SQLiteCommand createCmd = new SQLiteCommand(createTariflerTableQuery, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                }

                // Malzemeler tablosunu kontrol et ve oluştur
                string checkMalzemelerTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Malzemeler';";
                using (SQLiteCommand cmd = new SQLiteCommand(checkMalzemelerTableQuery, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result == null) // Malzemeler tablosu yoksa
                    {
                        string createMalzemelerTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Malzemeler (
                            MalzemeID INTEGER PRIMARY KEY AUTOINCREMENT,
                            MalzemeAdi VARCHAR(255),
                            ToplamMiktar VARCHAR(255),
                            MalzemeBirim VARCHAR(50),
                            BirimFiyat DECIMAL(10, 2)
                        );";

                        using (SQLiteCommand createCmd = new SQLiteCommand(createMalzemelerTableQuery, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                }

                // Tarif-Malzeme İlişkisi tablosunu kontrol et ve oluştur
                string checkTarifMalzemelerTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='TarifMalzemeler';";
                using (SQLiteCommand cmd = new SQLiteCommand(checkTarifMalzemelerTableQuery, conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result == null) // Tarif-Malzeme İlişkisi tablosu yoksa
                    {
                        string createTarifMalzemelerTableQuery = @"
                        CREATE TABLE IF NOT EXISTS TarifMalzemeler (
                            TarifID INTEGER,
                            MalzemeID INTEGER,
                            MalzemeMiktar FLOAT,
                            FOREIGN KEY (TarifID) REFERENCES Tarifler(TarifID),
                            FOREIGN KEY (MalzemeID) REFERENCES Malzemeler(MalzemeID)
                        );";

                        using (SQLiteCommand createCmd = new SQLiteCommand(createTarifMalzemelerTableQuery, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
