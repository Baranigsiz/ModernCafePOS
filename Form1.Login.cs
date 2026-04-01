#nullable disable
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ModernCafePOS;

/// <summary>
/// Form1 partial: Login (giriş) ekranı — PIN doğrulama ve demo geçişi.
/// </summary>
public partial class Form1
{
    private void BuildLoginUI()
    {
        pnlLogin = new Panel();
        pnlLogin.Dock = DockStyle.Fill;
        pnlLogin.BackColor = RenkArkaPlan;
        pnlLogin.Tag = "Bg";
        
        // Login ekranı z-index olarak her şeyin üstünde olsun
        this.Controls.Add(pnlLogin);
        pnlLogin.BringToFront();

        // Dekoratif arka plan çizgileri
        pnlLogin.Paint += (s, ev) =>
        {
            Graphics g = ev.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen pen = new Pen(Color.FromArgb(25, 255, 255, 255), 1))
            {
                for (int i = 0; i < pnlLogin.Width; i += 40)
                    g.DrawLine(pen, i, 0, i + pnlLogin.Height, pnlLogin.Height);
            }
        };

        // ── Merkez Giriş Kartı ──
        Panel pnlCenter = new Panel();
        pnlCenter.Size = new Size(380, 420);
        pnlCenter.Location = new Point((1100 - 380) / 2, (768 - 420) / 2);
        pnlCenter.Anchor = AnchorStyles.None;
        pnlCenter.BackColor = RenkKart;
        pnlCenter.Tag = "Kart";
        pnlCenter.Padding = new Padding(3); // Çizilmiş kenarlıkların ezilmemesi için koruma kalkanı
        pnlLogin.Controls.Add(pnlCenter);

        // İçerik Paneli (Bütün elemanlar bunun içine dolacak, böylece border ile çakışmayacaklar)
        Panel pnlIcerik = new Panel();
        pnlIcerik.Dock = DockStyle.Fill;
        pnlIcerik.BackColor = RenkKart; // Şeffaflık yerine doğrudan solid renk (MaterialButton bug'ını çözer)
        pnlIcerik.Tag = "Kart";         // Temaya duyarlı olması için tag ekliyoruz
        pnlCenter.Controls.Add(pnlIcerik);

        // Kart kenar çizgisi
        pnlCenter.Paint += (s, ev) =>
        {
            Graphics g = ev.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Color borderRenk = IsDarkTheme ? Color.FromArgb(60, 60, 90) : Color.FromArgb(200, 200, 200);
            using (Pen pen = new Pen(borderRenk, 2))
            {
                g.DrawRectangle(pen, 0, 0, pnlCenter.Width - 1, pnlCenter.Height - 1);
            }
        };

        // Kahve emoji logosu
        lblLoginLogo = new Label();
        lblLoginLogo.Name = "lblLoginLogo";
        lblLoginLogo.Text = "☕";
        lblLoginLogo.Font = new Font("Segoe UI Emoji", 36f);
        lblLoginLogo.ForeColor = RenkBeyazYazi;
        lblLoginLogo.Tag = "BeyazYazi";
        lblLoginLogo.BackColor = Color.Transparent;
        lblLoginLogo.Dock = DockStyle.Top;
        lblLoginLogo.Height = 70;
        lblLoginLogo.TextAlign = ContentAlignment.MiddleCenter;
        pnlIcerik.Controls.Add(lblLoginLogo);

        // Başlık
        lblLoginTitle = new Label();
        lblLoginTitle.Name = "lblLoginTitle";
        lblLoginTitle.Text = Localization.Get("LoginTitle");
        lblLoginTitle.Dock = DockStyle.Top;
        lblLoginTitle.TextAlign = ContentAlignment.MiddleCenter;
        lblLoginTitle.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
        lblLoginTitle.ForeColor = RenkBeyazYazi;
        lblLoginTitle.Tag = "BeyazYazi";
        lblLoginTitle.BackColor = Color.Transparent;
        lblLoginTitle.Height = 50;
        pnlIcerik.Controls.Add(lblLoginTitle);

        // Alt başlık
        lblLoginAltBaslik = new Label();
        lblLoginAltBaslik.Name = "lblLoginAltBaslik";
        lblLoginAltBaslik.Text = Localization.Get("LoginAlt");
        lblLoginAltBaslik.ForeColor = RenkSolukYazi;
        lblLoginAltBaslik.Tag = "SolukYazi";
        lblLoginAltBaslik.BackColor = Color.Transparent;
        lblLoginAltBaslik.Font = new Font("Segoe UI", 9f);
        lblLoginAltBaslik.Dock = DockStyle.Top;
        lblLoginAltBaslik.Height = 25;
        lblLoginAltBaslik.TextAlign = ContentAlignment.MiddleCenter;
        pnlIcerik.Controls.Add(lblLoginAltBaslik);

        Panel pnlBoslukUst = new Panel();
        pnlBoslukUst.Dock = DockStyle.Top;
        pnlBoslukUst.Height = 15;
        pnlBoslukUst.BackColor = Color.Transparent;
        pnlIcerik.Controls.Add(pnlBoslukUst);

        // PIN giriş alanı
        txtPin = new MaterialTextBox();
        txtPin.Hint = Localization.Get("LoginPinHint");
        txtPin.Dock = DockStyle.Top;
        pnlIcerik.Controls.Add(txtPin);
        
        Panel pnlBosluk = new Panel();
        pnlBosluk.Dock = DockStyle.Top;
        pnlBosluk.Height = 15;
        pnlBosluk.BackColor = Color.Transparent;
        pnlIcerik.Controls.Add(pnlBosluk);

        // Giriş butonu
        btnLogin = new MaterialButton();
        btnLogin.Text = Localization.Get("LoginBtn");
        btnLogin.Dock = DockStyle.Top;
        btnLogin.UseAccentColor = true;
        btnLogin.Height = 50;
        btnLogin.Click += (s, e) => {
            if (txtPin.Text == "1234") {
                pnlLogin.Visible = false;
            } else {
                MessageBox.Show(Localization.Get("LoginError"), Localization.Current == Language.TR ? "⛔ Yetkisiz Giriş" : "⛔ Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };
        pnlIcerik.Controls.Add(btnLogin);

        Panel pnlBosluk2 = new Panel();
        pnlBosluk2.Dock = DockStyle.Top;
        pnlBosluk2.Height = 20;
        pnlBosluk2.BackColor = Color.Transparent;
        pnlIcerik.Controls.Add(pnlBosluk2);

        // Hoca için Demo geçişi
        btnDemo = new MaterialButton();
        btnDemo.Text = Localization.Get("LoginDemo");
        btnDemo.Type = MaterialButton.MaterialButtonType.Outlined;
        btnDemo.Dock = DockStyle.Top;
        btnDemo.Height = 45;
        btnDemo.Click += (s, e) => {
            pnlLogin.Visible = false;
        };
        pnlIcerik.Controls.Add(btnDemo);

        Panel pnlBosluk3 = new Panel();
        pnlBosluk3.Dock = DockStyle.Top;
        pnlBosluk3.Height = 15;
        pnlBosluk3.BackColor = Color.Transparent;
        pnlIcerik.Controls.Add(pnlBosluk3);

        // PIN ipucu
        lblIpucu = new Label();
        lblIpucu.Text = Localization.Current == Language.TR ? "Demo PIN: 1234" : "Demo PIN: 1234";
        lblIpucu.ForeColor = RenkSolukYazi;
        lblIpucu.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
        lblIpucu.Dock = DockStyle.Top;
        lblIpucu.Height = 20;
        lblIpucu.TextAlign = ContentAlignment.MiddleCenter;
        lblIpucu.BackColor = Color.Transparent;
        pnlIcerik.Controls.Add(lblIpucu);

        // Doğru yukarıdan aşağıya (Top-to-Bottom) sıralama algoritması:
        lblLoginLogo.BringToFront();
        lblLoginTitle.BringToFront();
        lblLoginAltBaslik.BringToFront();
        pnlBoslukUst.BringToFront();
        txtPin.BringToFront();
        pnlBosluk.BringToFront();
        btnLogin.BringToFront();
        pnlBosluk2.BringToFront();
        btnDemo.BringToFront();
        pnlBosluk3.BringToFront();
        lblIpucu.BringToFront();
    }
}
