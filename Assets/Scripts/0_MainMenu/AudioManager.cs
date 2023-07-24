using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // public variables
    //-----------------------------------------------------------------------------------------------------------------------------
    public AudioClip GameMenuMusic;
    public AudioClip GameInfoApear;
    public AudioClip GameInfoDisapear;
    public AudioClip GameMusic1;
    public AudioClip GameMenuMusic2;
    public AudioClip VictoryMusic;
    public AudioClip LightsOn;
    public AudioClip SoundHurt;
    public AudioClip SoundDie;
    public AudioClip SoundHit;

    // private variables
    // ----------------------------------------------------------------------------------------------------------------------------
    private AudioSource[] audioSourceArray;
    private float lastVolumeValue;

    // Start is called before the first frame update
    private void Awake()
    {
        if(CommonValues.bCreateAudioManagerOnce == false)
        {
            CommonValues.bCreateAudioManagerOnce = true;
            audioSourceArray = gameObject.GetComponents<AudioSource>();
            DontDestroyOnLoad(transform.gameObject);
            startGameMenuMusic();
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }

    public void startGameMenuMusic()
    {
        if(CommonValues.bMusicDeactive == false)
        {
            audioSourceArray[0].clip = GameMenuMusic;
            audioSourceArray[0].Play();
            audioSourceArray[0].loop = true;
        }
    }

    public void stopGameMenuMusic()
    {
        audioSourceArray[0].Stop();
        audioSourceArray[0].loop = false;
    }

    public void startGameMenuMusic2()
    {
        if (CommonValues.bMusicDeactive == false)
        {
            audioSourceArray[0].clip = GameMenuMusic2;
            audioSourceArray[0].Play();
            audioSourceArray[0].loop = true;
        }
    }

    public bool startGameMenuMusic2IsRunning()
    {
        if(audioSourceArray[0].isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void stopGameMenuMusic2()
    {
        audioSourceArray[0].Stop();
        audioSourceArray[0].loop = false;
    }

    public void playMaskDisapear()
    {
        audioSourceArray[1].clip = GameInfoDisapear;
        audioSourceArray[1].Play();
    }

    public void playGameInfoApear()
    {
        audioSourceArray[1].clip = GameInfoApear;
        audioSourceArray[1].Play();
    }

    public void playSpinningWheelStop()
    {
        audioSourceArray[1].clip = GameInfoDisapear;
        audioSourceArray[1].Play();
    }

    public void playGameMusic1()
    {
        if (CommonValues.bMusicDeactive == false)
        {
            audioSourceArray[0].clip = GameMusic1;
            audioSourceArray[0].volume = 0.15f;
            audioSourceArray[0].Play();
            audioSourceArray[0].loop = true;
        }
    }

    public void stopGameMusic1()
    {
        audioSourceArray[0].Stop();
        audioSourceArray[0].loop = false;
    }

    public void playGameMusic2()
    {
        if (CommonValues.bMusicDeactive == false)
        {
            audioSourceArray[0].clip = GameMusic1;
            audioSourceArray[0].volume = 0.3f;
            audioSourceArray[0].Play();
            audioSourceArray[0].loop = true;
        }
    }

    public void stopGameMusic2()
    {
        audioSourceArray[0].Stop();
        audioSourceArray[0].loop = false;
    }

    public void startVictoryMusic()
    {
        audioSourceArray[1].clip = VictoryMusic;
        audioSourceArray[1].Play();
        //VictoryMusic.loop = true;
    }

    public void stopVictoryMusic()
    {
        audioSourceArray[1].Stop();
    }

    public void playLightsOn()
    {
        audioSourceArray[3].clip = LightsOn;
        audioSourceArray[3].volume = 1f;
        audioSourceArray[3].Play();
    }

    public void playSoundHurt()
    {
        audioSourceArray[3].clip = SoundHurt;
        audioSourceArray[3].volume = 1f;
        audioSourceArray[3].Play();
    }

    public void playSoundDie()
    {
        audioSourceArray[3].clip = SoundDie;
        audioSourceArray[3].volume = 1f;
        audioSourceArray[3].Play();
    }

    public void playSoundHit()
    {
        audioSourceArray[3].clip = SoundHit;
        audioSourceArray[3].volume = 1f;
        audioSourceArray[3].Play();
    }
}
