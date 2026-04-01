#nullable disable
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModernCafePOS;

/// <summary>
/// Form1 partial: Masalar sekmesi — salon görünümü, masa kartları oluşturma ve masa tıklama olayları.
/// </summary>
public partial class Form1
{
    private void BuildUI()
    {
        // 1. Sekme Kontrolcüsünü Kur (TabControl)
        tabControl = new MaterialTabControl();
        tabControl.Dock = DockStyle.Fill;
        tabControl.Depth = 0;
        tabControl.MouseState = MouseState.HOVER;

        // 2. Sekmeler (TabPages)
        tabMasalar = new TabPage(Localization.Get("TabTables"));
        tabMasalar.BackColor = RenkArkaPlan;
        tabMasalar.Tag = "Bg";
        tabControl.Controls.Add(tabMasalar);

        tabSiparisler = new TabPage(Localization.Get("TabOrders"));
        tabSiparisler.BackColor = RenkArkaPlan;
        tabSiparisler.Tag = "Bg";
        tabControl.Controls.Add(tabSiparisler);

        tabRaporlar = new TabPage(Localization.Get("TabReports"));
        tabRaporlar.BackColor = RenkArkaPlan;
        tabRaporlar.Tag = "Bg";
        tabControl.Controls.Add(tabRaporlar);

        tabYonetim = new TabPage(Localization.Get("TabManagement"));
        tabYonetim.BackColor = RenkArkaPlan;
        tabYonetim.Tag = "Bg";
        tabControl.Controls.Add(tabYonetim);

        // 3. Masalar üstü Navigasyon - TableLayoutPanel ile kesin hizalama (Sıkışmayı Önler)
        TableLayoutPanel tlpNav = new TableLayoutPanel();
        tlpNav.Dock = DockStyle.Top;
        tlpNav.Height = 70;
        tlpNav.BackColor = RenkUstPanel;
        tlpNav.Tag = "UstPanel";
        tlpNav.ColumnCount = 4;
        tlpNav.RowCount = 1;
        tlpNav.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // GÜN SONU
        tlpNav.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));  // Orta (Salon Görünümü)
        tlpNav.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // ÜRÜN YÖNETİMİ
        tlpNav.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // ÇIKIŞ YAP
        tabMasalar.Controls.Add(tlpNav);

        // SOL: Gün Sonu (En Solda)
        btnDayEnd = new MaterialButton { 
            Text = Localization.Get("BtnDayEnd"), 
            Type = MaterialButton.MaterialButtonType.Contained, 
            UseAccentColor = true, 
            Anchor = AnchorStyles.Left, 
            Margin = new Padding(15, 0, 0, 0) 
        };
        btnDayEnd.Click += (s, e) => {
            RaporVerileriniYenile();
            tabControl.SelectedTab = tabRaporlar; 
        };
        tlpNav.Controls.Add(btnDayEnd, 0, 0);

        // SAĞ: Çıkış Yap
        btnLogout = new MaterialButton { 
            Text = Localization.Get("BtnLogout"), 
            Type = MaterialButton.MaterialButtonType.Outlined, 
            Anchor = AnchorStyles.Right, 
            Margin = new Padding(0, 0, 15, 0) 
        };
        btnLogout.Click += (s, e) => {
            pnlLogin.Visible = true;
            tabControl.SelectedTab = tabMasalar;
        };
        tlpNav.Controls.Add(btnLogout, 3, 0);

        // ÜRÜN YÖNETİMİ butonu (Durum çubuğundan taşındı — üst barda rahat yerleşim)
        btnYonetimGecis = new MaterialButton { 
            Text = "⚙️ " + Localization.Get("TabManagement"), 
            Type = MaterialButton.MaterialButtonType.Outlined, 
            AutoSize = true,
            Anchor = AnchorStyles.Right, 
            Margin = new Padding(0, 0, 10, 0) 
        };
        btnYonetimGecis.Click += (s, ev) => {
            if (pnlLogin.Visible) return; 
            YonetimVerileriniYenile();
            tabControl.SelectedTab = tabYonetim;
        };
        tlpNav.Controls.Add(btnYonetimGecis, 2, 0);

        // ORTA: Merkezi Yönetim Paneli (TableLayoutPanel'in orta hücresine ekliyoruz)
        TableLayoutPanel pnlMerkezBaslik = new TableLayoutPanel();
        pnlMerkezBaslik.AutoSize = true;
        pnlMerkezBaslik.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pnlMerkezBaslik.ColumnCount = 3;
        pnlMerkezBaslik.RowCount = 1;
        pnlMerkezBaslik.BackColor = Color.Transparent;
        pnlMerkezBaslik.Anchor = AnchorStyles.None; // Hücre içinde tam merkezleme
        pnlMerkezBaslik.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlMerkezBaslik.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlMerkezBaslik.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpNav.Controls.Add(pnlMerkezBaslik, 1, 0);

        MaterialButton btnMasaSil = new MaterialButton {
            Text = "➖",
            Type = MaterialButton.MaterialButtonType.Text,
            AutoSize = true,
            DrawShadows = false,
            HighEmphasis = false,
            Margin = new Padding(0, 0, 10, 0),
            Cursor = Cursors.Hand
        };
        btnMasaSil.Click += (s, e) => MasaSil();
        pnlMerkezBaslik.Controls.Add(btnMasaSil, 0, 0);

        lblSalonView = new Label {
            Text = string.Format(Localization.Get("SalonView"), DatabaseManager.GetTableCount()),
            ForeColor = RenkSolukYazi,
            Tag = "SolukYazi",
            Font = new Font("Segoe UI Semibold", 12f),
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleCenter,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0)
        };
        pnlMerkezBaslik.Controls.Add(lblSalonView, 1, 0);

        MaterialButton btnMasaEkle = new MaterialButton {
            Text = "➕",
            Type = MaterialButton.MaterialButtonType.Text,
            AutoSize = true,
            DrawShadows = false,
            HighEmphasis = false,
            Margin = new Padding(15, 0, 0, 0),
            Cursor = Cursors.Hand
        };
        btnMasaEkle.Click += (s, e) => MasaEkle();
        pnlMerkezBaslik.Controls.Add(btnMasaEkle, 2, 0);
        
        pnlMerkezBaslik.BringToFront();

        // 4. Masalar Paneli
        pnlMasalar = new FlowLayoutPanel();
        pnlMasalar.Dock = DockStyle.Fill;
        pnlMasalar.AutoScroll = true;
        pnlMasalar.Padding = new Padding(20);
        pnlMasalar.BackColor = RenkArkaPlan;
        pnlMasalar.Tag = "Bg";
        tabMasalar.Controls.Add(pnlMasalar);
        
        pnlMasalar.BringToFront();

        // 5. Masaları Veritabanından Yükle
        int masaSayisi = DatabaseManager.GetTableCount();
        for (int i = 1; i <= masaSayisi; i++)
        {
            Panel masaKart = MasaKartiOlustur(i);
            pnlMasalar.Controls.Add(masaKart);
        }

        // Form üzerine sekmeleri ekle
        this.Controls.Add(tabControl);
    }

    private void MasaEkle()
    {
        DatabaseManager.AddTable();
        int yeniMasaNo = DatabaseManager.GetTableCount();
        Panel yeniMasa = MasaKartiOlustur(yeniMasaNo);
        pnlMasalar.Controls.Add(yeniMasa);
        UpdateAllTexts();
    }

    private void MasaSil()
    {
        int total = DatabaseManager.GetTableCount();
        if (total <= 1) return;

        // Son masanın dolu olup olmadığını kontrol et (İsteğe bağlı, güvenlik için)
        Control[] kartlar = pnlMasalar.Controls.Find($"masaKart{total}", false);
        if (kartlar.Length > 0 && kartlar[0].Tag.ToString() == "DOLU")
        {
            MessageBox.Show(Localization.Current == Language.TR ? "Dolu olan bir masayı silemezsiniz!" : "You cannot delete an occupied table!", 
                "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DatabaseManager.RemoveLastTable();
        if (kartlar.Length > 0) pnlMasalar.Controls.Remove(kartlar[0]);
        UpdateAllTexts();
    }

    /// <summary>
    /// Tek bir masa kartı paneli oluşturur. GDI+ ile gradient arka plan,
    /// yuvarlatılmış köşe ve durum rengi (Yeşil=Boş, Kırmızı=Dolu) çizer.
    /// </summary>
    private Panel MasaKartiOlustur(int masaNo)
    {
        Panel kart = new Panel();
        kart.Size = new Size(155, 110);
        kart.Margin = new Padding(10);
        kart.Name = $"masaKart{masaNo}";
        kart.Tag = "BOŞ";
        kart.Cursor = Cursors.Hand;
        kart.BackColor = Color.Transparent;

        kart.Paint += (s, e) =>
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, kart.Width - 1, kart.Height - 1);
            
            bool dolu = kart.Tag.ToString() == "DOLU";
            Color renk1 = dolu ? Color.FromArgb(183, 28, 28) : Color.FromArgb(27, 94, 32);
            Color renk2 = dolu ? Color.FromArgb(239, 83, 80) : Color.FromArgb(76, 175, 80);
            
            using (GraphicsPath path = YuvarlatilmisKare(rect, 16))
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, renk2, renk1, 90f))
            {
                g.FillPath(brush, path);
            }

            // Masa numarası
            using (Font font = new Font("Segoe UI", 16f, FontStyle.Bold))
            {
                string textNo = string.Format(Localization.Get("TableLabel"), masaNo);
                SizeF sz = g.MeasureString(textNo, font);
                g.DrawString(textNo, font, Brushes.White, (kart.Width - sz.Width) / 2, 18);
            }

            // Durum yazısı
            string textEmpty = Localization.Get("TableEmpty");
            string textFull = Localization.Get("TableFull");
            string durum = dolu ? "● " + textFull : "○ " + textEmpty;
            
            using (Font font2 = new Font("Segoe UI", 11f, FontStyle.Bold))
            {
                SizeF sz2 = g.MeasureString(durum, font2);
                Color durumRenk = dolu ? Color.FromArgb(255, 205, 210) : Color.FromArgb(200, 230, 201);
                using (SolidBrush br = new SolidBrush(durumRenk))
                {
                    g.DrawString(durum, font2, br, (kart.Width - sz2.Width) / 2, 65);
                }
            }
        };

        kart.Click += (s, e) => MasaTiklandi(masaNo, kart);
        kart.MouseEnter += (s, e) => { kart.BackColor = Color.FromArgb(20, 255, 255, 255); kart.Invalidate(); };
        kart.MouseLeave += (s, e) => { kart.BackColor = Color.Transparent; kart.Invalidate(); };

        return kart;
    }

    private void MasaTiklandi(int masaNo, Panel kart)
    {
        string masaAdi = string.Format(Localization.Get("TableLabel"), masaNo);

        if (kart.Tag.ToString() == "BOŞ")
        {
            kart.Tag = "DOLU";
            kart.Invalidate();
        }

        aktifMasa = masaAdi;
        lblSeciliMasa.Text = string.Format(Localization.Get("OrderTitle"), aktifMasa);
        tabControl.SelectedTab = tabSiparisler;
    }
}
