namespace deneme2
{
    partial class Form1
    {
        private System.Windows.Forms.DataGridView tarifDataGridView;
        private System.Windows.Forms.Button yeniTarifEkleButton;
        private System.Windows.Forms.Button malzemeEkleButton;
        private System.Windows.Forms.Button tarifUpButton;
        private System.Windows.Forms.Button malzemeUpButton;
        private System.Windows.Forms.Button tarifSilButton;
        private System.Windows.Forms.Button malzemeSilButton;
        private System.Windows.Forms.Button tarifTablosunuGoruntuleButton;
        private System.Windows.Forms.Button malzemeTablosunuGoruntuleButton;
        private System.Windows.Forms.GroupBox tarifGroupBox;
        private System.Windows.Forms.GroupBox malzemeGroupBox;
        private System.Windows.Forms.TextBox aramaTextBox;
        private System.Windows.Forms.Button araButton;
        private System.Windows.Forms.CheckedListBox malzemeListBox;
        private Button malzemeAraButton;
     
        private void InitializeComponent()
        {
            this.tarifDataGridView = new System.Windows.Forms.DataGridView();
            this.yeniTarifEkleButton = new System.Windows.Forms.Button();
            this.malzemeEkleButton = new System.Windows.Forms.Button();
            this.tarifUpButton = new System.Windows.Forms.Button();
            this.malzemeUpButton = new System.Windows.Forms.Button();
            this.tarifSilButton = new System.Windows.Forms.Button();
            this.malzemeSilButton = new System.Windows.Forms.Button();
            this.tarifTablosunuGoruntuleButton = new System.Windows.Forms.Button();
            this.malzemeTablosunuGoruntuleButton = new System.Windows.Forms.Button();
            this.tarifGroupBox = new System.Windows.Forms.GroupBox();
            this.malzemeGroupBox = new System.Windows.Forms.GroupBox();
            this.aramaTextBox = new System.Windows.Forms.TextBox();
            this.araButton = new System.Windows.Forms.Button();
            this.malzemeListBox = new System.Windows.Forms.CheckedListBox();
            

            // 
            // aramaTextBox
            // 
            this.aramaTextBox.Location = new System.Drawing.Point(12, 20);
            this.aramaTextBox.Name = "aramaTextBox";
            this.aramaTextBox.Size = new System.Drawing.Size(300, 25);
            this.aramaTextBox.TabIndex = 10;
            this.aramaTextBox.PlaceholderText = "Tarif adına göre arayın...";

            // 
            // araButton
            // 
            this.araButton.Location = new System.Drawing.Point(320, 20);
            this.araButton.Name = "araButton";
            this.araButton.Size = new System.Drawing.Size(75, 25);
            this.araButton.TabIndex = 11;
            this.araButton.Text = "Ara";
            this.araButton.UseVisualStyleBackColor = true;
            this.araButton.Click += new System.EventHandler(this.AraButton_Click);

            // 
            // malzemeListBox
            // 
            this.malzemeListBox.Location = new System.Drawing.Point(420, 20);
            this.malzemeListBox.Name = "malzemeListBox";
            this.malzemeListBox.Size = new System.Drawing.Size(200, 150);
            this.malzemeListBox.TabIndex = 12;
            this.malzemeListBox.CheckOnClick = true;

            // 
            // tarifDataGridView
            // 
            this.tarifDataGridView.Location = new System.Drawing.Point(12, 180);
            this.tarifDataGridView.Name = "tarifDataGridView";
            this.tarifDataGridView.Size = new System.Drawing.Size(1000, 350);
            this.tarifDataGridView.TabIndex = 0;
            this.tarifDataGridView.ReadOnly = true;
            this.tarifDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // 
            // tarifGroupBox
            // 
            this.tarifGroupBox.Location = new System.Drawing.Point(12, 550);
            this.tarifGroupBox.Name = "tarifGroupBox";
            this.tarifGroupBox.Size = new System.Drawing.Size(500, 220);
            this.tarifGroupBox.TabIndex = 1;
            this.tarifGroupBox.TabStop = false;
            this.tarifGroupBox.Text = "Tarif İşlemleri";

            // 
            // yeniTarifEkleButton
            // 
            this.yeniTarifEkleButton.Location = new System.Drawing.Point(20, 30);
            this.yeniTarifEkleButton.Name = "yeniTarifEkleButton";
            this.yeniTarifEkleButton.Size = new System.Drawing.Size(220, 45);
            this.yeniTarifEkleButton.TabIndex = 2;
            this.yeniTarifEkleButton.Text = "Yeni Tarif Ekle";
            this.yeniTarifEkleButton.UseVisualStyleBackColor = true;
            this.yeniTarifEkleButton.Click += new System.EventHandler(this.yeniTarifEkleButton_Click);
            this.tarifGroupBox.Controls.Add(this.yeniTarifEkleButton);

            // 
            // tarifUpButton
            // 
            this.tarifUpButton.Location = new System.Drawing.Point(250, 30);
            this.tarifUpButton.Name = "tarifUpButton";
            this.tarifUpButton.Size = new System.Drawing.Size(220, 45);
            this.tarifUpButton.TabIndex = 3;
            this.tarifUpButton.Text = "Tarifi Güncelle";
            this.tarifUpButton.UseVisualStyleBackColor = true;
            this.tarifUpButton.Click += new System.EventHandler(this.TarifUpButton_Click);
            this.tarifGroupBox.Controls.Add(this.tarifUpButton);

            // 
            // tarifSilButton
            // 
            this.tarifSilButton.Location = new System.Drawing.Point(20, 100);
            this.tarifSilButton.Name = "tarifSilButton";
            this.tarifSilButton.Size = new System.Drawing.Size(220, 45);
            this.tarifSilButton.TabIndex = 4;
            this.tarifSilButton.Text = "Tarifi Sil";
            this.tarifSilButton.UseVisualStyleBackColor = true;
            this.tarifSilButton.Click += new System.EventHandler(this.TarifSilButton_Click);
            this.tarifGroupBox.Controls.Add(this.tarifSilButton);

            // 
            // tarifTablosunuGoruntuleButton
            // 
            this.tarifTablosunuGoruntuleButton.Location = new System.Drawing.Point(250, 100);
            this.tarifTablosunuGoruntuleButton.Name = "tarifTablosunuGoruntuleButton";
            this.tarifTablosunuGoruntuleButton.Size = new System.Drawing.Size(220, 45);
            this.tarifTablosunuGoruntuleButton.TabIndex = 5;
            this.tarifTablosunuGoruntuleButton.Text = "Tarif Tablosunu Aç";
            this.tarifTablosunuGoruntuleButton.UseVisualStyleBackColor = true;
            this.tarifTablosunuGoruntuleButton.Click += new System.EventHandler(this.TarifTablosunuGoruntuleButton_Click);
            this.tarifGroupBox.Controls.Add(this.tarifTablosunuGoruntuleButton);

            // 
            // malzemeGroupBox
            // 
            this.malzemeGroupBox.Location = new System.Drawing.Point(520, 550);
            this.malzemeGroupBox.Name = "malzemeGroupBox";
            this.malzemeGroupBox.Size = new System.Drawing.Size(500, 220);
            this.malzemeGroupBox.TabIndex = 2;
            this.malzemeGroupBox.TabStop = false;
            this.malzemeGroupBox.Text = "Malzeme İşlemleri";

            // 
            // malzemeEkleButton
            // 
            this.malzemeEkleButton.Location = new System.Drawing.Point(20, 30);
            this.malzemeEkleButton.Name = "malzemeEkleButton";
            this.malzemeEkleButton.Size = new System.Drawing.Size(220, 45);
            this.malzemeEkleButton.TabIndex = 6;
            this.malzemeEkleButton.Text = "Malzeme Ekle";
            this.malzemeEkleButton.UseVisualStyleBackColor = true;
            this.malzemeEkleButton.Click += new System.EventHandler(this.MalzemeEkleButton_Click);
            this.malzemeGroupBox.Controls.Add(this.malzemeEkleButton);

            // 
            // malzemeUpButton
            // 
            this.malzemeUpButton.Location = new System.Drawing.Point(250, 30);
            this.malzemeUpButton.Name = "malzemeUpButton";
            this.malzemeUpButton.Size = new System.Drawing.Size(220, 45);
            this.malzemeUpButton.TabIndex = 7;
            this.malzemeUpButton.Text = "Malzemeyi Güncelle";
            this.malzemeUpButton.UseVisualStyleBackColor = true;
            this.malzemeUpButton.Click += new System.EventHandler(this.MalzemeUpButton_Click);
            this.malzemeGroupBox.Controls.Add(this.malzemeUpButton);

            // 
            // malzemeSilButton
            // 
            this.malzemeSilButton.Location = new System.Drawing.Point(20, 100);
            this.malzemeSilButton.Name = "malzemeSilButton";
            this.malzemeSilButton.Size = new System.Drawing.Size(220, 45);
            this.malzemeSilButton.TabIndex = 8;
            this.malzemeSilButton.Text = "Malzemeyi Sil";
            this.malzemeSilButton.UseVisualStyleBackColor = true;
            this.malzemeSilButton.Click += new System.EventHandler(this.MalzemeSilButton_Click);
            this.malzemeGroupBox.Controls.Add(this.malzemeSilButton);

            // 
            // malzemeTablosunuGoruntuleButton
            // 
            this.malzemeTablosunuGoruntuleButton.Location = new System.Drawing.Point(250, 100);
            this.malzemeTablosunuGoruntuleButton.Name = "malzemeTablosunuGoruntuleButton";
            this.malzemeTablosunuGoruntuleButton.Size = new System.Drawing.Size(220, 45);
            this.malzemeTablosunuGoruntuleButton.TabIndex = 9;
            this.malzemeTablosunuGoruntuleButton.Text = "Malzeme Tablosunu Aç";
            this.malzemeTablosunuGoruntuleButton.UseVisualStyleBackColor = true;
            this.malzemeTablosunuGoruntuleButton.Click += new System.EventHandler(this.MalzemeTablosunuGoruntuleButton_Click);
            this.malzemeGroupBox.Controls.Add(this.malzemeTablosunuGoruntuleButton);


            // Malzeme Ara Butonu
            this.malzemeAraButton = new System.Windows.Forms.Button();
            this.malzemeAraButton.Location = new System.Drawing.Point(650, 50); // Uygun bir konum verin
            this.malzemeAraButton.Name = "malzemeAraButton";
            this.malzemeAraButton.Size = new System.Drawing.Size(75, 23);
            this.malzemeAraButton.TabIndex = 3;
            this.malzemeAraButton.Text = "Ara";
            this.malzemeAraButton.UseVisualStyleBackColor = true;
            this.malzemeAraButton.Click += new System.EventHandler(this.malzemeAraButton_Click);
            this.Controls.Add(this.malzemeAraButton);



            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 800);
            this.Controls.Add(this.aramaTextBox);
            this.Controls.Add(this.araButton);
            this.Controls.Add(this.malzemeListBox);
            this.Controls.Add(this.tarifDataGridView);
            this.Controls.Add(this.tarifGroupBox);
            this.Controls.Add(this.malzemeGroupBox);
            this.Controls.Add(this.malzemeAraButton);
            this.Name = "Form1";
            this.Text = "Tarif Uygulaması";
            ((System.ComponentModel.ISupportInitialize)(this.tarifDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
