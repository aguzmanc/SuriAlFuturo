using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TapController : MonoBehaviour {
    public List<WaterTap> Taps = new List<WaterTap>();
    public Dictionary<Vector3, PersistedWaterSource> SavedTaps = new Dictionary<Vector3, PersistedWaterSource>();

    public FadeInFadeOut GoodNewsBG;
    public FadeInFadeOut GoodNewsText;

    void Start()
    {
        GoodNewsBG.Hide();
        GoodNewsText.Hide();
    }

    public void NotifyInteractionTriggered () {
        for (int i=0; i<Taps.Count; i++) {
            Taps[i].TriggerInteraction();
        }
    }

    public void Save (WaterTap tap) {
        SavedTaps[tap.PersistenceKey] = tap.GetPersistedObject();
    }

    public void Load (WaterTap tap) {
        try {
            PersistedWaterSource saved = SavedTaps[tap.PersistenceKey];
            tap.Load(saved.IsOn,saved.IsUsable);
        } catch {}
    }

    public int CountWaterTapsOff () {
        int count = 0;

        foreach (WaterTap tap in Taps) {
            if (false == tap.IsOn) {
                count++;
            }
        }

        return count;
    }



    public void NotifyTapOff() 
    {
        StartCoroutine(ShowGoodNews());
    }



    IEnumerator ShowGoodNews()
    {
        GoodNewsBG.FadeIn();
        GoodNewsText.FadeIn();

        yield return new WaitUntil(()=>GoodNewsText.HasFinishedFade);

        yield return new WaitForSeconds(2f);

        GoodNewsBG.FadeOut();
        GoodNewsText.FadeOut();
        
    }
}
