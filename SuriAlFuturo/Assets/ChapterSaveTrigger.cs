using UnityEngine;
using System.Collections;

public class ChapterSaveTrigger : MonoBehaviour 
{
    public bool DetectInTrigger = false;
    public int ChapterToChange = 1;

    void OnTriggerEnter(Collider coll)
    {
        if(false == DetectInTrigger){
            
        }

        if(coll.tag.CompareTo("Player") != 0)
            return;

        PersistenceController ctrl = GameObject.FindGameObjectWithTag(SuriAlFuturo.Tag.GameController)
            .GetComponent<PersistenceController>();

        PersistenceController.ReachedChapter = ChapterToChange;
        PersistenceController.SaveDataToDisk();

        Destroy(this);
    }

}
