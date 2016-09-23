using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIContainer : MonoBehaviour {
    public bool Active;
    public GameObject ActiveIndicator;

    void Update () {
        ActiveIndicator.SetActive(Active);
    }
}
