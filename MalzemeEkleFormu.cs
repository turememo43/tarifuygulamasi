using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace deneme2
{
    public partial class MalzemeEkleFormu : Form
    {
        private TextBox malzemeAdiTextBox = null!;
        private TextBox toplamMiktarTextBox = null!;
        private TextBox malzemeBirimTextBox = null!;
        private TextBox birimFiyatTextBox = null!;
        private Button kaydetButton = null!;

        public MalzemeEkleFormu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {         

            // TextBox ve Button için ayarlar

            Label malzemeAdiLabel = new Label();
            malzemeAdiLabel.Text = "Malzeme Adi:";
            malzemeAdiLabel.Location = new System.Drawing.Point(20, 50);
            malzemeAdiLabel.Size = new System.Drawing.Size(120, 20);
            this.Controls.Add(malzemeAdiLabel);

            this.malzemeAdiTextBox = new System.Windows.Forms.TextBox();
            this.malzemeAdiTextBox.Location = new System.Drawing.Point(165, 50);
            this.malzemeAdiTextBox.Name = "malzemeAdiTextBox";
            this.malzemeAdiTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.malzemeAdiTextBox);

            Label toplamMiktarLabel = new Label();
            toplamMiktarLabel.Text = "Toplam Miktar:";
            toplamMiktarLabel.Location = new System.Drawing.Point(20, 90);
            toplamMiktarLabel.Size = new System.Drawing.Size(130, 30);
            this.Controls.Add(toplamMiktarLabel);

            this.toplamMiktarTextBox = new System.Windows.Forms.TextBox();
            this.toplamMiktarTextBox.Location = new System.Drawing.Point(165, 90);
            this.toplamMiktarTextBox.Name = "toplamMiktarTextBox";
            this.toplamMiktarTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.toplamMiktarTextBox);

            Label malzemeBirimLabel = new Label();
            malzemeBirimLabel.Text = "Malzeme Birim:";
            malzemeBirimLabel.Location = new System.Drawing.Point(20, 130);
            malzemeBirimLabel.Size = new System.Drawing.Size(140, 30);
            this.Controls.Add(malzemeBirimLabel);

            this.malzemeBirimTextBox = new System.Windows.Forms.TextBox();
            this.malzemeBirimTextBox.Location = new System.Drawing.Point(165, 130);
            this.malzemeBirimTextBox.Name = "malzemeBirimTextBox";
            this.malzemeBirimTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.malzemeBirimTextBox);

            Label birimFiyatLabel = new Label();
            birimFiyatLabel.Text = "Birim Fiyat:";
            birimFiyatLabel.Location = new System.Drawing.Point(20, 170);
            birimFiyatLabel.Size = new System.Drawing.Size(130, 30);
            this.Controls.Add(birimFiyatLabel);

            this.birimFiyatTextBox = new System.Windows.Forms.TextBox();
            this.birimFiyatTextBox.Location = new System.Drawing.Point(165, 170);
            this.birimFiyatTextBox.Name = "birimFiyatTextBox";
            this.birimFiyatTextBox.Size = new System.Drawing.Size(200, 20);
            this.Controls.Add(this.birimFiyatTextBox);

            this.kaydetButton = new System.Windows.Forms.Button();
            this.kaydetButton.Location = new System.Drawing.Point(205, 210);
            this.kaydetButton.Name = "kaydetButton";
            this.kaydetButton.Size = new System.Drawing.Size(100, 35);
            this.kaydetButton.Text = "Kaydet";
            this.kaydetButton.UseVisualStyleBackColor = true;
            this.kaydetButton.Click += new System.EventHandler(this.kaydetButton_Click);
            this.Controls.Add(this.kaydetButton);

            // Form ayarları
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Text = "Yeni Malzeme Ekle";
        }

        private void kaydetButton_Click(object? sender, EventArgs e)
        {
            
            // Inputları kontrol et
            if (string.IsNullOrWhiteSpace(malzemeAdiTextBox.Text))
            {
                MessageBox.Show("Lütfen malzeme adını girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                malzemeAdiTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(toplamMiktarTextBox.Text))
            {
                MessageBox.Show("Lütfen elinizdeki toplam miktarı girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                toplamMiktarTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(malzemeBirimTextBox.Text))
            {
                MessageBox.Show("Lütfen malzemenin birimini girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                malzemeBirimTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(birimFiyatTextBox.Text))
            {
                MessageBox.Show("Lütfen malzemenin birim fiyatını girin!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                birimFiyatTextBox.Focus();
                return;
            }


            // Tüm inputlar doluysa, veri tabanına ekleme işlemini gerçekleştir

            string malzemeAdi = malzemeAdiTextBox.Text;
            string toplamMiktar = toplamMiktarTextBox.Text;
            string malzemeBirim = malzemeBirimTextBox.Text;
            decimal birimFiyat;
            decimal.TryParse(birimFiyatTextBox.Text, out birimFiyat);

            string connectionString = "Data Source=deneme2DB.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Malzemeler (MalzemeAdi, ToplamMiktar, MalzemeBirim, BirimFiyat)
                    VALUES (@malzemeAdi, @toplamMiktar, @malzemeBirim, @birimFiyat);
                ";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@malzemeAdi", malzemeAdi);
                    cmd.Parameters.AddWithValue("@toplamMiktar", toplamMiktar);
                    cmd.Parameters.AddWithValue("@malzemeBirim", malzemeBirim);
                    cmd.Parameters.AddWithValue("@birimFiyat", birimFiyat);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Malzeme başarıyla eklendi!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Malzeme eklenirken bir hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
}
