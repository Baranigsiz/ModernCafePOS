#nullable disable
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModernCafePOS;

/// <summary>
/// Form1 partial: Sipariş sekmesi — kategori navigasyonu, ürün kartları, adisyon ve ödeme/iptal işlemleri.
/// </summary>
public partial class Form1
{
    private void BuildSiparisUI()
    {
        TableLayoutPanel tlpSiparis = new TableLayoutPanel();
        tlpSiparis.Dock = DockStyle.Fill;
        tlpSiparis.ColumnCount = 2;
        tlpSiparis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpSiparis.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 360F));
        tlpSiparis.RowCount = 1;
        tlpSiparis.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpSiparis.BackColor = RenkArkaPlan;
        tlpSiparis.Tag = "Bg";
        tabSiparisler.Controls.Add(tlpSiparis);

        // ── Sol Panel: Kategoriler + Ürünler ──
        Panel pnlSolUst = new Panel();
        pnlSolUst.Dock = DockStyle.Fill;
        pnlSolUst.BackColor = RenkArkaPlan;
        pnlSolUst.Tag = "Bg";
        tlpSiparis.Controls.Add(pnlSolUst, 0, 0);

        pnlKategoriler = new FlowLayoutPanel();
        pnlKategoriler.Dock = DockStyle.Top;
        pnlKategoriler.AutoSize = true;
        pnlKategoriler.MaximumSize = new Size(0, 120); // Maks ~2 satır, taşarsa scroll
        pnlKategoriler.AutoScroll = true;
        pnlKategoriler.WrapContents = true;
        pnlKategoriler.Padding = new Padding(10, 8, 10, 6);
        pnlKategoriler.BackColor = RenkUstPanel;
        pnlKategoriler.Tag = "UstPanel";

        pnlUrunler = new FlowLayoutPanel();
        pnlUrunler.Dock = DockStyle.Fill;
        pnlUrunler.AutoScroll = true;
        pnlUrunler.Padding = new Padding(10);
        pnlUrunler.BackColor = RenkArkaPlan;
        pnlUrunler.Tag = "Bg";

        pnlSolUst.Controls.Add(pnlUrunler);
        pnlSolUst.Controls.Add(pnlKategoriler);

        // ── Sağ Panel: Adisyon Kutusu ──
        TableLayoutPanel pnlSag = new TableLayoutPanel();
        pnlSag.Dock = DockStyle.Fill;
        pnlSag.BackColor = RenkKart;
        pnlSag.Tag = "Kart";
        pnlSag.ColumnCount = 1;
        pnlSag.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlSag.Padding = new Padding(0, 0, 0, 45); // Durum çubuğu çakışmasını engellemek için alt boşluk
        pnlSag.RowCount = 3;
        pnlSag.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
        pnlSag.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlSag.RowStyles.Add(new RowStyle(SizeType.Absolute, 240F)); // 240px sabit ve güvenli alan
        tlpSiparis.Controls.Add(pnlSag, 1, 0);

        // Sağ Üst (Başlık + Geri Dön)
        Panel pnlSagUst = new Panel();
        pnlSagUst.Dock = DockStyle.Fill;
        pnlSagUst.BackColor = RenkKartAcik;
        pnlSagUst.Tag = "KartAcik";
        pnlSag.Controls.Add(pnlSagUst, 0, 0);

        lblSeciliMasa = new MaterialLabel();
        lblSeciliMasa.Text = "📋 Adisyon: Seçilmedi";
        lblSeciliMasa.AutoSize = false;
        lblSeciliMasa.Height = 40;
        lblSeciliMasa.TextAlign = ContentAlignment.MiddleCenter; // Adisyon başlığını da elegant şekilde tam ortalıyoruz
        lblSeciliMasa.Dock = DockStyle.Bottom;
        lblSeciliMasa.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
        pnlSagUst.Controls.Add(lblSeciliMasa);

        // Geri butonunu dinamik hesaplama hataları olmadan KUSURSUZ ortalamak için TLP kullanıyoruz
        TableLayoutPanel tlpGeri = new TableLayoutPanel();
        tlpGeri.Dock = DockStyle.Fill;
        tlpGeri.ColumnCount = 1;
        tlpGeri.RowCount = 1;
        tlpGeri.Padding = new Padding(0, 8, 0, 0); // Sol taraftaki "Kategoriler" panelinin 8px'lik üst boşluğuyla BİREBİR aynı hizada olması için.
        tlpGeri.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpGeri.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        
        btnGeri = new MaterialButton();
        btnGeri.Text = Localization.Get("BackToTables");
        btnGeri.Type = MaterialButton.MaterialButtonType.Outlined;
        btnGeri.AutoSize = true; 
        // AnchorStyles.Top; nesneyi yatayda tam ortaya dizer, Dikeyde (Y ekseni) ise yukarıya yaslar.
        // Yukarısı zaten 8px korumalı (Padding) olduğu için tam nokta atışı hizalanır!
        btnGeri.Anchor = AnchorStyles.Top; 
        btnGeri.Click += (s, e) => { tabControl.SelectedTab = tabMasalar; };
        
        tlpGeri.Controls.Add(btnGeri, 0, 0);
        pnlSagUst.Controls.Add(tlpGeri);

        // Sağ Orta (Adisyon Listesi)
        listAdisyon = new MaterialListView();
        listAdisyon.Dock = DockStyle.Fill;
        listAdisyon.View = View.Details;
        listAdisyon.FullRowSelect = true;
        listAdisyon.Columns.Add(Localization.Get("ColProduct"), 185);
        listAdisyon.Columns.Add(Localization.Get("ColQty"), 60);
        listAdisyon.Columns.Add(Localization.Get("ColPrice"), 110);
        
        // Sağ-Tık (Bağlam menüsü) ve Çift-Tık entegrasyonu (Ürün Silme / Çıkarma)
        listAdisyon.DoubleClick += ListAdisyon_DoubleClick;

        ctxAdisyon = new ContextMenuStrip();
        menuCikar = new ToolStripMenuItem(Localization.Get("MenuRemoveOne"));
        menuCikar.Click += (s, ev) => ListAdisyon_DoubleClick(null, null);
        menuTümünüSil = new ToolStripMenuItem(Localization.Get("MenuRemoveAll"));
        menuTümünüSil.Click += (s, ev) => Adisyon_TamamenKaldir();
        
        ctxAdisyon.Items.Add(menuCikar);
        ctxAdisyon.Items.Add("-");
        ctxAdisyon.Items.Add(menuTümünüSil);
        listAdisyon.ContextMenuStrip = ctxAdisyon;

        pnlSag.Controls.Add(listAdisyon, 0, 1);

        // Sağ Alt (Toplam + Ödeme + İptal)
        Panel pnlSagAlt = new Panel();
        pnlSagAlt.Dock = DockStyle.Fill;
        pnlSagAlt.BackColor = RenkKartAcik;
        pnlSagAlt.Tag = "KartAcik";
        pnlSag.Controls.Add(pnlSagAlt, 0, 2);

        pnlSagAlt.Padding = new Padding(12);
        
        TableLayoutPanel tlpAction = new TableLayoutPanel();
        tlpAction.Dock = DockStyle.Fill;
        tlpAction.ColumnCount = 1;
        tlpAction.RowCount = 3;
        tlpAction.RowStyles.Add(new RowStyle(SizeType.Percent, 34F)); 
        tlpAction.RowStyles.Add(new RowStyle(SizeType.Percent, 33F)); 
        tlpAction.RowStyles.Add(new RowStyle(SizeType.Percent, 33F)); 
        pnlSagAlt.Controls.Add(tlpAction);

        // Toplam tutarı vurgulu kutu
        Panel pnlToplamKutu = new Panel();
        pnlToplamKutu.Dock = DockStyle.Fill;
        pnlToplamKutu.BackColor = RenkUstPanel;
        pnlToplamKutu.Tag = "UstPanel";
        tlpAction.Controls.Add(pnlToplamKutu, 0, 0);

        lblToplamTutar = new MaterialLabel();
        lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), "₺0,00");
        lblToplamTutar.TextAlign = ContentAlignment.MiddleCenter;
        lblToplamTutar.Dock = DockStyle.Fill;
        lblToplamTutar.FontType = MaterialSkin.MaterialSkinManager.fontType.H5; // H3 çok devasa kaçtı, H5 daha ideal
        pnlToplamKutu.Controls.Add(lblToplamTutar);

        btnOde = new MaterialButton();
        btnOde.Text = Localization.Get("PayButton");
        btnOde.Dock = DockStyle.Fill;
        btnOde.Type = MaterialButton.MaterialButtonType.Contained; // Pembe (Accent) yerine daha kurumsal durması için Contained
        btnOde.Margin = new Padding(0, 10, 0, 0);
        btnOde.Click += BtnOde_Click;
        tlpAction.Controls.Add(btnOde, 0, 1);

        btnIptal = new MaterialButton();
        btnIptal.Text = Localization.Get("CancelTable");
        btnIptal.Type = MaterialButton.MaterialButtonType.Outlined;
        btnIptal.Dock = DockStyle.Fill;
        btnIptal.Margin = new Padding(0, 8, 0, 0);
        btnIptal.Click += BtnIptal_Click;
        tlpAction.Controls.Add(btnIptal, 0, 2);

        UrunleriDinamikYukle(pnlKategoriler);
    }

    // ──────────────────────────────────────────────
    //  KATEGORİ & ÜRÜN YÜKLEME
    // ──────────────────────────────────────────────

    private string aktifKategori = "";
    private void UrunleriDinamikYukle(FlowLayoutPanel pnlKategoriler)
    {
        pnlKategoriler.Controls.Clear();
        tumUrunler = DatabaseManager.GetProducts();
        var kategoriIsimleri = tumUrunler.Select(u => u.Category).Distinct().ToList();

        bool ilk = true;
        foreach (var kategori in kategoriIsimleri)
        {
            string emoji = KategoriEmojileri.ContainsKey(kategori) ? KategoriEmojileri[kategori] + " " : "";
            
            MaterialButton btnKategori = new MaterialButton();
            btnKategori.Text = $"{emoji}{Localization.Get(kategori)}";
            btnKategori.AutoSize = true;
            btnKategori.Margin = new Padding(4);
            
            bool isAktif = string.IsNullOrEmpty(aktifKategori) ? ilk : (kategori == aktifKategori);
            btnKategori.Type = isAktif ? MaterialButton.MaterialButtonType.Contained : MaterialButton.MaterialButtonType.Outlined;
            
            if (isAktif && string.IsNullOrEmpty(aktifKategori)) aktifKategori = kategori;
            ilk = false;

            btnKategori.Click += (s, ev) => {
                foreach (Control c in pnlKategoriler.Controls)
                {
                    if (c is MaterialButton mb)
                        mb.Type = MaterialButton.MaterialButtonType.Outlined;
                }
                ((MaterialButton)s).Type = MaterialButton.MaterialButtonType.Contained;
                aktifKategori = kategori;
                KategoriUrunleriGoster(kategori);
            };

            pnlKategoriler.Controls.Add(btnKategori);
        }

        if (!string.IsNullOrEmpty(aktifKategori)) 
        {
            KategoriUrunleriGoster(aktifKategori);
        }
    }

    private void KategoriUrunleriGoster(string kategori)
    {
        pnlUrunler.SuspendLayout();
        pnlUrunler.Controls.Clear();
        
        string emoji = KategoriEmojileri.ContainsKey(kategori) ? KategoriEmojileri[kategori] : "📦";
        
        foreach (var u in tumUrunler.Where(x => x.Category == kategori))
        {
            string ozelEmoji = GetProductEmoji(u.Name, emoji);
            Panel urunKart = UrunKartiOlustur(u, ozelEmoji);
            pnlUrunler.Controls.Add(urunKart);
        }
        pnlUrunler.ResumeLayout();
    }

    /// <summary>
    /// GDI+ ile özel çizilmiş bir ürün kartı paneli oluşturur.
    /// Emoji, ürün adı ve vurgulu fiyat gösterir.
    /// </summary>
    private Panel UrunKartiOlustur(Product u, string emoji)
    {
        Panel urunKart = new Panel();
        urunKart.Size = new Size(155, 110);
        urunKart.Margin = new Padding(8);
        urunKart.Cursor = Cursors.Hand;
        urunKart.Tag = u;
        urunKart.BackColor = Color.Transparent;

        urunKart.Paint += (s, ev) =>
        {
            Graphics g = ev.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, urunKart.Width - 1, urunKart.Height - 1);

            using (GraphicsPath path = YuvarlatilmisKare(rect, 12))
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, RenkKartAcik, RenkKart, 90f))
            {
                g.FillPath(brush, path);
                using (Pen pen = new Pen(IsDarkTheme ? Color.FromArgb(70, 70, 100) : Color.LightGray, 1.5f))
                    g.DrawPath(pen, path);
            }

            // Emoji
            using (Font fEmoji = new Font("Segoe UI Emoji", 18f))
                g.DrawString(emoji, fEmoji, new SolidBrush(RenkBeyazYazi), 8, 6);

            // Ürün adı
            using (Font fAd = new Font("Segoe UI Semibold", 9.2f)) // Fontu çok az küçülterek 2 satıra daha iyi sığmasını sağladık
            {
                string displayName = Localization.Get(u.Name);
                RectangleF textRect = new RectangleF(8, 38, urunKart.Width - 16, 36); // Yükseklik 25 -> 36 (2 satır desteği)
                using (StringFormat sf = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, Alignment = StringAlignment.Near })
                    g.DrawString(displayName, fAd, new SolidBrush(RenkBeyazYazi), textRect, sf);
            }

            // Fiyat
            using (Font fFiyat = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                string fiyatYazi = u.Price.ToString("C");
                g.DrawString(fiyatYazi, fFiyat, new SolidBrush(RenkVurgu), 8, 76); // Fiyat aşağı kaydırıldı
            }
        };

        urunKart.Click += (s, ev) =>
        {
            Product urun = (Product)((Panel)s).Tag;
            UrunEkle(urun);
        };

        urunKart.MouseEnter += (s, ev) => { urunKart.BackColor = Color.FromArgb(15, 255, 255, 255); urunKart.Invalidate(); };
        urunKart.MouseLeave += (s, ev) => { urunKart.BackColor = Color.Transparent; urunKart.Invalidate(); };

        return urunKart;
    }

    // ──────────────────────────────────────────────
    //  ADİSYON İŞLEMLERİ (Ürün Ekleme, Ödeme, İptal)
    // ──────────────────────────────────────────────

    private void UrunEkle(Product u)
    {
        if (string.IsNullOrEmpty(aktifMasa)) return;

        bool found = false;
        string searchName = Localization.Get(u.Name);
        foreach (ListViewItem existingItem in listAdisyon.Items)
        {
            if (existingItem.Text == searchName)
            {
                int currQty = int.Parse(existingItem.SubItems[1].Text);
                existingItem.SubItems[1].Text = (currQty + 1).ToString();
                
                decimal currentPriceTotal = 0;
                string priceString = existingItem.SubItems[2].Text.Replace("₺", "").Replace("TL", "").Trim();
                decimal.TryParse(priceString, out currentPriceTotal);
                
                existingItem.SubItems[2].Text = (currentPriceTotal + u.Price).ToString("C");
                found = true;
                break;
            }
        }

        if (!found)
        {
            ListViewItem item = new ListViewItem(Localization.Get(u.Name));
            item.SubItems.Add("1");
            item.SubItems.Add(u.Price.ToString("C"));
            listAdisyon.Items.Add(item);
        }

        toplamHesap += u.Price;
        lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), "₺" + toplamHesap.ToString("N2"));
    }

    private void ListAdisyon_DoubleClick(object sender, EventArgs e)
    {
        if (listAdisyon.SelectedItems.Count == 0) return;

        ListViewItem item = listAdisyon.SelectedItems[0];
        int mevcutAdet = int.Parse(item.SubItems[1].Text);
        
        string tfString = item.SubItems[2].Text.Replace("₺", "").Replace("TL", "").Trim();
        if (!decimal.TryParse(tfString, out decimal toplamFiyatUrun)) return;

        decimal birimFiyat = toplamFiyatUrun / mevcutAdet;

        // Hesaplamadan düşme
        toplamHesap -= birimFiyat;
        if (toplamHesap < 0) toplamHesap = 0; // Önlem

        if (mevcutAdet > 1)
        {
            mevcutAdet--;
            item.SubItems[1].Text = mevcutAdet.ToString();
            item.SubItems[2].Text = (birimFiyat * mevcutAdet).ToString("C");
        }
        else
        {
            listAdisyon.Items.Remove(item);
        }

        lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), toplamHesap.ToString("C"));
    }

    private void Adisyon_TamamenKaldir()
    {
        if (listAdisyon.SelectedItems.Count == 0) return;

        ListViewItem item = listAdisyon.SelectedItems[0];
        string tfString = item.SubItems[2].Text.Replace("₺", "").Replace("TL", "").Trim();
        
        if (decimal.TryParse(tfString, out decimal toplamFiyatUrun))
        {
            toplamHesap -= toplamFiyatUrun;
            if (toplamHesap < 0) toplamHesap = 0; // Önlem
            lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), toplamHesap.ToString("C"));
        }
        
        listAdisyon.Items.Remove(item);
    }

    private void BtnIptal_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(aktifMasa)) return;

        if (MessageBox.Show(string.Format(Localization.Get("CancelConfirm"), aktifMasa),
            Localization.Current == Language.TR ? "İptal Onayı" : "Cancel Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            listAdisyon.Items.Clear();
            toplamHesap = 0;
            lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), "0.00 TL");

            MasaDurumGuncelle(aktifMasa, true);
            aktifMasa = "";
            tabControl.SelectedTab = tabMasalar;
        }
    }

    private void BtnOde_Click(object sender, EventArgs e)
    {
        if (toplamHesap == 0) return;

        DatabaseManager.SaveOrder(aktifMasa, toplamHesap);

        MessageBox.Show(string.Format(Localization.Get("PaymentSuccess"), aktifMasa, "₺" + toplamHesap.ToString("N2")),
            Localization.Current == Language.TR ? "✅ Ödeme Başarılı" : "✅ Payment Successful");
        
        listAdisyon.Items.Clear();
        toplamHesap = 0;
        lblToplamTutar.Text = string.Format(Localization.Get("TotalPrice"), "0.00 TL");

        MasaDurumGuncelle(aktifMasa, true);
        aktifMasa = "";
        
        RaporVerileriniYenile();
        tabControl.SelectedTab = tabMasalar;
    }
}
