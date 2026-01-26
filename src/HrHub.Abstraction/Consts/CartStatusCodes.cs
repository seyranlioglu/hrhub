namespace HrHub.Abstraction.Consts
{
    public static class CartStatusCodes
    {
        // Veritabanındaki "Code" sütununda yazan değerlerle BİREBİR aynı olmalı
        public const string Active = "ACTIVE";           // Sepette
        public const string PendingApproval = "PENDING"; // Onay Bekliyor
        public const string Approved = "APPROVED";       // Onaylandı
        public const string Rejected = "REJECTED";       // Reddedildi
        public const string Cancelled = "CANCELLED";     // İptal
    }
}