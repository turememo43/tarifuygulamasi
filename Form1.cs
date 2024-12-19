using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source=deneme2DB.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            LoadTarifler();  // Form açıldığında mevcut tarifler yüklenecek
            LoadMalzemeListesi();  // Malzeme listesini yükle
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            // DataGridView'deki CellClick olayını bağla
            tarifDataGridView.CellClick += new DataGridViewCellEventHandler(tarifDataGridView_CellClick);

        }

        // Tarifleri yükleme fonksiyonu
        private void LoadTarifler()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TarifID, TarifAdi, Kategori, HazirlamaSuresi, Talimatlar FROM Tarifler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);

                    // 'Maliyet' sütununu ekle ve hesapla
                    if (!dataTable.Columns.Contains("Maliyet"))
                    {
                        dataTable.Columns.Add("Maliyet", typeof(decimal));
                    }

                    try
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            int tarifID = Convert.ToInt32(row["TarifID"]);
                            row["Maliyet"] = HesaplaMaliyet(tarifID);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Maliyet hesaplama hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // DataView oluştur ve DataSource olarak ata
                    DataView dataView = new DataView(dataTable);
                    tarifDataGridView.DataSource = dataView;
                }

                // TarifID ve Kategori sütunlarını gizle
                if (tarifDataGridView.Columns["TarifID"] != null)
                {
                    tarifDataGridView.Columns["TarifID"].Visible = false;
                }
                if (tarifDataGridView.Columns["Kategori"] != null)
                {
                    tarifDataGridView.Columns["Kategori"].Visible = false; // Kategori sütununu gizli
                }
                if (tarifDataGridView.Columns["Talimatlar"] != null)
                {
                    tarifDataGridView.Columns["Talimatlar"].Visible = false;
                }

                // Eksik maliyet bilgisini sıfırla
                if (tarifDataGridView.Columns.Contains("Eksik Maliyet"))
                {
                    tarifDataGridView.Columns.Remove("Eksik Maliyet");
                }
            }
        }




        // CheckedListBox'a malzeme yükleme fonksiyonu
        private void LoadMalzemeListesi()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Malzemeleri veritabanından çek
                string query = "SELECT MalzemeAdi FROM Malzemeler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    malzemeListBox.Items.Clear(); // Mevcut öğeleri temizle

                    // Malzemeleri listeye ekle
                    while (reader.Read())
                    {
                        // Null ve DBNull kontrolü ile malzeme adını al
                        string malzeme = reader.IsDBNull(reader.GetOrdinal("MalzemeAdi"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("MalzemeAdi"));

                        if (!string.IsNullOrEmpty(malzeme))
                        {
                            malzemeListBox.Items.Add(malzeme);
                        }
                    }
                }
            }
        }






        // Veritabanından kategori bilgisini çekme fonksiyonu
        private string GetKategori(int tarifID)
        {
            string kategori = "Kategori Yok";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Kategori FROM Tarifler WHERE TarifID = @tarifID LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifID", tarifID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kategori = reader["Kategori"]?.ToString() ?? "Kategori Yok";
                        }
                    }
                }
            }

            return kategori;
        }

        private string GetTalimatlar(int tarifID)
        {
            string talimatlar = "Talimat yok";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Talimatlar FROM Tarifler WHERE TarifID = @tarifID LIMIT 1";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifID", tarifID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            talimatlar = reader["Talimatlar"]?.ToString() ?? "Talimat yok";
                        }
                    }
                }
            }

            return talimatlar;
        }




        // DataGridView'deki hücreye tıklanma olayı
        private void tarifDataGridView_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklanmışsa
            {
                DataGridViewRow row = tarifDataGridView.Rows[e.RowIndex];

                // Null kontrolü ile sütun değerlerini al
                string tarifAdi = row.Cells["TarifAdi"]?.Value?.ToString() ?? "Bilinmiyor";

                string hazirlamaSuresi = row.Cells["HazirlamaSuresi"]?.Value?.ToString() ?? "Bilinmiyor";
                string maliyet = row.Cells["Maliyet"]?.Value?.ToString() ?? "Bilinmiyor";


                int tarifID = Convert.ToInt32(row.Cells["TarifID"].Value);

                // TarifID'yi al ve kategori bilgisini veritabanından çek

                string kategori = GetKategori(tarifID);
                string talimatlar = GetTalimatlar(tarifID);

                // Malzeme detaylarını yükle
                string malzemeDetaylari = LoadMalzemeDetaylari(tarifID);

                // Detayları gösteren bir mesaj kutusu
                string mesaj = $"Tarif Adı: {tarifAdi}\nKategori: {kategori}\nHazırlama Süresi: {hazirlamaSuresi} dk\nMaliyet: {maliyet} TL\nTalimatlar: {talimatlar}\n\nMalzemeler:\n{malzemeDetaylari}";
                MessageBox.Show(mesaj, "Tarif Detayları", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private string LoadMalzemeDetaylari(int tarifID)
        {
            string malzemeDetaylari = "";
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT M.MalzemeAdi, T.MalzemeMiktar, M.MalzemeBirim 
                    FROM TarifMalzemeler T
                    JOIN Malzemeler M ON T.MalzemeID = M.MalzemeID
                    WHERE T.TarifID = @tarifID";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifID", tarifID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Null kontrolü ile malzeme bilgilerini al
                            string malzemeAdi = reader["MalzemeAdi"]?.ToString() ?? "Bilinmiyor";
                            string malzemeMiktar = reader["MalzemeMiktar"]?.ToString() ?? "Bilinmiyor";
                            string malzemeBirim = reader["MalzemeBirim"]?.ToString() ?? "Bilinmiyor";

                            malzemeDetaylari += $"{malzemeAdi}: {malzemeMiktar} {malzemeBirim}\n";
                        }
                    }
                }
            }

            return string.IsNullOrEmpty(malzemeDetaylari) ? "Malzeme bulunamadı." : malzemeDetaylari;
        }

        private decimal HesaplaMaliyet(int tarifID)
        {
            decimal toplamMaliyet = 0;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT tm.MalzemeMiktar, m.BirimFiyat 
                    FROM TarifMalzemeler tm
                    JOIN Malzemeler m ON tm.MalzemeID = m.MalzemeID
                    WHERE tm.TarifID = @tarifID";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifID", tarifID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal malzemeMiktar = Convert.ToDecimal(reader["MalzemeMiktar"]);
                            decimal birimFiyat = Convert.ToDecimal(reader["BirimFiyat"]);
                            toplamMaliyet += malzemeMiktar * birimFiyat;
                        }
                    }
                }
            }

            return toplamMaliyet;
        }

        // Malzemeye göre arama
        private void MalzemeyeGoreArama()
        {
            var secilenMalzemeler = new HashSet<string>();

            // CheckedListBox'tan seçilen malzemeleri listeye ekle
            foreach (var item in malzemeListBox.CheckedItems)
            {
                string malzeme = item?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(malzeme))
                {
                    secilenMalzemeler.Add(malzeme);
                }
            }

            // Eğer seçilen malzeme yoksa kullanıcıya uyarı mesajı göster
            if (secilenMalzemeler.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir malzeme seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Tarifleri ve malzemeleri eşleştirme sorgusu
                string query = @"
        SELECT t.TarifID, t.TarifAdi, t.HazirlamaSuresi,
               COUNT(m.MalzemeAdi) AS EslestirilenMalzemeSayisi, 
               ROUND((COUNT(m.MalzemeAdi) * 100.0 / 
                      (SELECT COUNT(*) FROM TarifMalzemeler WHERE TarifID = t.TarifID)), 2) AS EslestirmeYuzdesi
        FROM Tarifler t
        JOIN TarifMalzemeler tm ON t.TarifID = tm.TarifID
        JOIN Malzemeler m ON tm.MalzemeID = m.MalzemeID
        WHERE m.MalzemeAdi IN (" + string.Join(",", secilenMalzemeler.Select(m => $"'{m.Replace("'", "''")}'")) + @")
        GROUP BY t.TarifID, t.TarifAdi, t.HazirlamaSuresi
        ORDER BY EslestirmeYuzdesi DESC";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    var dataTable = new DataTable();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    // 'Maliyet' ve 'Eksik Maliyet' sütunlarını ekle
                    if (!dataTable.Columns.Contains("Maliyet"))
                    {
                        dataTable.Columns.Add("Maliyet", typeof(decimal));
                    }
                    if (!dataTable.Columns.Contains("Eksik Maliyet"))
                    {
                        dataTable.Columns.Add("Eksik Maliyet", typeof(decimal));
                    }

                    // Maliyet ve Eksik Maliyet hesaplaması
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int tarifID = Convert.ToInt32(row["TarifID"]);

                        if (tarifID > 0)
                        {
                            // TarifID üzerinden maliyeti hesapla
                            row["Maliyet"] = HesaplaMaliyet(tarifID);

                            // Eksik malzeme maliyetini hesapla
                            decimal eksikMaliyet = HesaplaEksikMalzemeMaliyeti(tarifID, secilenMalzemeler);
                            row["Eksik Maliyet"] = eksikMaliyet;
                        }
                        else
                        {
                            row["Maliyet"] = 0; // Geçersiz TarifID ise maliyeti 0 yap
                            row["Eksik Maliyet"] = 0; // Geçersiz TarifID için eksik maliyet de 0
                        }
                    }

                    // DataGridView'i güncelle
                    tarifDataGridView.DataSource = dataTable;

                    // Veriler yüklendikten sonra renklendirme işlemi
                    foreach (DataGridViewRow row in tarifDataGridView.Rows)
                    {
                        if (row.Cells["Eksik Maliyet"].Value != null && decimal.TryParse(row.Cells["Eksik Maliyet"].Value.ToString(), out decimal eksikMaliyet))
                        {
                            if (eksikMaliyet > 0)
                            {
                                // Eksik malzeme varsa, satırı kırmızıya boya
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                // Eksik malzeme yoksa, satırı yeşile boya
                                row.DefaultCellStyle.BackColor = Color.Green;
                            }
                        }
                    }
                }
            }
        }


        // Eksik malzeme maliyetini hesaplayan fonksiyon
        private decimal HesaplaEksikMalzemeMaliyeti(int tarifID, HashSet<string> secilenMalzemeler)
        {
            decimal toplamEksikMaliyet = 0;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT m.MalzemeAdi, m.BirimFiyat, tm.MalzemeMiktar
            FROM TarifMalzemeler tm
            JOIN Malzemeler m ON tm.MalzemeID = m.MalzemeID
            WHERE tm.TarifID = @tarifID";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifID", tarifID);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string malzemeAdi = reader["MalzemeAdi"]?.ToString() ?? string.Empty;
                            decimal birimFiyat = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            decimal malzemeMiktar = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);

                            // Eğer malzeme kullanıcı tarafından seçilmemişse, eksik maliyeti ekle
                            if (!secilenMalzemeler.Contains(malzemeAdi))
                            {
                                toplamEksikMaliyet += birimFiyat * malzemeMiktar;
                            }
                        }
                    }
                }
            }

            return toplamEksikMaliyet;
        }



        private void AraButton_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(aramaTextBox.Text))
            {
                // Tarif adına göre arama
                DinamikArama(aramaTextBox.Text.Trim());
            }
            else
            {
                // Malzemeye göre arama
                MalzemeyeGoreArama();
            }
        }



        // Tarif adına göre arama fonksiyonu
        private void DinamikArama(string aramaTerimi)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT t.TarifID, t.TarifAdi, t.HazirlamaSuresi
                    FROM Tarifler t
                    WHERE t.TarifAdi LIKE @arama";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@arama", "%" + aramaTerimi + "%");

                    var dataTable = new DataTable();
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }

                    // Maliyet sütununu ekle ve hesapla
                    if (!dataTable.Columns.Contains("Maliyet"))
                    {
                        dataTable.Columns.Add("Maliyet", typeof(decimal));
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        int tarifID = Convert.ToInt32(row["TarifID"]);
                        row["Maliyet"] = HesaplaMaliyet(tarifID);
                    }

                    tarifDataGridView.DataSource = dataTable;
                }
            }
        }




        // Malzemeleri yükleme fonksiyonu
        private void LoadMalzemeler()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Malzemeler"; // Malzemeleri veritabanından çekiyoruz
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);  // Gelen veriyi tabloya yüklüyoruz
                    tarifDataGridView.DataSource = dataTable;  // DataGridView'e yüklüyoruz
                }

                // MalzemeID sütununu gizleme
                if (tarifDataGridView.Columns["MalzemeID"] != null)
                {
                    tarifDataGridView.Columns["MalzemeID"].Visible = false;
                }
            }
        }

        private void malzemeAraButton_Click(object sender, EventArgs e)
        {
            MalzemeyeGoreArama(); // Malzemeye göre arama fonksiyonunu çağır
        }


        // Tarif tablosunu güncelleme fonksiyonu
        public void TariflerTablosunuGuncelle()
        {
            LoadTarifler(); // Mevcut tarifleri yeniden yükler
        }

        // "Tarif Tablosunu Görüntüle" butonuna tıklama olayı
        private void TarifTablosunuGoruntuleButton_Click(object sender, EventArgs e)
        {
            LoadTarifler(); // Tarif tablosunu görüntüle
        }

        // "Malzeme Tablosunu Görüntüle" butonuna tıklama olayı
        private void MalzemeTablosunuGoruntuleButton_Click(object sender, EventArgs e)
        {
            LoadMalzemeler(); // Malzeme tablosunu görüntüle
        }

        // Yeni Tarif Ekle butonuna tıklama olayı
        private void yeniTarifEkleButton_Click(object sender, EventArgs e)
        {
            YeniTarifEkleFormu yeniTarifFormu = new YeniTarifEkleFormu();
            yeniTarifFormu.Owner = this; // Ana formu sahip olarak ata
            yeniTarifFormu.ShowDialog();
            LoadTarifler(); // Yeni tarif eklendikten sonra tarif listesini güncelle
        }

        // Malzeme Ekle butonuna tıklama olayı
        private void MalzemeEkleButton_Click(object sender, EventArgs e)
        {
            MalzemeEkleFormu malzemeFormu = new MalzemeEkleFormu();
            malzemeFormu.ShowDialog();
            LoadMalzemeler(); // Yeni malzeme eklendikten sonra malzeme listesini güncelle
        }

        // Tarif Güncelle butonuna tıklama olayı
        private void TarifUpButton_Click(object sender, EventArgs e)
        {
            TarifUpFormu trfupformu = new TarifUpFormu();
            trfupformu.ShowDialog();
            LoadTarifler(); // Tarif güncellendikten sonra tarif listesini güncelle
        }

        // Malzeme Güncelle butonuna tıklama olayı
        private void MalzemeUpButton_Click(object sender, EventArgs e)
        {
            MalzemeUpFormu mlzmeupformu = new MalzemeUpFormu();
            mlzmeupformu.ShowDialog();
            LoadMalzemeler(); // Malzeme güncellendikten sonra malzeme listesini güncelle
        }

        // Tarif Sil butonuna tıklama olayı
        private void TarifSilButton_Click(object sender, EventArgs e)
        {
            TarifSilFormu tarifSilFormu = new TarifSilFormu(tarifDataGridView);
            tarifSilFormu.ShowDialog();
            LoadTarifler(); // Tarif silindikten sonra tarif listesini güncelle
        }

        // Malzeme Sil butonuna tıklama olayı
        private void MalzemeSilButton_Click(object sender, EventArgs e)
        {
            MalzemeSilFormu malzemeSilFormu = new MalzemeSilFormu(tarifDataGridView);
            malzemeSilFormu.ShowDialog();
            LoadMalzemeler(); // Malzeme silindikten sonra malzeme listesini güncelle
        }
    }
}
