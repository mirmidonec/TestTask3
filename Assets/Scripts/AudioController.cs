using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioObject dropSound;
    public static AudioController Instance;
    public AudioObject grabObjectCombineSound;
    public AudioObject ringSound;
    	public AudioObject dialogLetterTick;

    private void Awake()
    {
        Instance = this;
    }
    public void SpawnDropSound(Vector3 audioPos, float tempVol)
    {
        AudioObject audioObject = Instantiate(dropSound, audioPos, Quaternion.identity, null);
        audioObject.GetComponent<AudioSource>().volume = tempVol;
        audioObject.PlayAudioOnThisObject();
        Destroy(audioObject.gameObject, 1f);
    }
    	public void SpawnDialogLetterTick()
	{
		AudioObject audioObject = Object.Instantiate(dialogLetterTick, base.transform.position, Quaternion.identity, null);
		audioObject.PlayAudioOnThisObject();
		Object.Destroy(audioObject.gameObject, 1f);
	}
    public void SpawnCombineSoundAtPos(Vector3 audioPos)
    {
        AudioObject audioObject = Object.Instantiate(grabObjectCombineSound, audioPos, Quaternion.identity, null);
        audioObject.PlayAudioOnThisObject();
        Object.Destroy(audioObject.gameObject, 1f);
    }
      public void SpawnRingAtPos(Vector3 audioPos)
    {
        AudioObject audioObject = Object.Instantiate(ringSound, audioPos, Quaternion.identity, null);
        audioObject.PlayAudioOnThisObject();
        Object.Destroy(audioObject.gameObject, 1f);
    }
}
