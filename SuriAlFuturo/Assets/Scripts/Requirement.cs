using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Requirement {
    public Collectable.Tag Name = Collectable.Tag.NONE;
    public int IndexOfDialogue;
}
