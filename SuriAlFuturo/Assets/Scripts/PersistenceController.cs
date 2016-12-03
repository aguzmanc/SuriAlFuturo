using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class PersistentChapter{
    public int ChapterNumber;
}

public class PersistenceController : MonoBehaviour 
{
    public Dictionary<Vector3, PersistedGameObject> SavedGameObjects =
        new Dictionary<Vector3, PersistedGameObject>();


    public static string FileName = "ReachedChapter.dat";

    public static int ReachedChapter;


    void Awake()
    {   
        _LoadDataFromDisk();
    }


    public void Save (Persist gameObject) 
    {
        SavedGameObjects[gameObject.PersistenceKey] =
            gameObject.GetPersistenceObject();
    }



    public void Load (Persist gameObject) 
    {
        gameObject.Load(SavedGameObjects[gameObject.PersistenceKey]);
    }



    public bool HasSavedData (Persist gameObject) 
    {
        return SavedGameObjects.ContainsKey(gameObject.PersistenceKey);
    }



    public static void SaveDataToDisk() 
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + FileName);

        PersistentChapter chapter = new PersistentChapter();
        chapter.ChapterNumber = ReachedChapter;

        bf.Serialize(file, chapter);
        file.Close();
    }



    private static void _LoadDataFromDisk()
    {
        string nombreArch = Application.persistentDataPath + "/" + FileName;

        if(File.Exists(nombreArch)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(nombreArch, FileMode.Open);

            PersistentChapter chapter = (PersistentChapter)bf.Deserialize(file);
            file.Close();

            ReachedChapter = chapter.ChapterNumber;
        } else  {
            ReachedChapter = 0;
            SaveDataToDisk();
        }
            
    }
}
