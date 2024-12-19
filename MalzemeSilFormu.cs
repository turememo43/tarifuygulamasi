using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class MalzemeSilFormu : Form
    {
        private ComboBox comboBoxMalzeme = null!;
        private Button silButton = null!;
        private Label labelMalzemeSeciniz = null!;
        private string connectionString = "Data Source=deneme2DB.db;Version=3;";
        private DataGridView malzemeDataGridView; 

       
        public MalzemeSilFormu(DataGridView dataGridView)
        {
            malzemeDataGridView = dataGridView;  
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Label ekleme ve ayarlar
            labelMalzemeSeciniz = new Label();
            labelMalzemeSeciniz.Location = new System.Drawing.Point(20, 50);
            labelMalzemeSeciniz.Size = new System.Drawing.Size(145, 20);
            labelMalzemeSeciniz.Text = "Malzemeyi Seçiniz:";
            this.Controls.Add(labelMalzemeSeciniz);

            // ComboBox ekleme ve ayarlar
            comboBoxMalzeme = new ComboBox();
            comboBoxMalzeme.Location = new System.Drawing.Point(165, 50);
            comboBoxMalzeme.Size = new System.Drawing.Size(200, 20);
            comboBoxMalzeme.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(comboBoxMalzeme);

            // Silme butonu
            silButton = new Button();
            silButton.Location = new System.Drawing.Point(165, 100);
            silButton.Size = new System.Drawing.Size(100, 35);
            silButton.Text = "Malzemeyi Sil";
            silButton.UseVisualStyleBackColor = true;
            silButton.Click += SilButton_Click;
            this.Controls.Add(silButton);

            // Form ayarları
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Text = "Malzeme Sil";

            // ComboBox'u doldur
            LoadMalzemeComboBox();
        }

        private void LoadMalzemeComboBox()
        {
            comboBoxMalzeme.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MalzemeAdi FROM Malzemeler";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxMalzeme.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void SilButton_Click(object? sender, EventArgs e)
        {
            if (comboBoxMalzeme.SelectedItem == null)
            {
                MessageBox.Show("Lütfen silmek istediğiniz malzemeyi seçin!", "Eksik Seçim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string secilenMalzeme = comboBoxMalzeme.SelectedItem.ToString() ?? string.Empty;
            int malzemeId = GetMalzemeIdByName(secilenMalzeme);

            if (malzemeId == -1)
            {
                MessageBox.Show("Seçilen malzeme bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Malzemeler WHERE MalzemeID = @malzemeId;";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@malzemeId", malzemeId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Malzeme başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMalzemeComboBox();  // ComboBox'u güncelle
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
                string query = "SELECT * FROM Malzemeler";  // Tüm malzemeleri çekiyoruz

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    var dataTable = new System.Data.DataTable();
                    dataTable.Load(reader);  // Gelen veriyi tabloya yüklüyoruz
                    malzemeDataGridView.DataSource = dataTable;  // DataGridView'e yüklüyoruz
                }
            }
        }

        private int GetMalzemeIdByName(string malzemeAdi)
        {
            int malzemeId = -1;

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
