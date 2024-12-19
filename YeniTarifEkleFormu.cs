using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class YeniTarifEkleFormu : Form
    {
        private TextBox tarifAdiTextBox = null!;
        private TextBox kategoriTextBox = null!;
        private TextBox hazirlamaSuresiTextBox = null!;
        private TextBox talimatlarTextBox = null!;
        private NumericUpDown malzemeSayisiNumeric = null!;
        private Panel dinamikMalzemePanel = null!;
        private Button malzemeOlusturButton = null!;
        private Button malzemeEkleButton = null!;
        private Button kaydetButton = null!;

        private List<ComboBox> malzemeComboBoxList = new List<ComboBox>();
        private List<TextBox> malzemeMiktarTextBoxList = new List<TextBox>();

        public YeniTarifEkleFormu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Yeni Tarif Ekle";

            // Tarif Adı
            Label tarifAdiLabel = new Label();
            tarifAdiLabel.Text = "Tarif Adı:";
            tarifAdiLabel.Location = new System.Drawing.Point(20, 30);
            tarifAdiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(tarifAdiLabel);

            this.tarifAdiTextBox = new TextBox();
            this.tarifAdiTextBox.Location = new System.Drawing.Point(120, 30);
            this.tarifAdiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.tarifAdiTextBox);

            // Kategori
            Label kategoriAdiLabel = new Label();
            kategoriAdiLabel.Text = "Kategori:";
            kategoriAdiLabel.Location = new System.Drawing.Point(20, 70);
            kategoriAdiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(kategoriAdiLabel);

            this.kategoriTextBox = new TextBox();
            this.kategoriTextBox.Location = new System.Drawing.Point(120, 70);
            this.kategoriTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.kategoriTextBox);

            // Hazırlama Süresi
            Label hazirlamaSuresiLabel = new Label();
            hazirlamaSuresiLabel.Text = "Hazırlama:";
            hazirlamaSuresiLabel.Location = new System.Drawing.Point(20, 110);
            hazirlamaSuresiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(hazirlamaSuresiLabel);

            this.hazirlamaSuresiTextBox = new TextBox();
            this.hazirlamaSuresiTextBox.Location = new System.Drawing.Point(120, 110);
            this.hazirlamaSuresiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.hazirlamaSuresiTextBox);

            // Talimatlar
            Label talimatlarLabel = new Label();
            talimatlarLabel.Text = "Talimatlar:";
            talimatlarLabel.Location = new System.Drawing.Point(20, 150);
            talimatlarLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(talimatlarLabel);

            this.talimatlarTextBox = new TextBox();
            this.talimatlarTextBox.Location = new System.Drawing.Point(120, 150);
            this.talimatlarTextBox.Size = new System.Drawing.Size(200, 100);
            this.talimatlarTextBox.Multiline = true;
            this.Controls.Add(this.talimatlarTextBox);

            // Malzeme Sayısı
            Label malzemeSayisiLabel = new Label();
            malzemeSayisiLabel.Text = "Malzeme:";
            malzemeSayisiLabel.Location = new System.Drawing.Point(400, 30);
            malzemeSayisiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(malzemeSayisiLabel);

            this.malzemeSayisiNumeric = new NumericUpDown();
            this.malzemeSayisiNumeric.Location = new System.Drawing.Point(500, 30);
            this.malzemeSayisiNumeric.Minimum = 1;
            this.malzemeSayisiNumeric.Maximum = 20;
            this.malzemeSayisiNumeric.Size = new System.Drawing.Size(60, 20);
            this.Controls.Add(this.malzemeSayisiNumeric);

            // Malzeme Olustur Butonu
            this.malzemeOlusturButton = new Button();
            this.malzemeOlusturButton.Location = new System.Drawing.Point(580, 30);
            this.malzemeOlusturButton.Size = new System.Drawing.Size(100, 25);
            this.malzemeOlusturButton.Text = "Oluştur";
            this.malzemeOlusturButton.Click += new EventHandler(this.malzemeOlusturButton_Click);
            this.Controls.Add(this.malzemeOlusturButton);

            // Dinamik Malzeme Paneli
            this.dinamikMalzemePanel = new Panel();
            this.dinamikMalzemePanel.Location = new System.Drawing.Point(400, 70);
            this.dinamikMalzemePanel.Size = new System.Drawing.Size(350, 300);
            this.dinamikMalzemePanel.AutoScroll = true;
            this.Controls.Add(this.dinamikMalzemePanel);

            // Yeni Malzeme Ekle Butonu
            this.malzemeEkleButton = new Button();
            this.malzemeEkleButton.Location = new System.Drawing.Point(580, 380);
            this.malzemeEkleButton.Size = new System.Drawing.Size(120, 30);
            this.malzemeEkleButton.Text = "Malzeme Ekle";
            this.malzemeEkleButton.Click += new EventHandler(this.malzemeEkleButton_Click);
            this.Controls.Add(this.malzemeEkleButton);

            // Kaydet Butonu
            this.kaydetButton = new Button();
            this.kaydetButton.Location = new System.Drawing.Point(350, 500);
            this.kaydetButton.Size = new System.Drawing.Size(100, 30);
            this.kaydetButton.Text = "Kaydet";
            this.kaydetButton.Click += new EventHandler(this.kaydetButton_Click);
            this.Controls.Add(this.kaydetButton);
        }

        private void malzemeOlusturButton_Click(object? sender, EventArgs e)
        {
            int malzemeSayisi = (int)malzemeSayisiNumeric.Value;

            
            dinamikMalzemePanel.Controls.Clear();
            malzemeComboBoxList.Clear();
            malzemeMiktarTextBoxList.Clear();

            // Mevcut malzemeleri yükle ve ComboBox'lara ekle
            List<string> mevcutMalzemeler = MalzemeleriYukle();

            for (int i = 0; i < malzemeSayisi; i++)
            {
                // Malzeme ComboBox
                Label malzemeAdiLabel = new Label();
                malzemeAdiLabel.Text = $"Malzeme {i + 1}:";
                malzemeAdiLabel.Location = new System.Drawing.Point(0, i * 40);
                malzemeAdiLabel.Size = new System.Drawing.Size(80, 20);
                dinamikMalzemePanel.Controls.Add(malzemeAdiLabel);

                ComboBox malzemeComboBox = new ComboBox();
                malzemeComboBox.Location = new System.Drawing.Point(100, i * 40);
                malzemeComboBox.Size = new System.Drawing.Size(120, 20);
                malzemeComboBox.Items.AddRange(mevcutMalzemeler.ToArray());
                dinamikMalzemePanel.Controls.Add(malzemeComboBox);
                malzemeComboBoxList.Add(malzemeComboBox);

                // Malzeme Miktar TextBox
                Label malzemeMiktarLabel = new Label();
                malzemeMiktarLabel.Text = "Miktar:";
                malzemeMiktarLabel.Location = new System.Drawing.Point(230, i * 40);
                malzemeMiktarLabel.Size = new System.Drawing.Size(40, 20);
                dinamikMalzemePanel.Controls.Add(malzemeMiktarLabel);

                TextBox malzemeMiktarTextBox = new TextBox();
                malzemeMiktarTextBox.Location = new System.Drawing.Point(280, i * 40);
                malzemeMiktarTextBox.Size = new System.Drawing.Size(50, 20);
                dinamikMalzemePanel.Controls.Add(malzemeMiktarTextBox);
                malzemeMiktarTextBoxList.Add(malzemeMiktarTextBox);
            }
        }

        private void malzemeEkleButton_Click(object? sender, EventArgs e)
        {
            // Yeni malzeme eklemek için bir pop-up oluştur
            Form malzemeEkleFormu = new Form();
            malzemeEkleFormu.Text = "Yeni Malzeme Ekle";
            malzemeEkleFormu.Size = new System.Drawing.Size(400, 350);
            malzemeEkleFormu.FormBorderStyle = FormBorderStyle.FixedDialog;
            malzemeEkleFormu.StartPosition = FormStartPosition.CenterParent;
            malzemeEkleFormu.MaximizeBox = false;
            malzemeEkleFormu.MinimizeBox = false;

            // Malzeme Adı
            Label malzemeAdiLabel = new Label();
            malzemeAdiLabel.Text = "Malzeme Adı:";
            malzemeAdiLabel.Location = new System.Drawing.Point(20, 30);
            malzemeEkleFormu.Controls.Add(malzemeAdiLabel);

            TextBox malzemeAdiTextBox = new TextBox();
            malzemeAdiTextBox.Location = new System.Drawing.Point(150, 30);
            malzemeAdiTextBox.Size = new System.Drawing.Size(200, 20);
            malzemeEkleFormu.Controls.Add(malzemeAdiTextBox);

            // Toplam Miktar 
            Label toplamMiktarLabel = new Label();
            toplamMiktarLabel.Text = "Toplam Miktar:";
            toplamMiktarLabel.Location = new System.Drawing.Point(20, 70);
            malzemeEkleFormu.Controls.Add(toplamMiktarLabel);

            TextBox toplamMiktarTextBox = new TextBox();
            toplamMiktarTextBox.Location = new System.Drawing.Point(150, 70);
            toplamMiktarTextBox.Size = new System.Drawing.Size(200, 20);
            malzemeEkleFormu.Controls.Add(toplamMiktarTextBox);

            // Malzeme Birimi
            Label malzemeBirimLabel = new Label();
            malzemeBirimLabel.Text = "Malzeme Birimi:";
            malzemeBirimLabel.Location = new System.Drawing.Point(20, 110);
            malzemeEkleFormu.Controls.Add(malzemeBirimLabel);

            TextBox malzemeBirimTextBox = new TextBox();
            malzemeBirimTextBox.Location = new System.Drawing.Point(150, 110);
            malzemeBirimTextBox.Size = new System.Drawing.Size(200, 20);
            malzemeEkleFormu.Controls.Add(malzemeBirimTextBox);

            // Birim Fiyat
            Label birimFiyatLabel = new Label();
            birimFiyatLabel.Text = "Birim Fiyat:";
            birimFiyatLabel.Location = new System.Drawing.Point(20, 150);
            malzemeEkleFormu.Controls.Add(birimFiyatLabel);

            TextBox birimFiyatTextBox = new TextBox();
            birimFiyatTextBox.Location = new System.Drawing.Point(150, 150);
            birimFiyatTextBox.Size = new System.Drawing.Size(200, 20);
            malzemeEkleFormu.Controls.Add(birimFiyatTextBox);

            // Kaydet Butonu
            Button kaydetMalzemeButton = new Button();
            kaydetMalzemeButton.Text = "Kaydet";
            kaydetMalzemeButton.Location = new System.Drawing.Point(150, 200);
            kaydetMalzemeButton.Size = new System.Drawing.Size(80, 30);
            kaydetMalzemeButton.Click += (s, ev) =>
            {
                string malzemeAdi = malzemeAdiTextBox.Text;
                string toplamMiktar = toplamMiktarTextBox.Text;
                string malzemeBirim = malzemeBirimTextBox.Text;
                string birimFiyat = birimFiyatTextBox.Text;

                if (string.IsNullOrWhiteSpace(malzemeAdi) || string.IsNullOrWhiteSpace(toplamMiktar) ||
                    string.IsNullOrWhiteSpace(malzemeBirim) || string.IsNullOrWhiteSpace(birimFiyat))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Veritabanına malzeme ekleme
                string connectionString = "Data Source=deneme2DB.db;Version=3;";
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
            INSERT INTO Malzemeler (MalzemeAdi, ToplamMiktar, MalzemeBirim, BirimFiyat) 
            VALUES (@malzemeAdi, @toplamMiktar, @malzemeBirim, @birimFiyat)";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@malzemeAdi", malzemeAdi);
                        cmd.Parameters.AddWithValue("@toplamMiktar", toplamMiktar);
                        cmd.Parameters.AddWithValue("@malzemeBirim", malzemeBirim);
                        cmd.Parameters.AddWithValue("@birimFiyat", birimFiyat);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Malzeme başarıyla eklendi!");
                malzemeEkleFormu.Close(); // Pop-up'ı kapat

                // Malzeme listelerini yeniden yükle
                List<string> mevcutMalzemeler = MalzemeleriYukle();
                foreach (var comboBox in malzemeComboBoxList)
                {
                    comboBox.Items.Clear();
                    comboBox.Items.AddRange(mevcutMalzemeler.ToArray());
                }
            };
            malzemeEkleFormu.Controls.Add(kaydetMalzemeButton);

            // İptal Butonu
            Button iptalMalzemeButton = new Button();
            iptalMalzemeButton.Text = "İptal";
            iptalMalzemeButton.Location = new System.Drawing.Point(250, 200);
            iptalMalzemeButton.Size = new System.Drawing.Size(80, 30);
            iptalMalzemeButton.Click += (s, ev) => { malzemeEkleFormu.Close(); };
            malzemeEkleFormu.Controls.Add(iptalMalzemeButton);

            // Ana formda pop-up olarak göster
            malzemeEkleFormu.ShowDialog();
        }


        private List<string> MalzemeleriYukle()
        {
            List<string> malzemeler = new List<string>();
            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MalzemeAdi FROM Malzemeler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Malzeme adı null olabilir, bu nedenle casting ve kontrol ekleniyor
                            string? malzemeAdi = reader["MalzemeAdi"] as string;
                            if (!string.IsNullOrEmpty(malzemeAdi))
                            {
                                malzemeler.Add(malzemeAdi);
                            }
                        }
                    }
                }
            }

            return malzemeler;
        }



        private void kaydetButton_Click(object? sender, EventArgs e)
        {
            // Inputları kontrol et
            if (string.IsNullOrWhiteSpace(tarifAdiTextBox.Text))
            {
                MessageBox.Show("Lütfen tarif adını girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tarifAdiTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(kategoriTextBox.Text))
            {
                MessageBox.Show("Lütfen kategori adını girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                kategoriTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(hazirlamaSuresiTextBox.Text))
            {
                MessageBox.Show("Lütfen hazırlama süresini girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hazirlamaSuresiTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(talimatlarTextBox.Text))
            {
                MessageBox.Show("Lütfen talimatları girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                talimatlarTextBox.Focus();
                return;
            }

            // Tarif bilgilerini al
            string tarifAdi = tarifAdiTextBox.Text;
            string kategori = kategoriTextBox.Text;
            int hazirlamaSuresi;
            int.TryParse(hazirlamaSuresiTextBox.Text, out hazirlamaSuresi);
            string talimatlar = talimatlarTextBox.Text;

            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Aynı ada sahip bir tarif olup olmadığını kontrol et
                string checkQuery = "SELECT COUNT(*) FROM Tarifler WHERE TarifAdi = @tarifAdi";
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@tarifAdi", tarifAdi);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Bu tarif adı zaten mevcut! Lütfen farklı bir tarif adı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; 
                    }
                }

                // Yeni tarif eklenir
                string insertTarifQuery = @"
        INSERT INTO Tarifler (TarifAdi, Kategori, HazirlamaSuresi, Talimatlar)
        VALUES (@tarifAdi, @kategori, @hazirlamaSuresi, @talimatlar);
        SELECT last_insert_rowid();"; // TarifID'yi almak için

                int tarifID;
                using (SQLiteCommand insertTarifCmd = new SQLiteCommand(insertTarifQuery, conn))
                {
                    insertTarifCmd.Parameters.AddWithValue("@tarifAdi", tarifAdi);
                    insertTarifCmd.Parameters.AddWithValue("@kategori", kategori);
                    insertTarifCmd.Parameters.AddWithValue("@hazirlamaSuresi", hazirlamaSuresi);
                    insertTarifCmd.Parameters.AddWithValue("@talimatlar", talimatlar);

                    tarifID = Convert.ToInt32(insertTarifCmd.ExecuteScalar()); // Eklenen tarifin ID'si alınır
                }

                // Malzeme ekleme işlemi
                for (int i = 0; i < malzemeComboBoxList.Count; i++)
                {
                    ComboBox malzemeComboBox = malzemeComboBoxList[i];
                    TextBox malzemeMiktarTextBox = malzemeMiktarTextBoxList[i];

                    string malzemeAdi = malzemeComboBox.SelectedItem?.ToString() ?? "";
                    float malzemeMiktar;
                    float.TryParse(malzemeMiktarTextBox.Text, out malzemeMiktar);

                    if (!string.IsNullOrEmpty(malzemeAdi) && malzemeMiktar > 0)
                    {
                        // MalzemeID'yi al
                        string getMalzemeIDQuery = "SELECT MalzemeID FROM Malzemeler WHERE MalzemeAdi = @malzemeAdi";
                        int malzemeID;
                        using (SQLiteCommand getMalzemeIDCmd = new SQLiteCommand(getMalzemeIDQuery, conn))
                        {
                            getMalzemeIDCmd.Parameters.AddWithValue("@malzemeAdi", malzemeAdi);
                            malzemeID = Convert.ToInt32(getMalzemeIDCmd.ExecuteScalar());
                        }

                        // Tarif-Malzeme İlişkisi tablosuna ekle
                        string insertTarifMalzemeQuery = @"
                INSERT INTO TarifMalzemeler (TarifID, MalzemeID, MalzemeMiktar)
                VALUES (@tarifID, @malzemeID, @malzemeMiktar)";

                        using (SQLiteCommand insertTarifMalzemeCmd = new SQLiteCommand(insertTarifMalzemeQuery, conn))
                        {
                            insertTarifMalzemeCmd.Parameters.AddWithValue("@tarifID", tarifID);
                            insertTarifMalzemeCmd.Parameters.AddWithValue("@malzemeID", malzemeID);
                            insertTarifMalzemeCmd.Parameters.AddWithValue("@malzemeMiktar", malzemeMiktar);

                            insertTarifMalzemeCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Tarif ve malzemeler kaydedildikten sonra ana formdaki tabloyu güncelle
            if (this.Owner != null && this.Owner is Form1 anaForm)
            {
                anaForm.TariflerTablosunuGuncelle(); // Ana formdaki tabloyu güncelle
            }

            MessageBox.Show("Tarif ve malzemeler başarıyla eklendi!");
        }


    }
}
