using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistenceController : MonoBehaviour {
    public Dictionary<Vector3, PersistedGameObject> SavedGameObjects =
        new Dictionary<Vector3, PersistedGameObject>();

    public void Save (Persist gameObject) {
        SavedGameObjects[gameObject.PersistenceKey] =
            gameObject.GetPersistenceObject();
    }

    public void Load (Persist gameObject) {
        gameObject.Load(SavedGameObjects[gameObject.PersistenceKey]);
    }

    public bool HasSavedData (Persist gameObject) {
        return SavedGameObjects.ContainsKey(gameObject.PersistenceKey);
    }
}
