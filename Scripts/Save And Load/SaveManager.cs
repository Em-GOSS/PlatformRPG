using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : SingletonMonoBehaviour<SaveManager> 
{   
    private GameData gameData;
    private List<ISaveManager> ISaveManagers;

    //IO
    [SerializeField] private string fileName;
    private FileDataHandler fileDataHandler;

    protected override void Start()
    {
        base.Start();
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        ISaveManagers = FindAllISaveManager();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {   
        gameData = fileDataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No Save Data Found !");
            NewGame();
        }

        foreach(ISaveManager isaveManager in ISaveManagers)
        {
            isaveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        Debug.Log("Game Was Saved !");
        
        foreach(ISaveManager isaveManager in ISaveManagers)
        {
            isaveManager.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    [ContextMenu("Delete Current Game Data")] 
    private void DeleteGameSaveData() 
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        fileDataHandler.DestorySaveData();
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllISaveManager()
    {
        IEnumerable<ISaveManager> ISaveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(ISaveManagers);
    }
}
