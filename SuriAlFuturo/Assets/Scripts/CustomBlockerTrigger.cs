using UnityEngine;
using System.Collections;

public class CustomBlockerTrigger : MonoBehaviour {
    public Blocker TheBlocker;
    
    void OnTriggerEnter (Collider player) {
        TheBlocker.TriggerEnter();
    }

    void OnTriggerExit (Collider player) {
        TheBlocker.TriggerExit();
    }
}
