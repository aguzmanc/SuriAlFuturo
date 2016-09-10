using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HideUntilPlay : MonoBehaviour {
    public List<GameObject> HiddenItems;

    void Start () {
        for (int i=0; i<HiddenItems.Count; i++) {
            HiddenItems[i].SetActive(true);
        }
    }
}
