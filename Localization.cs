#nullable disable
using System.Collections.Generic;

namespace ModernCafePOS;

public enum Language { TR, EN }

public static class Localization
{
    public static Language Current = Language.TR;

    private static readonly Dictionary<string, string[]> Strings = new()
    {
        // Login Screen
        { "LoginTitle", new[] { "SİSTEME GİRİŞ YAPIN", "LOGIN TO SYSTEM" } },
        { "LoginAlt", new[] { "Modern Cafe POS • Yetkilendirme", "Modern Cafe POS • Authorization" } },
        { "LoginPinHint", new[] { "PIN Kodunuz", "Enter PIN" } },
        { "LoginBtn", new[] { "🔑 GİRİŞ YAP", "🔑 LOGIN" } },
        { "LoginDemo", new[] { "👤 ŞİFRESİZ GİRİŞ (DEMO)", "👤 NO PASSWORD (DEMO)" } },
        { "LoginError", new[] { "Hatalı PIN! (Yardım: Şifre 1234)", "Wrong PIN! (Hint: PIN is 1234)" } },

        // Tables Screen
        { "TableOpen", new[] { "🔓 MASA AÇ / SİPARİŞ", "🔓 OPEN TABLE / ORDER" } },
        { "TableClose", new[] { "🔒 MASA KAPAT / ÖDEME", "🔒 CLOSE TABLE / PAY" } },
        { "TableEmpty", new[] { "BOŞ", "EMPTY" } },
        { "TableFull", new[] { "DOLU", "FULL" } },
        { "TableTransfer", new[] { "Masa Taşıma", "Table Transfer" } },

        // Order Screen
        { "BackToTables", new[] { "← MASALARA DÖN", "← BACK TO TABLES" } },
        { "OrderTitle", new[] { "📋 Adisyon: {0}", "📋 Check: {0}" } },
        { "OrderNotSelected", new[] { "📋 Adisyon: Seçilmedi", "📋 Check: Not Selected" } },
        { "ColProduct", new[] { "Ürün", "Product" } },
        { "ColQty", new[] { "Adet", "Qty" } },
        { "ColPrice", new[] { "Fiyat", "Price" } },
        { "TotalPrice", new[] { "💰 Toplam: {0}", "💰 Total: {0}" } },
        { "PayButton", new[] { "💳 HESABI KAPAT VE TAHSİL ET", "💳 CLOSE CHECK AND PAY" } },
        { "CancelTable", new[] { "🗑️ MASAYI İPTAL ET / SİL", "🗑️ CANCEL TABLE / RESET" } },
        { "CancelConfirm", new[] { "{0} iptal edilecek ve girilen tüm siparişler silinecektir. Emin misiniz?", "{0} will be cancelled and all orders will be deleted. Are you sure?" } },
        { "PaymentSuccess", new[] { "{0} için {1} tahsil edildi!\nTutar kasaya eklendi.", "Success! {1} collected for {0}.\nAdded to cash register." } },
        { "MenuRemoveOne", new[] { "➖ Bir Adet Çıkar (Çift Tıklama)", "➖ Remove One (Double Click)" } },
        { "MenuRemoveAll", new[] { "🗑️ Bu Ürünü Tamamen Kaldır", "🗑️ Remove This Product Entirely" } },

        // Report Screen
        { "ReportTitle", new[] { "📊 KASA DURUMU VE GÜN SONU", "📊 CASH REGISTER && REPORTS" } },
        { "DailyRevenue", new[] { "💰 GÜNLÜK CİRO:  {0}", "💰 DAILY REVENUE:  {0}" } },
        { "GeneralCash", new[] { "🏦 GENEL KASA (Tümü):  {0}", "🏦 GENERAL TOTAL:  {0}" } },
        { "OrderCount", new[] { "📋 Bugünkü Sipariş Sayısı: {0}", "📋 Order Count Today: {0}" } },
        { "RecentOrders", new[] { "📜 Son Siparişler", "📜 Recent Orders" } },
        { "Edit", new[] { "Düzenle", "Edit" } },
        { "CloseScreen", new[] { "✕ EKRANI KAPAT / GERİ DÖN", "✕ CLOSE SCREEN / BACK" } },
        { "ColTable", new[] { "Masa", "Table" } },
        { "ColTotal", new[] { "Tutar", "Total" } },
        { "ColDate", new[] { "Tarih/Saat", "Date/Time" } },
        { "CashEditTitle", new[] { "Kasa Bakiyesi Güncelleme", "Update Cash Register Balance" } },
        { "CashEditLabel", new[] { "Kasadaki Güncel Tutar (TL):", "Current Amount in Cash (TL):" } },
        { "CashEditBtn", new[] { "KAYDET VE GÜNCELLE", "SAVE AND UPDATE" } },
        { "CashEditSuccess", new[] { "Kasa tutarı başarıyla güncellendi!", "Cash amount updated successfully!" } },

        // Status Bar
        { "CashierActive", new[] { "👤 Kasiyer Oturumu Aktif", "👤 Cashier Session Active" } },
        { "VersionInfo", new[] { "    Modern Cafe POS", "    Modern Cafe POS" } },
        { "LanguageSwitch", new[] { "🌍 Dil: {0}", "🌍 Lang: {0}" } },

        // Tabs
        { "TabTables", new[] { "Masalar", "Tables" } },
        { "TabOrders", new[] { "Siparişler", "Orders" } },
        { "TabReports", new[] { "Yönetici Özeti", "Admin Summary" } },
        { "TabManagement", new[] { "Ürün Yönetimi", "Product Management" } },

        // Misc
        { "BtnDayEnd", new[] { "📊 GÜN SONU / KASA RAPORU", "📊 END OF DAY REPORT" } },
        { "BtnLogout", new[] { "🚪 ÇIKIŞ YAP (KİLİTLE)", "🚪 LOGOUT (LOCK)" } },
        { "BtnSave", new[] { "💾 KAYDET", "💾 SAVE" } },
        { "BtnDelete", new[] { "🗑️ SİL / KALDIR", "🗑️ DELETE / REMOVE" } },
        { "NewProduct", new[] { "Yeni Ürün Ekle", "Add New Product" } },
        { "EditPrice", new[] { "Fiyat Güncelle", "Update Price" } },
        { "CategoryAdd", new[] { "Yeni Kategori", "New Category" } },
        { "PriceLabel", new[] { "Fiyat (TL):", "Price (TL):" } },
        { "NameLabel", new[] { "İsim:", "Name:" } },
        { "SelectCategory", new[] { "Kategori Seç:", "Select Category:" } },
        { "ManageAll", new[] { "Tüm Ürünleri Yönet", "Manage All Products" } },
        { "SuccessSave", new[] { "Başarıyla kaydedildi!", "Saved successfully!" } },
        { "SalonView", new[] { "SALON GÖRÜNÜMÜ  •  {0} Masa", "FLOOR VIEW  •  {0} Tables" } },
        { "TableLabel", new[] { "MASA {0}", "TABLE {0}" } },

        // Categories
        { "Sıcak İçecekler", new[] { "Sıcak İçecekler", "Hot Beverages" } },
        { "Soğuk İçecekler", new[] { "Soğuk İçecekler", "Cold Beverages" } },
        { "Tatlılar", new[] { "Tatlılar", "Desserts" } },
        { "Ana Yemekler", new[] { "Ana Yemekler", "Main Courses" } },
        { "Aperatifler", new[] { "Aperatifler", "Snacks" } },

        // Theme Switch
        { "DarkMode", new[] { "🌙 Karanlık Mod", "🌙 Dark Mode" } },
        { "LightMode", new[] { "☀️ Aydınlık Mod", "☀️ Light Mode" } },

        // Products - Sıcak İçecekler
        { "Bardak Çay", new[] { "Bardak Çay", "Tea (Glass)" } },
        { "Fincan Çay", new[] { "Fincan Çay", "Tea (Cup)" } },
        { "Filtre Kahve", new[] { "Filtre Kahve", "Filter Coffee" } },
        { "Türk Kahvesi", new[] { "Türk Kahvesi", "Turkish Coffee" } },
        { "Latte", new[] { "Latte", "Latte" } },
        { "Cappuccino", new[] { "Cappuccino", "Cappuccino" } },
        { "Espresso", new[] { "Espresso", "Espresso" } },
        { "Americano", new[] { "Americano", "Americano" } },
        { "Mocha", new[] { "Mocha", "Mocha" } },
        { "Sıcak Çikolata", new[] { "Sıcak Çikolata", "Hot Chocolate" } },
        { "Bitki Çayı", new[] { "Bitki Çayı", "Herbal Tea" } },

        // Products - Soğuk İçecekler
        { "Limonata", new[] { "Limonata", "Lemonade" } },
        { "Çilekli Limonata", new[] { "Çilekli Limonata", "Strawberry Lemonade" } },
        { "Ice Latte", new[] { "Ice Latte", "Ice Latte" } },
        { "Ice Mocha", new[] { "Ice Mocha", "Ice Mocha" } },
        { "Kutu Kola", new[] { "Kutu Kola", "Cola (Can)" } },
        { "Fanta", new[] { "Fanta", "Fanta" } },
        { "Sprite", new[] { "Sprite", "Sprite" } },
        { "Buzlu Çay (Şeft)", new[] { "Buzlu Çay (Şeft)", "Ice Tea (Peach)" } },
        { "Buzlu Çay (Limon)", new[] { "Buzlu Çay (Limon)", "Ice Tea (Lemon)" } },
        { "Sıkma Portakal", new[] { "Sıkma Portakal", "Fresh Orange Juice" } },
        { "Su (0.5L)", new[] { "Su (0.5L)", "Water (0.5L)" } },
        { "Maden Suyu", new[] { "Maden Suyu", "Mineral Water" } },

        // Products - Tatlılar
        { "Tiramisu", new[] { "Tiramisu", "Tiramisu" } },
        { "San Sebastian", new[] { "San Sebastian", "San Sebastian" } },
        { "Limonlu Cheesecake", new[] { "Limonlu Cheesecake", "Lemon Cheesecake" } },
        { "Orman Meyveli Kek", new[] { "Orman Meyveli Kek", "Berry Cake" } },
        { "Brownie", new[] { "Brownie", "Brownie" } },
        { "Profiterol", new[] { "Profiterol", "Profiterole" } },
        { "Sufle", new[] { "Sufle", "Soufflé" } },
        { "Marlenka", new[] { "Marlenka", "Marlenka" } },

        // Products - Ana Yemekler
        { "Köri Soslu Tavuk", new[] { "Köri Soslu Tavuk", "Curry Chicken" } },
        { "Cafe de Paris Tavuk", new[] { "Cafe de Paris Tavuk", "Cafe de Paris Chicken" } },
        { "Spagetti Bolonez", new[] { "Spagetti Bolonez", "Spaghetti Bolognese" } },
        { "Penne Arrabbiata", new[] { "Penne Arrabbiata", "Penne Arrabbiata" } },
        { "Fettuccine Alfredo", new[] { "Fettuccine Alfredo", "Fettuccine Alfredo" } },
        { "Izgara Köfte", new[] { "Izgara Köfte", "Grilled Meatballs" } },
        { "Hamburger Menü", new[] { "Hamburger Menü", "Hamburger Menu" } },
        { "Cheeseburger Menü", new[] { "Cheeseburger Menü", "Cheeseburger Menu" } },

        // Products - Aperatifler
        { "Patates Kızartması", new[] { "Patates Kızartması", "French Fries" } },
        { "Soğan Halkası (8li)", new[] { "Soğan Halkası (8li)", "Onion Rings (8pcs)" } },
        { "Çıtır Tavuk Sepeti", new[] { "Çıtır Tavuk Sepeti", "Crispy Chicken" } },
        { "Sosis Tabağı", new[] { "Sosis Tabağı", "Sausage Plate" } },
        { "Kaşarlı Tost", new[] { "Kaşarlı Tost", "Cheese Toast" } },
        { "Karışık Tost", new[] { "Karışık Tost", "Mixed Toast" } },
        { "Sigara Böreği", new[] { "Sigara Böreği", "Cheese Rolls" } },
        { "Dondurma", new[] { "Dondurma", "Ice Cream" } },
        { "Çikolatalı Dondurma", new[] { "Çikolatalı Dondurma", "Chocolate Ice Cream" } },
        { "Vanilyalı Dondurma", new[] { "Vanilyalı Dondurma", "Vanilla Ice Cream" } },
        { "Çilekli Limonlu Dondurma", new[] { "Çilekli Limonlu Dondurma", "Strawberry & Lemon Ice Cream" } },
        { "Fıstıklı Dondurma", new[] { "Fıstıklı Dondurma", "Pistachio Ice Cream" } },
        { "Karışık Dondurma (3 Top)", new[] { "Karışık Dondurma (3 Top)", "Mixed Ice Cream (3 Scoops)" } }
    };

    public static string Get(string key)
    {
        if (Strings.ContainsKey(key))
            return Strings[key][(int)Current];
        return key;
    }

    public static void Toggle()
    {
        Current = Current == Language.TR ? Language.EN : Language.TR;
    }
}
