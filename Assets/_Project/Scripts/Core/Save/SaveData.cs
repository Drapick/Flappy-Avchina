using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // Список рекордов по уровням
    public List<int> levelHighScores = new List<int>();

    // Текущий выбранный скин игрока
    public string currentSkin = "00_standart_skin";
    
    // Простой список купленных/открытых скинов
    public List<string> unlockedSkins = new List<string>() { "00_standart_skin" };
}