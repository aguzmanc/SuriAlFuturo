using UnityEngine;
using System.Collections;

public class ForcedTalkable : MonoBehaviour {
    Talkable TheTalkable;

    void Update () {

    }

    void OnTriggerEnter (Collider c) {
        TheTalkable.IsForcedToTalk = true;
    }
}
