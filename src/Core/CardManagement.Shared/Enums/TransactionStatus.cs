namespace CardManagement.Shared.Enums;

public enum TransactionStatus
{
    Pending = 0,        // İşlem başlatıldı, kullanıcı onayı bekleniyor
    Success = 1,        // İşlem başarılı
    Failed = 2,         // İşlem başarısız
    Cancelled = 3,      // İşlem iptal edildi
    Expired = 4         // İşlem süresi doldu
} 