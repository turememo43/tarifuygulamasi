using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class TarifSilFormu : Form
    {
        private ComboBox comboBoxTarif = null!;
        private Button silButton = null!;
        private Label labelTarifSeciniz = null!;
        private string connectionString = "Data Source=deneme2DB.db;Version=3;";
        private DataGridView tarifDataGridView;  

        
        public TarifSilFormu(DataGridView dataGridView)
        {
            tarifDataGridView = dataGridView;  
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Label ekleme ve ayarlar
            labelTarifSeciniz = new Label();
            labelTarifSeciniz.Location = new System.Drawing.Point(50, 50);
            labelTarifSeciniz.Size = new System.Drawing.Size(100, 20);
            labelTarifSeciniz.Text = "Tarifi Seçiniz:";
            this.Controls.Add(labelTarifSeciniz);

            // ComboBox ekleme ve ayarlar
            comboBoxTarif = new ComboBox();
            comboBoxTarif.Location = new System.Drawing.Point(165, 50);
            comboBoxTarif.Size = new System.Drawing.Size(200, 20);
            comboBoxTarif.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(comboBoxTarif);

            // Silme butonu
            silButton = new Button();
            silButton.Location = new System.Drawing.Point(165, 100);
            silButton.Size = new System.Drawing.Size(100, 35);
            silButton.Text = "Tarifi Sil";
            silButton.UseVisualStyleBackColor = true;
            silButton.Click += SilButton_Click;
            this.Controls.Add(silButton);

            // Form ayarları
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Text = "Tarif Sil";

            // ComboBox'u doldur
            LoadTarifComboBox();
        }

        private void LoadTarifComboBox()
        {
            comboBoxTarif.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TarifAdi FROM Tarifler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxTarif.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void SilButton_Click(object? sender, EventArgs e)
        {
            if (comboBoxTarif.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz tarifi seçin!", "Eksik Seçim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenTarif = comboBoxTarif.SelectedItem.ToString() ?? string.Empty;
            int tarifId = GetTarifIdByName(secilenTarif);

            if (tarifId == -1)
            {
                MessageBox.Show("Seçilen tarif bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Tarifler WHERE TarifID = @tarifId;";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tarifId", tarifId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Tarif başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTarifComboBox();  // ComboBox'u güncelle
                UpdateDataGridView(); // DataGridView'i güncelle
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}", "Uygulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridView()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tarifler";  // Tüm tarifleri çekiyoruz

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    var dataTable = new System.Data.DataTable();
                    dataTable.Load(reader);  // Gelen veriyi tabloya yüklüyoruz
                    tarifDataGridView.DataSource = dataTable;  // DataGridView'e yüklüyoruz
                }
            }
        }

        private int GetTarifIdByName(string tarifAdi)
        {
            int tarifId = -1;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TarifID FROM Tarifler WHERE TarifAdi = @tarifAdi LIMIT 1;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tarifAdi", tarifAdi);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tarifId = reader.GetInt32(0);
                        }
                    }
                }
            }

            return tarifId;
        }
    }
}
