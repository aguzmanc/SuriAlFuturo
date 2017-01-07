using UnityEngine;
using System.Collections;

public class CustomAccept : MonoBehaviour {
    public Collectable.Tag Item;
    public Blocker TheBlocker;
    public int DialogueIndex;
    public WaterTap ImprovesFuturePatch;

    void Start () {
        if (TheBlocker == null) {
            TheBlocker = GetComponent<Blocker>();
        }

        TheBlocker.Register(this);
    }

    public void Activate () {
        ImprovesFuturePatch.ToggleFountain(false, true);
    }

}
