#nullable disable
using MaterialSkin.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace ModernCafePOS;

/// <summary>
/// Form1 partial: Yönetici Özeti (Rapor) sekmesi — ciro, kasa toplamı, sipariş geçmişi.
/// </summary>
public partial class Form1
{
    private void BuildRaporUI()
    {
        TableLayoutPanel tlpWrapper = new TableLayoutPanel();
        tlpWrapper.Dock = DockStyle.Fill;
        tlpWrapper.BackColor = RenkArkaPlan;
        tlpWrapper.Tag = "Bg";
        tlpWrapper.ColumnCount = 1;
        tlpWrapper.RowCount = 1;
        tlpWrapper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpWrapper.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tabRaporlar.Controls.Add(tlpWrapper);

        TableLayoutPanel tlpRapor = new TableLayoutPanel();
        tlpRapor.Size = new Size(700, 520);
        tlpRapor.BackColor = RenkKart;
        tlpRapor.Tag = "Kart";
        tlpRapor.Anchor = AnchorStyles.None;
        tlpRapor.ColumnCount = 1;
        tlpRapor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRapor.RowCount = 6;
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));   // Kapat Butonu
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));   // Başlık
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));   // Günlük Ciro
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));   // Genel Kasa
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));   // Sipariş Adet
        tlpRapor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));   // Sipariş Geçmişi
        tlpRapor.Padding = new Padding(10);
        tlpWrapper.Controls.Add(tlpRapor, 0, 0);

        btnKapatRapor = new MaterialButton();
        btnKapatRapor.Text = Localization.Get("CloseScreen");
        btnKapatRapor.Dock = DockStyle.Fill;
        btnKapatRapor.Type = MaterialButton.MaterialButtonType.Contained;
        btnKapatRapor.UseAccentColor = true;
        btnKapatRapor.Click += (s, e) => { tabControl.SelectedTab = tabMasalar; };
        tlpRapor.Controls.Add(btnKapatRapor, 0, 0);

        // Başlık
        lblBaslikRapor = new MaterialLabel();
        lblBaslikRapor.Text = Localization.Get("ReportTitle");
        lblBaslikRapor.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
        lblBaslikRapor.Dock = DockStyle.Fill;
        lblBaslikRapor.TextAlign = ContentAlignment.MiddleCenter;
        tlpRapor.Controls.Add(lblBaslikRapor, 0, 1);

        // Günlük Ciro — büyük ve vurgulu
        Panel pnlCiroKutu = new Panel();
        pnlCiroKutu.Dock = DockStyle.Fill;
        pnlCiroKutu.BackColor = RenkUstPanel;
        pnlCiroKutu.Tag = "UstPanel";
        pnlCiroKutu.Padding = new Padding(10);
        tlpRapor.Controls.Add(pnlCiroKutu, 0, 2);

        lblCiro = new MaterialLabel();
        lblCiro.Text = string.Format(Localization.Get("DailyRevenue"), "₺" + DatabaseManager.GetDailyTotal().ToString("N2"));
        lblCiro.FontType = MaterialSkin.MaterialSkinManager.fontType.H3;
        lblCiro.Dock = DockStyle.Fill;
        lblCiro.TextAlign = ContentAlignment.MiddleCenter;
        pnlCiroKutu.Controls.Add(lblCiro);

        // Genel Kasa - TableLayoutPanel kullanarak kesin dikey ortalama sağlıyoruz
        TableLayoutPanel pnlKasaGrup = new TableLayoutPanel();
        pnlKasaGrup.AutoSize = true;
        pnlKasaGrup.Anchor = AnchorStyles.None; 
        pnlKasaGrup.ColumnCount = 2;
        pnlKasaGrup.RowCount = 1;
        pnlKasaGrup.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlKasaGrup.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlKasaGrup.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRapor.Controls.Add(pnlKasaGrup, 0, 3);

        lblKasaToplam = new MaterialLabel();
        decimal totalKasa = DatabaseManager.GetTotalRevenue() + DatabaseManager.GetInitialCash();
        lblKasaToplam.Text = string.Format(Localization.Get("GeneralCash"), "₺" + totalKasa.ToString("N2"));
        lblKasaToplam.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
        lblKasaToplam.AutoSize = true;
        lblKasaToplam.TextAlign = ContentAlignment.MiddleLeft;
        lblKasaToplam.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
        pnlKasaGrup.Controls.Add(lblKasaToplam, 0, 0);

        btnKasaDuzenle = new MaterialButton();
        btnKasaDuzenle.Text = Localization.Get("Edit");
        btnKasaDuzenle.Type = MaterialButton.MaterialButtonType.Text;
        btnKasaDuzenle.CharacterCasing = MaterialButton.CharacterCasingEnum.Title;
        btnKasaDuzenle.Click += KasaDuzelt_Click;
        btnKasaDuzenle.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
        btnKasaDuzenle.Height = 40; // Label ile benzer boyuta zorla
        pnlKasaGrup.Controls.Add(btnKasaDuzenle, 1, 0);

        // Sipariş adet
        lblSiparisAdet = new MaterialLabel();
        lblSiparisAdet.Text = string.Format(Localization.Get("OrderCount"), DatabaseManager.GetDailyOrderCount());
        lblSiparisAdet.FontType = MaterialSkin.MaterialSkinManager.fontType.Body1;
        lblSiparisAdet.Dock = DockStyle.Fill;
        lblSiparisAdet.TextAlign = ContentAlignment.MiddleCenter;
        tlpRapor.Controls.Add(lblSiparisAdet, 0, 4);

        // Son siparişler listesi
        Panel pnlGecmis = new Panel();
        pnlGecmis.Dock = DockStyle.Fill;
        pnlGecmis.Padding = new Padding(0, 5, 0, 0);
        tlpRapor.Controls.Add(pnlGecmis, 0, 5);

        lblGecmisBaslik = new Label();
        lblGecmisBaslik.Text = Localization.Get("RecentOrders");
        lblGecmisBaslik.ForeColor = RenkSolukYazi;
        lblGecmisBaslik.Font = new Font("Segoe UI Semibold", 10f);
        lblGecmisBaslik.Dock = DockStyle.Top;
        lblGecmisBaslik.Height = 25;
        pnlGecmis.Controls.Add(lblGecmisBaslik);

        listSiparisGecmisi = new MaterialListView();
        listSiparisGecmisi.Dock = DockStyle.Fill;
        listSiparisGecmisi.View = View.Details;
        listSiparisGecmisi.FullRowSelect = true;
        listSiparisGecmisi.Columns.Add("Masa", 100);
        listSiparisGecmisi.Columns.Add("Tutar", 120);
        listSiparisGecmisi.Columns.Add("Tarih/Saat", 200);
        pnlGecmis.Controls.Add(listSiparisGecmisi);
        listSiparisGecmisi.BringToFront();
    }

    /// <summary>
    /// Rapor ekranındaki tüm verileri (ciro, kasa toplamı, sipariş geçmişi) günceller.
    /// Ödeme sonrası ve rapor sekmesine geçişte çağrılır.
    /// </summary>
    private void RaporVerileriniYenile()
    {
        lblCiro.Text = string.Format(Localization.Get("DailyRevenue"), "₺" + DatabaseManager.GetDailyTotal().ToString("N2"));
        if (lblKasaToplam != null)
        {
            decimal currentRev = DatabaseManager.GetTotalRevenue();
            decimal initial = DatabaseManager.GetInitialCash();
            lblKasaToplam.Text = string.Format(Localization.Get("GeneralCash"), "₺" + (currentRev + initial).ToString("N2"));
        }
        
        if (listSiparisGecmisi != null)
        {
            listSiparisGecmisi.Items.Clear();
            var gecmis = DatabaseManager.GetRecentOrders();
            foreach (var s in gecmis)
            {
                ListViewItem item = new ListViewItem(string.Format(Localization.Get("TableLabel"), s.TableId));
                item.SubItems.Add("₺" + s.TotalAmount.ToString("N2"));
                item.SubItems.Add(s.OrderDate);
                listSiparisGecmisi.Items.Add(item);
            }
            if (lblSiparisAdet != null)
                lblSiparisAdet.Text = string.Format(Localization.Get("OrderCount"), DatabaseManager.GetDailyOrderCount());
        }
    }

    private void KasaDuzelt_Click(object sender, EventArgs e)
    {
        // Basit bir dialog/prompt simülasyonu
        Form prompt = new Form()
        {
            Width = 380, // Sığması için biraz genişlettik
            Height = 200,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = Localization.Get("CashEditTitle"),
            StartPosition = FormStartPosition.CenterParent,
            BackColor = RenkKart,
            ForeColor = RenkBeyazYazi
        };

        Label textLabel = new Label() { Left = 25, Top = 25, Text = Localization.Get("CashEditLabel"), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };
        TextBox textBox = new TextBox() { Left = 25, Top = 55, Width = 310, Font = new Font("Segoe UI", 12f) };
        
        Button confirmation = new Button() 
        { 
            Text = Localization.Get("CashEditBtn"), 
            Left = 145, 
            Width = 190, 
            Top = 100, 
            Height = 40, 
            DialogResult = DialogResult.OK, 
            BackColor = RenkVurgu, 
            ForeColor = Color.White, 
            FlatStyle = FlatStyle.Flat 
        };
        confirmation.Click += (sender, e) => { prompt.Close(); };
        prompt.Controls.Add(textLabel);
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.AcceptButton = confirmation;
        
        // Formu güzelleştirelim
        prompt.Paint += (s, ev) => {
            ev.Graphics.DrawRectangle(new Pen(RenkVurgu, 2), 0, 0, prompt.Width - 1, prompt.Height - 1);
        };

        if (prompt.ShowDialog() == DialogResult.OK)
        {
            if (decimal.TryParse(textBox.Text, out decimal userEnteredAmount))
            {
                decimal currentOrderRevenue = DatabaseManager.GetTotalRevenue();
                // Kasa Devir = Kullanıcının girdiği (Kasadaki para) - Siparişlerden gelen ciro
                decimal newInitial = userEnteredAmount - currentOrderRevenue;
                
                DatabaseManager.UpdateInitialCash(newInitial);
                RaporVerileriniYenile();
                MessageBox.Show(Localization.Get("CashEditSuccess"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
