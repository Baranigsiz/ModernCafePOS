#nullable disable
using MaterialSkin;
using MaterialSkin.Controls;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace ModernCafePOS;

/// <summary>
/// Form1 partial: Yönetim / Admin sekmesi - Ürün ve Kategori yönetimi.
/// </summary>
public partial class Form1
{
    // Kategori ComboBox için wrapper — DB ismi ile lokalize görüntüyü ayrı tutar
    private class CategoryItem
    {
        public string DbName { get; set; }
        public override string ToString() => Localization.Get(DbName);
    }

    private void BuildYonetimUI()
    {
        // ── Üst Navigasyon Barı (stilize, boşluklu) ──
        TableLayoutPanel pnlNav = new TableLayoutPanel();
        pnlNav.Dock = DockStyle.Top;
        pnlNav.Height = 60;
        pnlNav.BackColor = RenkUstPanel;
        pnlNav.Tag = "UstPanel";
        pnlNav.Padding = new Padding(15, 0, 15, 0);
        pnlNav.ColumnCount = 3;
        pnlNav.RowCount = 1;
        pnlNav.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlNav.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlNav.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tabYonetim.Controls.Add(pnlNav);

        btnYonetimGeri = new MaterialButton() { 
            Text = Localization.Get("BackToTables"), 
            Type = MaterialButton.MaterialButtonType.Outlined,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Margin = new Padding(0, 8, 0, 8) 
        };
        btnYonetimGeri.Click += (s, e) => tabControl.SelectedTab = tabMasalar;
        pnlNav.Controls.Add(btnYonetimGeri, 0, 0);

        lblYonetimBaslik = new Label() { 
            Text = "⚙️ " + Localization.Get("TabManagement"),
            Font = new Font("Segoe UI Semibold", 13f),
            ForeColor = RenkBeyazYazi,
            Tag = "BeyazYazi",
            AutoSize = true,
            Anchor = AnchorStyles.None,
            TextAlign = ContentAlignment.MiddleCenter
        };
        pnlNav.Controls.Add(lblYonetimBaslik, 1, 0);

        // ── Ana İçerik (Liste + Düzenleme Formu) ──
        TableLayoutPanel tlpBase = new TableLayoutPanel();
        tlpBase.Dock = DockStyle.Fill;
        tlpBase.ColumnCount = 2;
        tlpBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        tlpBase.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        tlpBase.RowCount = 1;
        tlpBase.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpBase.Padding = new Padding(15, 10, 15, 50); // Alt: 50px → durum çubuğu ile çakışmayı önler
        tabYonetim.Controls.Add(tlpBase);

        pnlNav.BringToFront(); // Nav bar her zaman üstte

        // ── Sol: Ürün Listesi ──
        Panel pnlSol = new Panel();
        pnlSol.Dock = DockStyle.Fill;
        pnlSol.Padding = new Padding(0, 0, 8, 0); // Sağ tarafla arasına boşluk
        tlpBase.Controls.Add(pnlSol, 0, 0);

        listYonetimUrunler = new MaterialListView();
        listYonetimUrunler.Dock = DockStyle.Fill;
        listYonetimUrunler.View = View.Details;
        listYonetimUrunler.FullRowSelect = true;
        listYonetimUrunler.Columns.Add(Localization.Get("ColProduct"), 250);
        listYonetimUrunler.Columns.Add(Localization.Get("ColPrice"), 120);
        listYonetimUrunler.Columns.Add(Localization.Get("SelectCategory"), 180);
        listYonetimUrunler.SelectedIndexChanged += (s, e) => {
            if (listYonetimUrunler.SelectedItems.Count > 0) {
                var item = listYonetimUrunler.SelectedItems[0];
                txtUrunAd.Text = item.Text;
                txtUrunFiyat.Text = item.SubItems[1].Text.Replace("₺", "").Trim();
                // Lokalize edilmiş kategori adıyla ComboBox'ta eşleşme bul
                int idx = cmbKategori.FindStringExact(item.SubItems[2].Text);
                if (idx >= 0) cmbKategori.SelectedIndex = idx;
            }
        };
        pnlSol.Controls.Add(listYonetimUrunler);

        // ── Sağ: Düzenleme Paneli (Scroll destekli) ──
        FlowLayoutPanel pnlSag = new FlowLayoutPanel();
        pnlSag.Dock = DockStyle.Fill;
        pnlSag.FlowDirection = FlowDirection.TopDown;
        pnlSag.Padding = new Padding(15, 20, 15, 10);
        pnlSag.WrapContents = false;
        pnlSag.AutoScroll = true; // Küçük ekranlarda scroll açılır
        tlpBase.Controls.Add(pnlSag, 1, 0);

        // Ürün Ekle/Düzenle
        lblYonetimUrunBaslik = new Label() { Text = Localization.Get("NewProduct"), Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, ForeColor = RenkVurgu, Margin = new Padding(0,5,0,10) };
        pnlSag.Controls.Add(lblYonetimUrunBaslik);

        txtUrunAd = new MaterialTextBox() { Hint = Localization.Get("NameLabel"), Width = 300 };
        pnlSag.Controls.Add(txtUrunAd);

        txtUrunFiyat = new MaterialTextBox() { Hint = Localization.Get("PriceLabel"), Width = 300, Margin = new Padding(0,10,0,0) };
        pnlSag.Controls.Add(txtUrunFiyat);

        lblYonetimKategoriSec = new Label() { Text = Localization.Get("SelectCategory"), ForeColor = RenkSolukYazi, Margin = new Padding(0,15,0,5) };
        pnlSag.Controls.Add(lblYonetimKategoriSec);

        cmbKategori = new ComboBox() { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
        pnlSag.Controls.Add(cmbKategori);

        btnUrunKaydet = new MaterialButton() { Text = Localization.Get("BtnSave"), Width = 300, Margin = new Padding(0,15,0,0), Type = MaterialButton.MaterialButtonType.Contained };
        btnUrunKaydet.Click += (s, e) => {
            if (string.IsNullOrEmpty(txtUrunAd.Text) || string.IsNullOrEmpty(txtUrunFiyat.Text) || cmbKategori.SelectedItem == null) return;
            if (decimal.TryParse(txtUrunFiyat.Text, out decimal price)) {
                DatabaseManager.SaveProduct(txtUrunAd.Text, price, ((CategoryItem)cmbKategori.SelectedItem).DbName);
                YonetimVerileriniYenile();
                UrunleriDinamikYukle(pnlKategoriler); // Ana ekrandaki menüyü tazele
                MessageBox.Show(Localization.Get("SuccessSave"));
            }
        };
        pnlSag.Controls.Add(btnUrunKaydet);

        btnUrunSil = new MaterialButton() { Text = Localization.Get("BtnDelete"), Width = 300, Margin = new Padding(0,10,0,30), Type = MaterialButton.MaterialButtonType.Outlined };
        btnUrunSil.Click += (s, e) => {
            if (listYonetimUrunler.SelectedItems.Count > 0) {
                var product = tumUrunler.FirstOrDefault(x => Localization.Get(x.Name) == listYonetimUrunler.SelectedItems[0].Text);
                if (product != null) {
                    DatabaseManager.DeleteProduct(product.Id);
                    YonetimVerileriniYenile();
                    UrunleriDinamikYukle(pnlKategoriler);
                }
            }
        };
        pnlSag.Controls.Add(btnUrunSil);

        // Kategori Ekle
        lblYonetimKatBaslik = new Label() { Text = Localization.Get("CategoryAdd"), Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, ForeColor = RenkVurgu, Margin = new Padding(0,0,0,10) };
        pnlSag.Controls.Add(lblYonetimKatBaslik);

        txtYeniKategori = new MaterialTextBox() { Hint = "Kategori Adı", Width = 300 };
        pnlSag.Controls.Add(txtYeniKategori);

        btnKategoriEkle = new MaterialButton() { Text = Localization.Get("BtnSave"), Width = 300, Margin = new Padding(0,10,0,0) };
        btnKategoriEkle.Click += (s, e) => {
            if (!string.IsNullOrEmpty(txtYeniKategori.Text)) {
                DatabaseManager.AddCategory(txtYeniKategori.Text);
                txtYeniKategori.Text = "";
                YonetimVerileriniYenile();
                UrunleriDinamikYukle(pnlKategoriler);
                MessageBox.Show(Localization.Get("SuccessSave"));
            }
        };
        pnlSag.Controls.Add(btnKategoriEkle);

        YonetimVerileriniYenile();
    }

    private void YonetimVerileriniYenile()
    {
        if (listYonetimUrunler == null) return;

        listYonetimUrunler.Items.Clear();
        tumUrunler = DatabaseManager.GetProducts();
        
        foreach (var u in tumUrunler)
        {
            ListViewItem item = new ListViewItem(Localization.Get(u.Name));
            item.SubItems.Add("₺" + u.Price.ToString("N2"));
            item.SubItems.Add(Localization.Get(u.Category));
            listYonetimUrunler.Items.Add(item);
        }

        if (cmbKategori != null)
        {
            cmbKategori.Items.Clear();
            var categories = DatabaseManager.GetCategoryNames();
            foreach (var cat in categories) cmbKategori.Items.Add(new CategoryItem { DbName = cat });
            if (cmbKategori.Items.Count > 0) cmbKategori.SelectedIndex = 0;
        }
    }
}
