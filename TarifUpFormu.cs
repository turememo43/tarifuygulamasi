using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class TarifUpFormu : Form
    {
        private TextBox tarifAdiTextBox = null!;
        private TextBox kategoriTextBox = null!;
        private TextBox hazirlamaSuresiTextBox = null!;
        private TextBox talimatlarTextBox = null!;
        private ComboBox comboBoxTarif = null!;
        private Button kaydetButton = null!;
        private string connectionString = "Data Source=deneme2DB.db;Version=3;";

        public TarifUpFormu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // ComboBox ekleme ve ayarlar
            comboBoxTarif = new ComboBox();
            comboBoxTarif.Location = new System.Drawing.Point(165, 20);
            comboBoxTarif.Size = new System.Drawing.Size(200, 20);
            comboBoxTarif.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTarif.SelectedIndexChanged += ComboBoxTarif_SelectedIndexChanged; // Olayı bağlama
            this.Controls.Add(comboBoxTarif);

            Label comboBoxLabel = new Label();
            comboBoxLabel.Text = "Tarif Seçin:";
            comboBoxLabel.Location = new System.Drawing.Point(20, 20);
            comboBoxLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(comboBoxLabel);

            // Tarif Adı TextBox
            tarifAdiTextBox = new TextBox();
            tarifAdiTextBox.Location = new System.Drawing.Point(165, 60);
            tarifAdiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(tarifAdiTextBox);

            Label tarifAdiLabel = new Label();
            tarifAdiLabel.Text = "Tarif Adı:";
            tarifAdiLabel.Location = new System.Drawing.Point(20, 60);
            tarifAdiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(tarifAdiLabel);

            // Kategori TextBox
            kategoriTextBox = new TextBox();
            kategoriTextBox.Location = new System.Drawing.Point(165, 100);
            kategoriTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(kategoriTextBox);

            Label kategoriAdiLabel = new Label();
            kategoriAdiLabel.Text = "Kategori Adı:";
            kategoriAdiLabel.Location = new System.Drawing.Point(20, 100);
            kategoriAdiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(kategoriAdiLabel);

            // Hazırlama Süresi TextBox
            hazirlamaSuresiTextBox = new TextBox();
            hazirlamaSuresiTextBox.Location = new System.Drawing.Point(165, 140);
            hazirlamaSuresiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(hazirlamaSuresiTextBox);

            Label hazirlamaSuresiLabel = new Label();
            hazirlamaSuresiLabel.Text = "Hazırlama Süresi:";
            hazirlamaSuresiLabel.Location = new System.Drawing.Point(20, 140);
            hazirlamaSuresiLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(hazirlamaSuresiLabel);

            // Talimatlar TextBox
            talimatlarTextBox = new TextBox();
            talimatlarTextBox.Location = new System.Drawing.Point(165, 180);
            talimatlarTextBox.Size = new System.Drawing.Size(200, 60);
            talimatlarTextBox.Multiline = true;
            this.Controls.Add(talimatlarTextBox);

            Label talimatlarLabel = new Label();
            talimatlarLabel.Text = "Talimatlar:";
            talimatlarLabel.Location = new System.Drawing.Point(20, 180);
            talimatlarLabel.Size = new System.Drawing.Size(100, 20);
            this.Controls.Add(talimatlarLabel);

            // Kaydet Butonu
            kaydetButton = new Button();
            kaydetButton.Location = new System.Drawing.Point(165, 260);
            kaydetButton.Size = new System.Drawing.Size(100, 30);
            kaydetButton.Text = "Kaydet";
            kaydetButton.Click += KaydetButton_Click;
            this.Controls.Add(kaydetButton);

            // Form ayarları
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Text = "Tarif Güncelle";

            // ComboBox'ı doldur
            LoadTarifComboBox();
        }

        private void LoadTarifComboBox()
        {
            comboBoxTarif.Items.Clear();
            List<string> tarifAdlari = new List<string>();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TarifAdi FROM Tarifler;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tarifAdlari.Add(reader.GetString(0));
                    }
                }
            }

            comboBoxTarif.Items.AddRange(tarifAdlari.ToArray());
        }

        private void ComboBoxTarif_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxTarif.SelectedItem != null)
            {
                string secilenTarif = comboBoxTarif.SelectedItem.ToString() ?? string.Empty;
                LoadTarifDetails(secilenTarif); 
            }
        }

        private void LoadTarifDetails(string tarifAdi)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tarifler WHERE TarifAdi = @tarifAdi LIMIT 1;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifAdi", tarifAdi);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tarifAdiTextBox.Text = reader["TarifAdi"].ToString();
                            kategoriTextBox.Text = reader["Kategori"].ToString();
                            hazirlamaSuresiTextBox.Text = reader["HazirlamaSuresi"].ToString();
                            talimatlarTextBox.Text = reader["Talimatlar"].ToString();
                        }
                    }
                }
            }
        }

        private void KaydetButton_Click(object? sender, EventArgs e)
        {
            if (comboBoxTarif.SelectedItem == null)
            {
                MessageBox.Show("Lütfen güncellemek istediğiniz tarifi seçin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string secilenTarif = comboBoxTarif.SelectedItem.ToString() ?? string.Empty;
            string yeniTarifAdi = tarifAdiTextBox.Text;
            string yeniKategori = kategoriTextBox.Text;
            float yeniHazirlamaSuresi;
            if (!float.TryParse(hazirlamaSuresiTextBox.Text, out yeniHazirlamaSuresi))
            {
                MessageBox.Show("Geçerli bir hazırlama süresi girin!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string yeniTalimatlar = talimatlarTextBox.Text;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Tarifler 
                    SET TarifAdi = @yeniTarifAdi, Kategori = @yeniKategori, HazirlamaSuresi = @yeniHazirlamaSuresi, Talimatlar = @yeniTalimatlar 
                    WHERE TarifAdi = @secilenTarif;
                ";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@yeniTarifAdi", yeniTarifAdi);
                    cmd.Parameters.AddWithValue("@yeniKategori", yeniKategori);
                    cmd.Parameters.AddWithValue("@yeniHazirlamaSuresi", yeniHazirlamaSuresi);
                    cmd.Parameters.AddWithValue("@yeniTalimatlar", yeniTalimatlar);
                    cmd.Parameters.AddWithValue("@secilenTarif", secilenTarif);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Tarif başarıyla güncellendi!");
                }
            }

            LoadTarifComboBox();
        }
    }
}
