using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Requirement {
    public Sprite Image;
    public int IndexOfDialogue;

    public static implicit operator Sprite (Requirement requirement) {
        return requirement.Image;
    }

}
