using UnityEngine;
using System.Collections;

public class ChapterSaveTrigger : MonoBehaviour 
{
    public bool DetectInTrigger = false;
    public int ChapterToChange = 1;

    void OnTriggerEnter(Collider coll)
    {
        if(false == DetectInTrigger)
            return;

        if(coll.tag.CompareTo(SuriAlFuturo.Tag.Player) != 0)
            return;

        PersistenceController.SaveDataToDisk();

        Destroy(this);
    }

}
