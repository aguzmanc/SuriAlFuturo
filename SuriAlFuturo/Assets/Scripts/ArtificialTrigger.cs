using UnityEngine;
using System.Collections;

public class ArtificialTrigger : MonoBehaviour {
    public Blocker TheBlocker;

    void OnTriggerEnter (Collider c) {
        TheBlocker.TriggerEnter();
    }

    void OnTriggerExit (Collider c) {
        TheBlocker.TriggerExit();
    }
}
