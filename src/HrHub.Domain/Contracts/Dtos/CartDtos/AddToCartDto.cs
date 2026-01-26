namespace HrHub.Domain.Contracts.Dtos.CartDtos
{
    // Sepete Ekleme İsteği
    public class AddToCartDto
    {
        public long TrainingId { get; set; }
        public int LicenceCount { get; set; } = 1; // Varsayılan 1 lisans
    }

    // Sepetten Çıkarma İsteği
    public class RemoveFromCartDto
    {
        public long CartItemId { get; set; }
    }

    // Sepet Görüntüleme (Header)
    public class CartViewDto
    {
        public long CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PromotionCode { get; set; }
        public int TotalItemCount { get; set; }
        public List<CartItemViewDto> Items { get; set; } = new();
    }

    // Sepet Kalemleri (Detail)
    public class CartItemViewDto
    {
        public long Id { get; set; } // CartItemId
        public long TrainingId { get; set; }
        public string TrainingTitle { get; set; }
        public string TrainingImage { get; set; } // Frontend'de göstermek için
        public string CategoryName { get; set; }

        public decimal Amount { get; set; }        // Birim Fiyat (İndirimsiz)
        public decimal DiscountRate { get; set; }  // İndirim Oranı
        public decimal CurrentAmount { get; set; } // İndirimli Birim Fiyat
        public decimal TaxRate { get; set; }       // KDV Oranı

        public int LicenceCount { get; set; }      // Adet
        public decimal RowTotal { get; set; }      // Satır Toplamı (CurrentAmount * Count)
    }
}