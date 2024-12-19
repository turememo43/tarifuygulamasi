using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class MalzemeUpFormu : Form
    {
        private TextBox malzemeAdiTextBox = null!;
        private TextBox toplamMiktarTextBox = null!;
        private TextBox malzemeBirimTextBox = null!;
        private TextBox birimFiyatTextBox = null!;
        private ComboBox comboBoxMalzeme = null!;
        private Button kaydetButton = null!;

        public MalzemeUpFormu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ComboBox ekleme ve ayarlar
            comboBoxMalzeme = new ComboBox();
            comboBoxMalzeme.Location = new System.Drawing.Point(165, 50);
            comboBoxMalzeme.Size = new System.Drawing.Size(200, 20);
            comboBoxMalzeme.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMalzeme.SelectedIndexChanged += ComboBoxMalzeme_SelectedIndexChanged; // Olayı bağlama
            this.Controls.Add(comboBoxMalzeme);

            Label malzemeAdiLabel = new Label();
            malzemeAdiLabel.Text = "Malzeme Seçin:";
            malzemeAdiLabel.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            malzemeAdiLabel.Location = new System.Drawing.Point(20, 50);
            malzemeAdiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(malzemeAdiLabel);

            Label malzemeAdiLabel2 = new Label();
            malzemeAdiLabel2.Text = "Malzeme Adı:";
            malzemeAdiLabel2.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            malzemeAdiLabel2.Location = new System.Drawing.Point(20, 90);
            malzemeAdiLabel2.Size = new System.Drawing.Size(100, 30);
            this.Controls.Add(malzemeAdiLabel2);

            malzemeAdiTextBox = new TextBox();
            malzemeAdiTextBox.Location = new System.Drawing.Point(165, 90);
            malzemeAdiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(malzemeAdiTextBox);

            Label toplamMiktarLabel = new Label();
            toplamMiktarLabel.Text = "Toplam Miktar:";
            toplamMiktarLabel.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            toplamMiktarLabel.Location = new System.Drawing.Point(20, 130);
            toplamMiktarLabel.Size = new System.Drawing.Size(140, 30);
            this.Controls.Add(toplamMiktarLabel);

            toplamMiktarTextBox = new TextBox();
            toplamMiktarTextBox.Location = new System.Drawing.Point(165, 130);
            toplamMiktarTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(toplamMiktarTextBox);

            Label malzemeBirimLabel = new Label();
            malzemeBirimLabel.Text = "Malzeme Birim:";
            malzemeBirimLabel.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            malzemeBirimLabel.Location = new System.Drawing.Point(20, 170);
            malzemeBirimLabel.Size = new System.Drawing.Size(130, 30);
            this.Controls.Add(malzemeBirimLabel);

            malzemeBirimTextBox = new TextBox();
            malzemeBirimTextBox.Location = new System.Drawing.Point(165, 170);
            malzemeBirimTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(malzemeBirimTextBox);

            Label birimFiyatLabel = new Label();
            birimFiyatLabel.Text = "Birim Fiyat:";
            birimFiyatLabel.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
            birimFiyatLabel.Location = new System.Drawing.Point(20, 210);
            birimFiyatLabel.Size = new System.Drawing.Size(130, 30);
            this.Controls.Add(birimFiyatLabel);

            birimFiyatTextBox = new TextBox();
            birimFiyatTextBox.Location = new System.Drawing.Point(165, 210);
            birimFiyatTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(birimFiyatTextBox);

            kaydetButton = new Button();
            kaydetButton.Location = new System.Drawing.Point(205, 260);
            kaydetButton.Size = new System.Drawing.Size(100, 35);
            kaydetButton.Text = "Kaydet";
            kaydetButton.UseVisualStyleBackColor = true;
            kaydetButton.Click += kaydetButton_Click;
            this.Controls.Add(kaydetButton);

            // Form ayarları
            this.ClientSize = new System.Drawing.Size(500, 350);
            this.Text = "Malzeme Güncelle";

            // ComboBox'u doldur
            LoadMalzemeComboBox();
        }

        private void ComboBoxMalzeme_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxMalzeme?.SelectedItem != null)
            {
                string secilenMalzeme = comboBoxMalzeme.SelectedItem.ToString() ?? string.Empty;

                // Seçilen malzemeye göre ilgili bilgileri getir ve TextBox'lara yerleştir
                LoadMalzemeDetails(secilenMalzeme);
            }
        }

        private void LoadMalzemeComboBox()
        {
            List<string> malzemeAdlari = new List<string>();
            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MalzemeAdi FROM Malzemeler;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        malzemeAdlari.Add(reader.GetString(0));
                    }
                }
            }

            comboBoxMalzeme.Items.Clear();
            comboBoxMalzeme.Items.AddRange(malzemeAdlari.ToArray());
        }

        private void LoadMalzemeDetails(string malzemeAdi)
        {
            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Malzemeler WHERE MalzemeAdi = @malzemeAdi;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@malzemeAdi", malzemeAdi);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            malzemeAdiTextBox.Text = reader["MalzemeAdi"].ToString();
                            toplamMiktarTextBox.Text = reader["ToplamMiktar"].ToString();
                            malzemeBirimTextBox.Text = reader["MalzemeBirim"].ToString();
                            birimFiyatTextBox.Text = reader["BirimFiyat"].ToString();
                        }
                    }
                }
            }
        }

        private void kaydetButton_Click(object? sender, EventArgs e)
        {
            if (comboBoxMalzeme.SelectedItem == null)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz malzemeyi seçin!", "Eksik Seçim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenMalzeme = comboBoxMalzeme.SelectedItem.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(malzemeAdiTextBox.Text) ||
                string.IsNullOrWhiteSpace(toplamMiktarTextBox.Text) ||
                string.IsNullOrWhiteSpace(malzemeBirimTextBox.Text) ||
                string.IsNullOrWhiteSpace(birimFiyatTextBox.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string yeniMalzemeAdi = malzemeAdiTextBox.Text;
            string yeniToplamMiktar = toplamMiktarTextBox.Text;
            string yeniMalzemeBirim = malzemeBirimTextBox.Text;
            float yeniBirimFiyat;
            if (!float.TryParse(birimFiyatTextBox.Text, out yeniBirimFiyat))
            {
                MessageBox.Show("Geçerli bir birim fiyat girin!", "Geçersiz Değer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=deneme2DB.db;Version=3;";
            int malzemeId = GetMalzemeIdByName(secilenMalzeme);

            if (malzemeId == -1)
            {
                MessageBox.Show("Seçilen malzeme bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Malzemeler 
                    SET MalzemeAdi = @yeniMalzemeAdi, ToplamMiktar = @yeniToplamMiktar, MalzemeBirim = @yeniMalzemeBirim, BirimFiyat = @yeniBirimFiyat
                    WHERE MalzemeID = @malzemeId;
                ";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@yeniMalzemeAdi", yeniMalzemeAdi);
                    cmd.Parameters.AddWithValue("@yeniToplamMiktar", yeniToplamMiktar);
                    cmd.Parameters.AddWithValue("@yeniMalzemeBirim", yeniMalzemeBirim);
                    cmd.Parameters.AddWithValue("@yeniBirimFiyat", yeniBirimFiyat);
                    cmd.Parameters.AddWithValue("@malzemeId", malzemeId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Malzeme başarıyla güncellendi!");
                }
            }
        }

        private int GetMalzemeIdByName(string malzemeAdi)
        {
            if (string.IsNullOrWhiteSpace(malzemeAdi))
            {
                MessageBox.Show("Malzeme adı boş olamaz!", "Geçersiz Değer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            int malzemeId = -1;
            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MalzemeID FROM Malzemeler WHERE MalzemeAdi = @malzemeAdi LIMIT 1;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@malzemeAdi", malzemeAdi);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            malzemeId = reader.GetInt32(0);
                        }
                    }
                }
            }

            return malzemeId;
        }
    }
}
