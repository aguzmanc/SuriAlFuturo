using UnityEngine;
using System.Collections;

public class DisableOnStart : MonoBehaviour {
    bool disabled = false;

    void Start () {
        if (!disabled) {
            this.gameObject.SetActive(false);
            disabled = true;
        }
    }
}
