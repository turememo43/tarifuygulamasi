CREATE TABLE IF NOT EXISTS Tarifler (
    TarifID INTEGER PRIMARY KEY AUTOINCREMENT,
    TarifAdi VARCHAR(255),
    Kategori VARCHAR(255),
    HazirlamaSuresi INTEGER,
    Talimatlar TEXT
);

CREATE TABLE IF NOT EXISTS Malzemeler (
    MalzemeID INTEGER PRIMARY KEY AUTOINCREMENT,
    MalzemeAdi VARCHAR(255),
    ToplamMiktar VARCHAR(255),
    MalzemeBirim VARCHAR(50),
    BirimFiyat DECIMAL(10, 2)
);

CREATE TABLE IF NOT EXISTS TarifMalzeme (
    TarifID INTEGER,
    MalzemeID INTEGER,
    MalzemeMiktar FLOAT,
    FOREIGN KEY (TarifID) REFERENCES Tarifler(TarifID),
    FOREIGN KEY (MalzemeID) REFERENCES Malzemeler(MalzemeID)
);
