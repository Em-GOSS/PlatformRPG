using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";

    [Header("Encrypted?")]
    private bool encryptData = false;
    
    [SerializeField] private string encryptKey = "EMPERIS_SRY";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData = true)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(gameData, true);

            if(encryptData)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("Error in the saving " + fullPath + "\n" + e);
        }
    }


    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }
                
                if(encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.Log("Error in the loading " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void DestorySaveData()
    {   
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for(int i = 0; i < data.Length; i ++)
        {
            modifiedData += (char)(data[i] ^ encryptKey[i % encryptKey.Length]);
        }

        return modifiedData;
    }
}
