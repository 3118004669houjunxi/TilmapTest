using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
//using Newtonsoft.Json;

public class SaveAndLoad
{
    static string savePath = Application.persistentDataPath + "/save";
    BinaryFormatter bf;
    public SaveAndLoad()
    {
        bf = new BinaryFormatter();
    }
    public void Save(int num)
    {
        SaveData data = PlayerUnitSetting.instance.GameExport();
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        string str = JsonUtility.ToJson(data);
        StreamWriter sw = new StreamWriter(savePath + "/save" + num.ToString() + ".save");
        sw.Write(str);        
        sw.Close();
    }
    public void Load(int num)
    {
        if (!Directory.Exists(savePath))
            return;
        if (File.Exists(savePath + "/save" + num.ToString() + ".save"))
        {
            SaveData data = new SaveData();
            StreamReader sr = new StreamReader(savePath + "/save" + num.ToString() + ".save");
            string str = sr.ReadToEnd();
            sr.Close();
            JsonUtility.FromJsonOverwrite(str, data);
            PlayerUnitSetting.instance.GameImport(data);
        }
    }
}

[System.Serializable]
public class SaveData
{
    public int sceneIndex;
    public float maxHP;
    public float maxMP;
    public float HP;
    public float MP;
    public int ATK;
    public int gameProgress;
    public float positionX;
    public float positionY;
}