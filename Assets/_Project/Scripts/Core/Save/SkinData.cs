using System.Collections.Generic;
using System.Linq;

public static class SkinData
{
    // Ваша таблица: Ключ = точное имя префаба из Resources/Skins, Значение = цена
    public static readonly Dictionary<string, int> Skins = new Dictionary<string, int>()
    {
        { "00_standart_skin",  0 },
        { "01_christmas_skin", 100 },
        { "02_shlapa_skin",    250 },
        { "03_skin",           500 },
        { "04_skin",           750 },
        { "05_skin",           1000 },
        { "06_skin",           1500 },
        { "07_skin",           2000 }
    };

    // Готовый список имен по порядку для конвейера
    public static List<string> AllSkinNames => Skins.Keys.ToList();

    // Удобный метод получения цены по имени
    public static int GetPrice(string skinName)
    {
        return Skins.TryGetValue(skinName, out int price) ? price : 0;
    }
}