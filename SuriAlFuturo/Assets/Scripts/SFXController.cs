using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SFXController : MonoBehaviour 
{
    private string _currentCharacter;
    private string _currentEmotion;

    public AudioSource EmbarkSound;
    public AudioSource DisembarkSound;
    public AudioSource PickItemSound;
    public AudioSource ButtonSound;
    public AudioSource ErrorSound;
    public AudioSource WaterOnSound;
    public AudioSource WaterOffSound;
    public AudioSource TimeTravelSound;
    public AudioSource TimeMachineOnSound;


    public AudioSource DialogAudioSource;

    [System.Serializable]
    public class DialogueSFX {
        public enum Character {Suri, Chapu, Anciano, Cholita, Kid, Fernando, Robot, Joaquin, James, Andres, ydroid, Natalia, Tim};
        public enum Emotion {happy,sad,cry,surprised,bark,neutral,normal,mad,laugh,scared};

        public Character character;
        public Emotion emotion;

        public List<AudioClip> Clips;
    }

    public List<DialogueSFX> DialoguesSFX;
    public Dictionary<DialogueSFX.Character, Dictionary<DialogueSFX.Emotion, DialogueSFX> > _dialoguesDict;


    void Awake() {
        // order to easy find
        _dialoguesDict = new Dictionary<DialogueSFX.Character, Dictionary<DialogueSFX.Emotion, DialogueSFX>>();

        foreach(DialogueSFX dial in DialoguesSFX) {
            if(false == _dialoguesDict.ContainsKey(dial.character)) {
                _dialoguesDict.Add(dial.character, new Dictionary<DialogueSFX.Emotion, DialogueSFX>());
            }

            if(false == _dialoguesDict[dial.character].ContainsKey(dial.emotion)){
                _dialoguesDict[dial.character].Add(dial.emotion, dial);
            } else {
                _dialoguesDict[dial.character][dial.emotion] = dial; // just in case there is the same char-emotion combination in list
            }
        }

    }


    public void PlayEmbark(){EmbarkSound.Play();}

    public void PlayDisembark(){DisembarkSound.Play();}

    public void PlayPickItem(){PickItemSound.Play();}

    public void PlayButton(){ButtonSound.Play();}

    public void PlayError(){ErrorSound.Play();}

    public void PlayWaterOn(){WaterOnSound.Play();}

    public void PlayWaterOff(){WaterOffSound.Play();}

    public void PlayTimeTravel(){TimeTravelSound.Play();}

    public void PlayTimeMachineOn(){TimeMachineOnSound.Play();}

    public void PlayDialogue(string character, string emotion) 
    {
        if(character=="RN-570"){character="Robot";} // fix to avoid problems with enum names
        if(character=="old-joaquin" || character=="old-enrique"){character="Anciano";}

        if(character == _currentCharacter && emotion == _currentEmotion) {
            return;
        }

        _currentCharacter = character;
        _currentEmotion = emotion;

        DialogueSFX.Character ch = (DialogueSFX.Character)System.Enum.Parse(typeof(DialogueSFX.Character), character);
        DialogueSFX.Emotion emo = (DialogueSFX.Emotion)System.Enum.Parse(typeof(DialogueSFX.Emotion), emotion);

        if(_dialoguesDict.ContainsKey(ch) && _dialoguesDict[ch].ContainsKey(emo)) {
            PlayAudio(_dialoguesDict[ch][emo].Clips);
        } else {
            throw new UnityException("Dialogue for character : " + character  + "  with emotion: " + emotion + " was not configured");
        }
    }


    private void PlayAudio(List<AudioClip> clips)
    {
        if(clips==null) return;
        if(clips.Count == 0) return;

        int r = Random.Range(0, clips.Count);

        if(DialogAudioSource.clip != clips[r]) {
            if(DialogAudioSource.isPlaying) {
                DialogAudioSource.Stop();
            }

            DialogAudioSource.clip = clips[r];
            DialogAudioSource.Play();
        }
    }

}
