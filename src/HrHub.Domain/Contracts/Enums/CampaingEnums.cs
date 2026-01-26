using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Domain.Contracts.Enums
{
    public enum RuleType
    {
        IncludeCategory = 1,      // Şu kategoriler dahil
        ExcludeCategory = 2,      // Şu kategoriler hariç
        IncludePriceTier = 3,     // Sadece Tier 3 ve üzeri
        IncludeSubscriptionPlan = 4 // Sadece Gold Paket
    }
    public enum CampaignType
    {
        PercentageDiscount = 1,   // Yüzde İndirim (%20)
        FixedAmountDiscount = 2,  // Sabit Tutar İndirimi (50 TL)
        BuyXGetY = 3              // 1 Alana 1 Bedava (BOGO)
    }

    public enum CampaignScope
    {
        Global = 1,           // Tüm sistemde geçerli (Örn: KDV İndirimi)
        OptIn = 2,            // Eğitmen onayı gereken (Örn: Yaz İndirimleri)
        ProductSpecific = 3,  // Sadece belirli ürünlerde (Örn: Lansman)
        CategorySpecific = 4  // Belirli bir kategoride (Örn: Yazılım Eğitimleri %10)
    }

    public enum TargetAudience
    {
        Retail = 1,           // Tekil Satışlar
        SubscriptionPlan = 2, // Abonelik Paketleri
        Both = 3              // Hepsi
    }

    public enum ParticipationStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        OptOut = 4 // Eğitmen sonradan çıktı
    }
}
