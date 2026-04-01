#nullable disable
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModernCafePOS;

/// <summary>
/// Ana form sınıfı - partial class olarak ekran bazlı dosyalara ayrılmıştır.
/// Bu dosya: Constructor, fieldlar, tema ayarları, durum çubuğu ve ortak yardımcılar.
/// </summary>
public partial class Form1 : MaterialForm
{
    // Sekme kontrolleri
    private MaterialTabControl tabControl;
    private TabPage tabMasalar;
    private TabPage tabSiparisler;
    private TabPage tabRaporlar;
    private TabPage tabYonetim;
    private FlowLayoutPanel pnlMasalar;
    
    // Yonetim UI references
    private MaterialListView listYonetimUrunler;
    private MaterialTextBox txtUrunAd;
    private MaterialTextBox txtUrunFiyat;
    private ComboBox cmbKategori;
    private MaterialButton btnUrunKaydet;
    private MaterialButton btnUrunSil;
    private MaterialTextBox txtYeniKategori;
    private MaterialButton btnKategoriEkle;
    private MaterialButton btnYonetimGecis;
    private MaterialButton btnYonetimGeri;
    private Label lblYonetimBaslik;
    private Label lblYonetimUrunBaslik;
    private Label lblYonetimKategoriSec;
    private Label lblYonetimKatBaslik;
    
    // Sipariş UI Bileşenleri
    private FlowLayoutPanel pnlUrunler;
    private MaterialListView listAdisyon;
    private MaterialLabel lblToplamTutar;
    private MaterialLabel lblSeciliMasa;
    private MaterialLabel lblCiro;
    private MaterialLabel lblKasaToplam;
    private string aktifMasa = "";
    private decimal toplamHesap = 0;
    private List<Product> tumUrunler;
    
    // Login UI
    private Panel pnlLogin;

    // Durum çubuğu
    private Label lblSaat;
    private Label lblKullanici;
    private System.Windows.Forms.Timer tmrSaat;

    // Rapor - sipariş geçmişi
    private MaterialListView listSiparisGecmisi;
    private MaterialLabel lblSiparisAdet;
    private MaterialLabel lblBaslikRapor;
    private MaterialButton btnKapatRapor;
    private MaterialButton btnKasaDuzenle;
    private Label lblGecmisBaslik;
    
    // Order UI references
    private MaterialButton btnGeri;
    private MaterialButton btnOde;
    private MaterialButton btnIptal;
    
    // Login UI references
    private Label lblLoginLogo;
    private Label lblLoginTitle;
    private Label lblLoginAltBaslik;
    private MaterialTextBox txtPin;
    private MaterialButton btnLogin;
    private MaterialButton btnDemo;
    private Label lblIpucu;

    // Status bar references
    private MaterialButton btnDilSec;
    private MaterialButton btnDayEnd;
    private MaterialButton btnLogout;
    private Label lblSalonView;
    private MaterialSwitch swTema;
    private FlowLayoutPanel pnlKategoriler;

    // Context Menu references
    private ContextMenuStrip ctxAdisyon;
    private ToolStripMenuItem menuCikar;
    private ToolStripMenuItem menuTümünüSil;

    // ──────────────────────────────────────────────
    //  TEMA VE RENK YÖNETİMİ
    // ──────────────────────────────────────────────
    internal bool IsDarkTheme => materialSkinManager != null && materialSkinManager.Theme == MaterialSkinManager.Themes.DARK;

    internal Color RenkArkaPlan => IsDarkTheme ? Color.FromArgb(18, 18, 18) : Color.FromArgb(245, 245, 245);
    internal Color RenkKart => IsDarkTheme ? Color.FromArgb(30, 30, 30) : Color.White;
    internal Color RenkKartAcik => IsDarkTheme ? Color.FromArgb(45, 45, 45) : Color.FromArgb(250, 250, 250);
    internal Color RenkUstPanel => IsDarkTheme ? Color.FromArgb(25, 25, 25) : Color.FromArgb(230, 230, 230);
    
    internal Color RenkYesil => IsDarkTheme ? Color.FromArgb(76, 175, 80) : Color.FromArgb(46, 125, 50);
    internal Color RenkKirmizi => IsDarkTheme ? Color.FromArgb(239, 83, 80) : Color.FromArgb(198, 40, 40);
    
    internal Color RenkBeyazYazi => IsDarkTheme ? Color.FromArgb(255, 255, 255) : Color.FromArgb(33, 33, 33);
    internal Color RenkSolukYazi => IsDarkTheme ? Color.FromArgb(170, 170, 170) : Color.FromArgb(100, 100, 100);
    internal Color RenkVurgu => IsDarkTheme ? Color.FromArgb(100, 181, 246) : Color.FromArgb(30, 136, 229);

    internal MaterialSkinManager materialSkinManager;

    // Kategori emojileri
    internal static readonly Dictionary<string, string> KategoriEmojileri = new()
    {
        { "Sıcak İçecekler", "☕" },
        { "Soğuk İçecekler", "🧊" },
        { "Tatlılar", "🍰" },
        { "Ana Yemekler", "🍽️" },
        { "Aperatifler", "🍟" },
        { "Dondurma", "🍦" }
    };

    // Her yiyecek ismine özel emoji tespit edici (Kullanıcı Talebi: Sürekli aynı ürün emojisi çıkmasını engelleme)
    internal static string GetProductEmoji(string productName, string categoryEmoji)
    {
        string n = productName.ToLower();
        if (n.Contains("çay")) return "🍵";
        if (n.Contains("kahve") || n.Contains("latte") || n.Contains("mocha") || n.Contains("espresso") || n.Contains("americano")) return "☕";
        if (n.Contains("çikolata")) return "🍫";
        if (n.Contains("limonata")) return "🍋";
        if (n.Contains("kola") || n.Contains("fanta") || n.Contains("sprite") || n.Contains("kutu") || n.Contains("buzlu")) return "🥤";
        if (n.Contains("portakal")) return "🍊";
        if (n.Contains("su") || n.Contains("maden")) return "💧";
        if (n.Contains("cheesecake") || n.Contains("kek")) return "🍰";
        if (n.Contains("sufle") || n.Contains("profiterol")) return "🧁";
        if (n.Contains("brownie") || n.Contains("tiramisu") || n.Contains("marlenka") || n.Contains("san sebastian")) return "🍮";
        if (n.Contains("tavuk")) return "🍗";
        if (n.Contains("spagetti") || n.Contains("penne") || n.Contains("fettuccine")) return "🍝";
        if (n.Contains("köfte")) return "🥩";
        if (n.Contains("hamburger") || n.Contains("burger")) return "🍔";
        if (n.Contains("patates")) return "🍟";
        if (n.Contains("soğan")) return "🧅";
        if (n.Contains("sosis")) return "🌭";
        if (n.Contains("tost")) return "🥪";
        if (n.Contains("dondurma") || n.Contains("ice cream") || n.Contains("gelato")) return "🍦";
        if (n.Contains("börek")) return "🥟";
        return categoryEmoji; // Varsayılan kategori emojisine geri dön
    }

    // ──────────────────────────────────────────────
    //  CONSTRUCTOR
    // ──────────────────────────────────────────────
    public Form1()
    {
        InitializeComponent();

        // Veritabanını Hazırla
        DatabaseManager.InitializeDatabase();

        // MaterialSkinManager - THEME
        materialSkinManager = MaterialSkinManager.Instance;
        materialSkinManager.AddFormToManage(this);
        materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
        materialSkinManager.ColorScheme = new ColorScheme(
            Primary.Indigo500, Primary.Indigo700, 
            Primary.Indigo100, Accent.Pink200, 
            TextShade.WHITE);
            
        this.Text = "☕ Modern Cafe POS";
        this.Size = new Size(1100, 768);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.MinimumSize = new Size(900, 600);

        BuildUI();          // Form1.Masalar.cs
        BuildSiparisUI();   // Form1.Siparis.cs
        BuildRaporUI();     // Form1.Rapor.cs
        BuildYonetimUI();   // Form1.Yonetim.cs
        BuildLoginUI();     // Form1.Login.cs
        BuildDurumCubugu();
        BaslatSaat();
        
        TemaUygula();
    }

    private void TemaUygula()
    {
        // Temaya göre arkaplan renklerini yenile
        if (tabMasalar != null) tabMasalar.BackColor = RenkArkaPlan;
        if (tabSiparisler != null) tabSiparisler.BackColor = RenkArkaPlan;
        if (tabRaporlar != null) tabRaporlar.BackColor = RenkArkaPlan;
        if (pnlLogin != null) pnlLogin.BackColor = RenkArkaPlan;

        if (lblSaat != null) lblSaat.ForeColor = RenkSolukYazi;
        
        // Context Menu styling
        if (ctxAdisyon != null)
        {
            ctxAdisyon.BackColor = RenkKart;
            ctxAdisyon.ForeColor = RenkBeyazYazi;
            if (menuCikar != null) menuCikar.ForeColor = RenkBeyazYazi;
            if (menuTümünüSil != null) menuTümünüSil.ForeColor = RenkBeyazYazi;
            ctxAdisyon.RenderMode = ToolStripRenderMode.System;
        }

        YenileArkaplan(this);
        UpdateAllTexts();
        this.Invalidate(true);
    }
    
    /// <summary>
    /// Bütün ekranlardaki metinleri seçili dile göre günceller.
    /// </summary>
    private void UpdateAllTexts()
    {
        // Login Ekranı
        if (lblLoginTitle != null) lblLoginTitle.Text = Localization.Get("LoginTitle");
        if (lblLoginAltBaslik != null) lblLoginAltBaslik.Text = Localization.Get("LoginAlt");
        if (txtPin != null) txtPin.Hint = Localization.Get("LoginPinHint");
        if (btnLogin != null) btnLogin.Text = Localization.Get("LoginBtn");
        if (btnDemo != null) btnDemo.Text = Localization.Get("LoginDemo");
        if (lblIpucu != null) lblIpucu.Text = Localization.Current == Language.TR ? "Demo PIN: 1234" : "Demo PIN: 1234";

        // Masalar Ekranı (Form1.Masalar.cs içindeki referanslar lazımsa oraya taşınabilir, ama şimdilik burada)
        // Not: Masalardaki BOŞ/DOLU yazıları Invalidate/Paint içinde halledilebilir.
        
        // Sipariş Ekranı
        if (lblSeciliMasa != null) 
            lblSeciliMasa.Text = string.IsNullOrEmpty(aktifMasa) ? Localization.Get("OrderNotSelected") : string.Format(Localization.Get("OrderTitle"), aktifMasa);
        if (lblToplamTutar != null) 
            lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), "₺" + toplamHesap.ToString("N2"));
        
        // Rapor Ekranı
        if (lblBaslikRapor != null) lblBaslikRapor.Text = Localization.Get("ReportTitle");
        if (btnKapatRapor != null) btnKapatRapor.Text = Localization.Get("CloseScreen");
        if (lblCiro != null) lblCiro.Text = string.Format(Localization.Get("DailyRevenue"), "₺" + DatabaseManager.GetDailyTotal().ToString("N2"));
        if (lblKasaToplam != null) {
            decimal total = DatabaseManager.GetTotalRevenue() + DatabaseManager.GetInitialCash();
            lblKasaToplam.Text = string.Format(Localization.Get("GeneralCash"), "₺" + total.ToString("N2"));
        }
        if (lblSiparisAdet != null) lblSiparisAdet.Text = string.Format(Localization.Get("OrderCount"), DatabaseManager.GetDailyOrderCount());
        if (lblGecmisBaslik != null) lblGecmisBaslik.Text = Localization.Get("RecentOrders");
        if (btnKasaDuzenle != null) btnKasaDuzenle.Text = Localization.Get("Edit");

        // Rapor listesi başlıkları
        if (listSiparisGecmisi != null) {
            listSiparisGecmisi.Columns[0].Text = Localization.Get("ColTable");
            listSiparisGecmisi.Columns[1].Text = Localization.Get("ColTotal");
            listSiparisGecmisi.Columns[2].Text = Localization.Get("ColDate");
            RaporVerileriniYenile(); // Verileri de tekrar çek (Masa 1 -> Table 1 olması için)
        }

        // Sipariş listesi başlıkları
        if (listAdisyon != null) {
            listAdisyon.Columns[0].Text = Localization.Get("ColProduct");
            listAdisyon.Columns[1].Text = Localization.Get("ColQty");
            listAdisyon.Columns[2].Text = Localization.Get("ColPrice");
        }

        if (btnGeri != null) btnGeri.Text = Localization.Get("BackToTables");
        if (btnOde != null) btnOde.Text = Localization.Get("PayButton");
        if (btnIptal != null) btnIptal.Text = Localization.Get("CancelTable");

        if (menuCikar != null) menuCikar.Text = Localization.Get("MenuRemoveOne");
        if (menuTümünüSil != null) menuTümünüSil.Text = Localization.Get("MenuRemoveAll");

        if (swTema != null) swTema.Text = swTema.Checked ? Localization.Get("LightMode") : Localization.Get("DarkMode");
        if (lblKullanici != null) lblKullanici.Text = Localization.Get("CashierActive");
        if (btnDilSec != null) btnDilSec.Text = string.Format(Localization.Get("LanguageSwitch"), Localization.Current.ToString());
        
        // Kategorileri yeniden yükle (Dillerini çevirmek için)
        if (pnlKategoriler != null) UrunleriDinamikYukle(pnlKategoriler);

        // Masalar Ekranı Üst Panel
        if (tabMasalar != null) tabMasalar.Text = Localization.Get("TabTables");
        if (tabSiparisler != null) tabSiparisler.Text = Localization.Get("TabOrders");
        if (tabRaporlar != null) tabRaporlar.Text = Localization.Get("TabReports");
        if (btnDayEnd != null) btnDayEnd.Text = Localization.Get("BtnDayEnd");
        if (btnLogout != null) btnLogout.Text = Localization.Get("BtnLogout");
        if (lblSalonView != null) lblSalonView.Text = string.Format(Localization.Get("SalonView"), DatabaseManager.GetTableCount());
        
        // Yönetim Ekranı Metinleri
        if (tabYonetim != null) tabYonetim.Text = Localization.Get("TabManagement");
        if (btnYonetimGecis != null) btnYonetimGecis.Text = "⚙️ " + Localization.Get("TabManagement");
        if (btnUrunKaydet != null) btnUrunKaydet.Text = Localization.Get("BtnSave");
        if (btnUrunSil != null) btnUrunSil.Text = Localization.Get("BtnDelete");
        if (btnKategoriEkle != null) btnKategoriEkle.Text = Localization.Get("BtnSave");
        if (txtUrunAd != null) txtUrunAd.Hint = Localization.Get("NameLabel");
        if (txtUrunFiyat != null) txtUrunFiyat.Hint = Localization.Get("PriceLabel");
        if (txtYeniKategori != null) txtYeniKategori.Hint = Localization.Get("CategoryAdd");

        if (listYonetimUrunler != null)
        {
            listYonetimUrunler.Columns[0].Text = Localization.Get("ColProduct");
            listYonetimUrunler.Columns[1].Text = Localization.Get("ColPrice");
            listYonetimUrunler.Columns[2].Text = Localization.Get("SelectCategory");
        }

        // Yönetim ekranı label/butonları (yerel değişkenlerden field'a çevrildiler)
        if (btnYonetimGeri != null) btnYonetimGeri.Text = Localization.Get("BackToTables");
        if (lblYonetimBaslik != null) lblYonetimBaslik.Text = "⚙️ " + Localization.Get("TabManagement");
        if (lblYonetimUrunBaslik != null) lblYonetimUrunBaslik.Text = Localization.Get("NewProduct");
        if (lblYonetimKategoriSec != null) lblYonetimKategoriSec.Text = Localization.Get("SelectCategory");
        if (lblYonetimKatBaslik != null) lblYonetimKatBaslik.Text = Localization.Get("CategoryAdd");
        YonetimVerileriniYenile(); // Ürün listesini dile göre tazele
        
        // Aktif masa başlığını (Adisyon başlığı) dile göre tazele
        if (!string.IsNullOrEmpty(aktifMasa)) {
            string numOnly = new string(aktifMasa.Where(char.IsDigit).ToArray());
            if (!string.IsNullOrEmpty(numOnly)) {
                lblSeciliMasa.Text = string.Format(Localization.Get("OrderTitle"), string.Format(Localization.Get("TableLabel"), numOnly));
            }
        } else {
            lblSeciliMasa.Text = Localization.Get("OrderNotSelected");
        }
        
        // Masa kartları ve butonlar gibi dinamik çizilenler Invalidate ile düzelir
        if (pnlMasalar != null) pnlMasalar.Invalidate(true);
    }



    private void YenileArkaplan(Control parent)
    {
        foreach(Control c in parent.Controls)
        {
            if(c is Panel || c is TableLayoutPanel || c is FlowLayoutPanel || c is TabPage)
            {
                if (c.Name == "pnlLoginCard") { } // skip specific manually drawn if any
                else if (c.Tag != null)
                {
                    string t = c.Tag.ToString();
                    if (t.Contains("Bg")) c.BackColor = RenkArkaPlan;
                    else if (t.Contains("KartAcik")) c.BackColor = RenkKartAcik;
                    else if (t.Contains("Kart")) c.BackColor = RenkKart;
                    else if (t.Contains("UstPanel")) c.BackColor = RenkUstPanel;
                }
                c.Invalidate();
            }
            if (c is Label lbl && lbl.Tag != null)
            {
                if (lbl.Tag.ToString().Contains("SolukYazi"))
                    lbl.ForeColor = RenkSolukYazi;
                
                if (lbl.Tag.ToString().Contains("BeyazYazi"))
                    lbl.ForeColor = RenkBeyazYazi;

                // MaterialSkin varsayılan fontları ezdiği için Login ekranı fontlarını tema değişiminde geri yüklüyoruz.
                if (lbl.Name == "lblLoginLogo") lbl.Font = new Font("Segoe UI Emoji", 36f);
                else if (lbl.Name == "lblLoginTitle") lbl.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
                else if (lbl.Name == "lblLoginAltBaslik") lbl.Font = new Font("Segoe UI", 9f);
            }
            YenileArkaplan(c);
        }
    }

    // ──────────────────────────────────────────────
    //  DURUM ÇUBUĞU & SAAT
    // ──────────────────────────────────────────────
    private void BaslatSaat()
    {
        tmrSaat = new System.Windows.Forms.Timer();
        tmrSaat.Interval = 1000;
        tmrSaat.Tick += (s, e) =>
        {
            if (lblSaat != null)
                lblSaat.Text = $"🕐 {DateTime.Now:dd.MM.yyyy  HH:mm:ss}";
            };
        tmrSaat.Start();
    }

    private void BuildDurumCubugu()
    {
        Panel pnlDurum = new Panel();
        pnlDurum.Dock = DockStyle.Bottom;
        pnlDurum.Height = 40;
        pnlDurum.BackColor = Color.Transparent; // Şeffaf bırak, material skin halletsin
        pnlDurum.Padding = new Padding(10, 0, 10, 0);

        // Tema Toggle
        swTema = new MaterialSwitch();
        swTema.Text = Localization.Get("DarkMode");
        swTema.Dock = DockStyle.Right;
        swTema.AutoSize = true;
        swTema.Checked = false; // Varsayılan Light
        swTema.CheckedChanged += (s, e) => {
            materialSkinManager.Theme = swTema.Checked ? MaterialSkinManager.Themes.DARK : MaterialSkinManager.Themes.LIGHT;
            swTema.Text = swTema.Checked ? Localization.Get("LightMode") : Localization.Get("DarkMode");
            
            if(materialSkinManager.Theme == MaterialSkinManager.Themes.DARK) {
                // Karanlık modda, hem Outlined (şartsız dış çizgili) hem de Contained (seçili dolu) butonların 
                // kusursuz okunup şık görünmesi için Saf Gök Mavisi (Blue500) ve Beyaz yazı (TextShade.WHITE) kombinasyonu kuruyoruz.
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Blue500, Primary.Blue700, Primary.Blue200, Accent.LightBlue200, TextShade.WHITE);
            } else {
                // Aydınlık modda kurumsal ve ferah Indigo (Lacivert-Mor tonu)
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
            }
            TemaUygula();
        };
        pnlDurum.Controls.Add(swTema);

        lblSaat = new Label();
        lblSaat.Text = $"🕐 {DateTime.Now:dd.MM.yyyy  HH:mm:ss}";
        lblSaat.ForeColor = RenkSolukYazi;
        lblSaat.Tag = "SolukYazi";
        lblSaat.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
        lblSaat.Dock = DockStyle.Right;
        lblSaat.AutoSize = false;
        lblSaat.Width = 200;
        lblSaat.TextAlign = ContentAlignment.MiddleRight;
        pnlDurum.Controls.Add(lblSaat);

        btnDilSec = new MaterialButton();
        btnDilSec.Text = string.Format(Localization.Get("LanguageSwitch"), "TR");
        btnDilSec.Type = MaterialButton.MaterialButtonType.Text;
        btnDilSec.Dock = DockStyle.Left;
        btnDilSec.AutoSize = true;
        btnDilSec.Click += (s, ev) => {
            Localization.Toggle();
            UpdateAllTexts();
        };
        pnlDurum.Controls.Add(btnDilSec);

        lblKullanici = new Label();
        lblKullanici.Text = Localization.Get("CashierActive");
        lblKullanici.ForeColor = RenkYesil;
        lblKullanici.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        lblKullanici.Dock = DockStyle.Left;
        lblKullanici.AutoSize = false;
        lblKullanici.Width = 220;
        lblKullanici.Padding = new Padding(10, 0, 0, 0);
        lblKullanici.TextAlign = ContentAlignment.MiddleLeft;
        pnlDurum.Controls.Add(lblKullanici);

        this.Controls.Add(pnlDurum);
        
        // Önemli: tabControl (Dock.Fill) olan ana konteynerin durum çubuğunun altında kalmaması için
        // durum çubuğunun yer kaplamasını sağlıyoruz. tabControl'ü arkaya gönderiyoruz.
        if (tabControl != null) tabControl.SendToBack();
        if (pnlLogin != null) pnlLogin.BringToFront(); // Login her zaman en üstte kalmalı
    }

    // ──────────────────────────────────────────────
    //  ORTAK YARDIMCI METODLAR
    // ──────────────────────────────────────────────
    
    /// <summary>
    /// GDI+ ile yuvarlatılmış köşeli dikdörtgen oluşturur.
    /// Masa kartları ve ürün kartları tarafından ortaklaşa kullanılır.
    /// </summary>
    internal GraphicsPath YuvarlatilmisKare(Rectangle bounds, int radius)
    {
        int d = radius * 2;
        GraphicsPath path = new GraphicsPath();
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// Masa kartının görsel durumunu günceller (BOŞ/DOLU).
    /// Sipariş iptal ve ödeme sonrası çağrılır.
    /// </summary>
    private void MasaDurumGuncelle(string masaAdi, bool bos)
    {
        // "MASA 5" veya "TABLE 5" içerisinden sadece rakamları al
        string numStr = new string(masaAdi.Where(char.IsDigit).ToArray());
        Control[] kartlar = pnlMasalar.Controls.Find($"masaKart{numStr}", false);
        if (kartlar.Length > 0 && kartlar[0] is Panel kart)
        {
            kart.Tag = bos ? "BOŞ" : "DOLU";
            kart.Invalidate();
        }
    }
}
