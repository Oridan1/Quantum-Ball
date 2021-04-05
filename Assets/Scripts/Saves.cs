using UnityEngine;
using System.Collections;
using System;// libreria para guardado  de datos 
using System.Runtime.Serialization.Formatters.Binary; //  lib para guardado de datos
using System.IO;

public class Saves : MonoBehaviour
{
    public static Saves instance;
    public static PlayerData datos;
    [SerializeField]
    string path;
    //public static int maxLevel;
    //public static float bestScore;
    //public static float currentScore;
    //public int currentLevel;

    // Use this for initialization
    void Awake()
    {
        datos = Load();
        instance = this;
        GameManger.manager.SetCurrentScore(datos.currentScore);
        GameManger.manager.LoadLevel(datos.maxLevel);
        GameManger.manager.SetVolume(datos.volumen);
    }
    
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + path);
        PlayerData data = new PlayerData(); // crea un nuevo player data 
        data = datos;// al crear uno  nuevo  lo  sobreescribe con los datos deseados        
        bf.Serialize(file, data);
        file.Close();
    }

    public PlayerData Load()
    {
        if (File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        return new PlayerData();
    }

    public void Restart()
    {
        datos = new PlayerData();
        Save();
    }
}

[Serializable]
public class PlayerData
{
    public int maxLevel = 1;
    public float bestScore = 0;
    public float currentScore = 0;
    public int volumen = 1;
}