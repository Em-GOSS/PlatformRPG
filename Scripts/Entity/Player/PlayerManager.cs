using UnityEngine;


public class PlayerManager : SingletonMonoBehaviour<PlayerManager>, ISaveManager
{
    public Player player;

    public int currency;
    protected override void Start()
    {
        base.Start();
    }
    public void FlipPlayerWithPos(Vector2 judgePos)
    {
        Vector2 playerPos = player.transform.position;
        player.FlipController(judgePos.x-playerPos.x);
    }

    public int GetCurrency()
    {
        return currency;
    } 

    void ISaveManager.LoadData(GameData gameData)
    {
        currency = gameData.currency;
    }

    void ISaveManager.SaveData(ref GameData gameData)
    {
        gameData.currency = currency;
    }

}
