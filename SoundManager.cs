using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] effectSounds;
    [SerializeField] AudioSource[] effectPlayer;
    [SerializeField] Sound[] bgmSounds;
    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioSource voicePlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void PlayBGM(string p_name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (p_name == bgmSounds[i].name)
            {
                bgmPlayer.clip = bgmSounds[i].clip;
                bgmPlayer.Play();
                return;
            }
        }
        Debug.LogError(p_name + "에 해당되는 BGM이 없습니다");
    }
    void StopBGM()
    {
        bgmPlayer.Stop();
    }
    void PauseBGM()
    {
        bgmPlayer.Pause();
    }
    void UnPauseBGM()
    {
        bgmPlayer.UnPause();
    }

    void PlayEffectSound(string p_name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (p_name == effectSounds[i].name)
            {
                for (int j = 0; j < effectPlayer.Length; j++)
                {
                    if (!effectPlayer[j].isPlaying)
                    {
                        effectPlayer[j].clip = effectSounds[i].clip;
                        effectPlayer[j].Play();
                        return;
                    }
                }
                Debug.LogError("모든 효과음 플레이어가 사용중입니다.");
                return;
            }
        }
        Debug.LogError(p_name + "에 해당하는 효과음 사운드가 없습니다");
    }
    public void StopAllEffectSound()
    {
        for (int i = 0; i < effectPlayer.Length; i++)
        {
            effectPlayer[i].Stop();
        }
    }

    void PlayVoiceSound(string p_name)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Sounds/Voice/" + p_name);
        if (_clip != null)
        {
            voicePlayer.clip = _clip;
            voicePlayer.Play();
        }
        else
        {
            Debug.LogError(p_name + "에 해당하는 보이스 사운드가 없습니다");
        }

    }
  /// <summary>
  /// 
  /// <>
  /// <param name="p_Name"></param>
  /// <param name="p_Type"></param>0 = BGM, 1 = Effect, 2 = Voice>
    public void  PlaySound(string p_Name, int p_Type)
    {
        if (p_Type == 0) PlayBGM(p_Name);
        else if (p_Type == 1) PlayEffectSound(p_Name);
        else PlayVoiceSound(p_Name);
    }
}