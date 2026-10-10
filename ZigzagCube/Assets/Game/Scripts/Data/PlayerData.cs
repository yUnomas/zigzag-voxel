using UnityEngine;

public class PlayerData
{
    public string name;
    public string id;
    public SkinType skin;

    /// <summary>
    /// ランダムなプレイヤー名の生成    </summary>
    private string GenerateRandomName()
    {
        string randomName = "";
        string[] playerList = { "PLAYER", "ENDLESS", "RUNNER", "ZIGZAG", "CUBE" };
        string[] symbolList = { "", "_", "#", "." };

        randomName += playerList[Random.Range(0, playerList.Length)];
        randomName += symbolList[Random.Range(0, symbolList.Length)];
        randomName += Random.Range(0, 10000).ToString("D4");

        Debug.Log(randomName);
        return randomName;
    }
    /// <summary>
    /// データ作成    </summary>
    public void Create()
    {
        name = GenerateRandomName();
        id = System.Guid.NewGuid().ToString();
    }
}
