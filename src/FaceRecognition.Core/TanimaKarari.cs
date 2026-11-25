namespace FaceRecognition.Core;

/// <summary>
/// LBPH tanıyıcısının döndürdüğü mesafeyi kullanıcıya gösterilecek karara çevirir.
/// LBPH'de mesafe ne kadar küçükse yüz o kadar benzerdir (0 = birebir aynı).
/// </summary>
public readonly record struct TanimaKarari(string Etiket, double Mesafe, int GuvenYuzde, bool Taninan)
{
    public const string Bilinmiyor = "Bilinmiyor";
    public const double VarsayilanEsik = 80;

    /// <summary>
    /// Mesafe eşiğin altındaysa kişi tanınmış sayılır; değilse "Bilinmiyor".
    /// Güven yüzdesi basit bir ölçek: 100 − mesafe (0-100 arasına sınırlanır).
    /// </summary>
    public static TanimaKarari Olustur(string? tahminEtiketi, double mesafe, double esik = VarsayilanEsik)
    {
        if (esik <= 0)
            throw new ArgumentOutOfRangeException(nameof(esik), "Eşik pozitif olmalı.");
        if (double.IsNaN(mesafe) || mesafe < 0)
            mesafe = double.MaxValue;

        int guven = (int)Math.Round(Math.Clamp(100 - mesafe, 0, 100));
        bool taninan = tahminEtiketi is not null && mesafe <= esik;
        return new TanimaKarari(taninan ? tahminEtiketi! : Bilinmiyor, mesafe, guven, taninan);
    }

    public string Metin() => Taninan
        ? $"{Etiket}  (güven %{GuvenYuzde}, mesafe {Mesafe:0.0})"
        : Mesafe == double.MaxValue ? Bilinmiyor : $"{Bilinmiyor}  (mesafe {Mesafe:0.0})";
}
