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

    public Dictionary<Vector3, bool> HasBeenLoaded =
        new Dictionary<Vector3, bool>();

    public static string FileName = "SavedGame.dat";

    public bool ShipMightBeInaccessible = true;

    void Awake()
    {
        _LoadDataFromDisk();
    }

    void Start () {
        ShipMightBeInaccessible = true;
    }


    public void Save (Persist gameObject)
    {
        SavedGameObjects[gameObject.PersistenceKey] =
            gameObject.GetPersistenceObject();
    }



    public void Load (Persist gameObject)
    {
        gameObject.Load(SavedGameObjects[gameObject.PersistenceKey]);
        NotifyLoad(gameObject);
    }

    public void NotifyLoad (Persist gameObject) {
        HasBeenLoaded[gameObject.PersistenceKey] = true;
    }



    public bool HasSavedData (Persist gameObject)
    {
        return SavedGameObjects.ContainsKey(gameObject.PersistenceKey);
    }



    /* // Activate when debugging
    void OnGUI()
    {
        if(GUI.Button(new Rect(100,100, 300,100), "GUARDAR DATOS")) {
            SaveDataToDisk();
        }


        if(GUI.Button(new Rect(100,200, 300,100), "CARGAR DATOS")) {
            _LoadDataFromDisk();
        }
    }
    */



    public static void SaveDataToDisk ( )
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + FileName);

        GameObject gc = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController);
        DialogueController dialogCtrl = gc.GetComponent<DialogueController>();
        PersistenceController persistenceCtrl = gc.GetComponent<PersistenceController>();
        CollectionSystem collectionSystem = gc.GetComponent<CollectionSystem>();
        TapController tapCtrl = gc.GetComponent<TapController>();
        TimeTravelController timeTravelCtrl = gc.GetComponent<TimeTravelController>();
        EventController eventCtrl = gc.GetComponent<EventController>();

        SavedGame savedGame     = new SavedGame();
        savedGame.Talkables         = dialogCtrl.SavedTalkableStates;
        savedGame.GameObjects       = persistenceCtrl.SavedGameObjects;
        savedGame.Collectables      = collectionSystem.SavedCollectables;
        savedGame.Blockers          = collectionSystem.SavedBlockers;
        savedGame.Inventory         = collectionSystem.Inventory;
        savedGame.Taps              = tapCtrl.SavedTaps;
        savedGame.CurrentReality    = timeTravelCtrl.CurrentReality;
        savedGame.TouchTriggers     = eventCtrl.SavedTouchTriggers;
        savedGame.TriggeredEvents   = eventCtrl.TriggeredEvents;

        bf.Serialize(file, savedGame);
        file.Close();
    }



    private static void _LoadDataFromDisk()
    {
        string nombreArch = Application.persistentDataPath + "/" + FileName;

        GameObject gc = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController);
        DialogueController dialogCtrl = gc.GetComponent<DialogueController>();
        PersistenceController persistenceCtrl = gc.GetComponent<PersistenceController>();
        CollectionSystem collectionSystem = gc.GetComponent<CollectionSystem>();
        TapController tapCtrl = gc.GetComponent<TapController>();
        TimeTravelController timeTravelCtrl = gc.GetComponent<TimeTravelController>();
        EventController eventCtrl = gc.GetComponent<EventController>();

        if(File.Exists(nombreArch)){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(nombreArch, FileMode.Open)  ;

            SavedGame savedGame = (SavedGame)bf.Deserialize(file);

            file.Close();

            dialogCtrl.SavedTalkableStates      = savedGame.Talkables;
            persistenceCtrl.SavedGameObjects    = savedGame.GameObjects;
            collectionSystem.SavedCollectables  = savedGame.Collectables;
            collectionSystem.SavedBlockers      = savedGame.Blockers;
            collectionSystem.Inventory          = savedGame.Inventory;
            tapCtrl.SavedTaps                   = savedGame.Taps;
            timeTravelCtrl.CurrentReality       = savedGame.CurrentReality;
            eventCtrl.SavedTouchTriggers        = savedGame.TouchTriggers;
            eventCtrl.TriggeredEvents           = savedGame.TriggeredEvents;
        } else  {
            SaveDataToDisk();
        }

    }
}

[System.Serializable]
public class SavedGame
{
    private Dictionary<Vector3Serializable, PersistedTalkable> _talkables;
    private Dictionary<Vector3Serializable, GameObjectSerializable> _gameObjects;
    private Dictionary<Vector3Serializable, PersistedCollectable> _collectables;
    private Dictionary<Vector3Serializable, PersistedBlocker> _blockers;
    private Dictionary<Vector3Serializable, PersistedWaterSource> _taps;

    public Dictionary<Vector3Serializable, PersistedOnTouchTrigger> _touchTriggers;
    public Dictionary<Event, int> _triggeredEvents;


    public List<Collectable.Tag> Inventory;
    public string CurrentReality;


    public Dictionary<Vector3, PersistedTalkable> Talkables
    {
        get {
            Dictionary<Vector3, PersistedTalkable> ret = new Dictionary<Vector3, PersistedTalkable>();

            foreach(KeyValuePair<Vector3Serializable, PersistedTalkable> kv in _talkables) {
                ret.Add(kv.Key.V3, kv.Value);
            }

            return ret;
        }

        set {
            _talkables = new Dictionary<Vector3Serializable, PersistedTalkable>();

            foreach(KeyValuePair<Vector3, PersistedTalkable> kv in value) {
                _talkables.Add(new Vector3Serializable(kv.Key), kv.Value);
            }
        }
    }



    public Dictionary<Vector3, PersistedGameObject> GameObjects {
        get{
            Dictionary<Vector3, PersistedGameObject> ret = new Dictionary<Vector3, PersistedGameObject>();

            foreach(KeyValuePair<Vector3Serializable, GameObjectSerializable> kv in _gameObjects) {
                ret.Add(kv.Key.V3, new PersistedGameObject (
                    kv.Value.Position.V3,
                    kv.Value.Rotation.Q,
                    kv.Value.Scale.V3,
                    kv.Value.Enabled
                ));
            }

            return ret;
        }

        set {
            _gameObjects = new Dictionary<Vector3Serializable, GameObjectSerializable>();

            foreach(KeyValuePair<Vector3, PersistedGameObject> kv in value) {
                _gameObjects.Add(new Vector3Serializable(kv.Key), new GameObjectSerializable(kv.Value));
            }
        }
    }



    public Dictionary<Vector3, PersistedCollectable> Collectables
    {
        get{
            Dictionary<Vector3, PersistedCollectable> ret = new Dictionary<Vector3, PersistedCollectable>();

            foreach(KeyValuePair<Vector3Serializable, PersistedCollectable> kv in _collectables) {
                ret.Add(kv.Key.V3, kv.Value);
            }

            return ret;
        }

        set{
            _collectables = new Dictionary<Vector3Serializable, PersistedCollectable>();

            foreach(KeyValuePair<Vector3, PersistedCollectable> kv in value) {
                _collectables.Add(new Vector3Serializable(kv.Key), kv.Value);
            }
        }
    }



    public Dictionary<Vector3, PersistedBlocker> Blockers
    {
        get{
            Dictionary<Vector3, PersistedBlocker> ret = new Dictionary<Vector3, PersistedBlocker>();

            foreach(KeyValuePair<Vector3Serializable, PersistedBlocker> kv in _blockers) {
                ret.Add(kv.Key.V3, kv.Value);
            }

            return ret;
        }

        set{
            _blockers = new Dictionary<Vector3Serializable, PersistedBlocker>();

            foreach(KeyValuePair<Vector3, PersistedBlocker> kv in value) {
                _blockers.Add(new Vector3Serializable(kv.Key), kv.Value);
            }
        }
    }



    public Dictionary<Vector3, PersistedWaterSource> Taps
    {
        get {
            Dictionary<Vector3, PersistedWaterSource> ret = new Dictionary<Vector3, PersistedWaterSource>();

            foreach(KeyValuePair<Vector3Serializable, PersistedWaterSource> kv in _taps) {
                ret.Add(kv.Key.V3, kv.Value);
            }

            return ret;
        }

        set {
            _taps = new Dictionary<Vector3Serializable, PersistedWaterSource>();

            foreach(KeyValuePair<Vector3, PersistedWaterSource> kv in value) {
                _taps.Add(new Vector3Serializable(kv.Key), kv.Value);
            }
        }
    }


    public Dictionary<Vector3, PersistedOnTouchTrigger> TouchTriggers
    {
        get {
            Dictionary<Vector3, PersistedOnTouchTrigger> ret = new Dictionary<Vector3, PersistedOnTouchTrigger>();

            foreach(KeyValuePair<Vector3Serializable, PersistedOnTouchTrigger> kv in _touchTriggers) {
                ret.Add(kv.Key.V3, kv.Value);
            }

            return ret;
        }

        set {
            _touchTriggers = new Dictionary<Vector3Serializable, PersistedOnTouchTrigger>();

            foreach(KeyValuePair<Vector3, PersistedOnTouchTrigger> kv in value) {
                _touchTriggers.Add(new Vector3Serializable(kv.Key), kv.Value);
            }
        }

    }


    public Dictionary<Event, int> TriggeredEvents
    {
        get{return _triggeredEvents;}
        set{_triggeredEvents = value;}
    }

}

[Serializable]
public struct Vector3Serializable
{
    public float x;
    public float y;
    public float z;

    public Vector3Serializable(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }

    public Vector3 V3
    { get { return new Vector3(x, y, z); } }
}



[Serializable]
public struct QuaternionSerializable
{
    public float x;
    public float y;
    public float z;
    public float w;

    public QuaternionSerializable(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion Q
    { get { return new Quaternion(x, y, z, w); } }
}



[Serializable]
public struct GameObjectSerializable
{
    public Vector3Serializable Position;
    public QuaternionSerializable Rotation;
    public Vector3Serializable Scale;
    public bool Enabled;


    public GameObjectSerializable(PersistedGameObject _go)
    {
        Position = new Vector3Serializable(_go.Position);
        Rotation = new QuaternionSerializable(_go.Rotation);
        Scale = new Vector3Serializable(_go.Scale);
        Enabled = _go.Enabled;
    }


    public PersistedGameObject GameObject
    {
        get {
            PersistedGameObject go = new PersistedGameObject(
                Position.V3,
                Rotation.Q,
                Scale.V3,
                Enabled
            );

            return go;
        }
    }
}
