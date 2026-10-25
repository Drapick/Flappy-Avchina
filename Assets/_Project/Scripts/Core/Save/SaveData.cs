using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // Список рекордов по уровням
    public List<int> levelHighScores = new List<int>();

    // Текущий выбранный скин игрока
    public string currentSkin = "00_standart_scin";
}