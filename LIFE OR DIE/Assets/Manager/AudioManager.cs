using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _audioManager;
    public static AudioManager Instance { get { return _audioManager; }}

    [SerializeField] public AudioSOdata _audioSOdata;

    private Dictionary<string, AudioClip> _DicAudioData;

    public const string _1999�������� = "BackGround_1999";
    public const string _�ش����Ч = "HitClip_heavy_1";
    public const string _���ܻ���Ч = "BeHittedClip_blood_1";

    private AudioSource musicSource;

    private List<AudioSource> ListClipSource;
    private const int INITIAL_POOL_SIZE = 5;


    private void Awake()
    {
        if (_audioManager != null && _audioManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _audioManager = this;
            DontDestroyOnLoad(gameObject); // �糡��������
        }
        //ר����������Ƶ��
     musicSource=gameObject.AddComponent<AudioSource>();
     musicSource.loop = true;
        musicSource.volume = 0.5f;
        ListClipSource = new List<AudioSource>();
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
           AudioSource temp =gameObject.AddComponent<AudioSource>();
            ListClipSource.Add(temp);
        }

        if (_audioSOdata == null)
        {
            Debug.LogWarning("��Ƶ��δ��ʼ��");
            this.gameObject.SetActive(false);
        }
        InitSODic();
    }



    private void Start()
    {
       
    }

   private  void InitSODic()
    {
        _DicAudioData = new Dictionary<string, AudioClip>();
        foreach (var vd in _audioSOdata.Audiodata) 
        {
            _DicAudioData.Add(vd.Name,vd.audioClip);
        }
    }

    private AudioSource FindFreeSource()
    {
        foreach(AudioSource v in ListClipSource)
        {
            if(!v.isPlaying)
            {
                return v;
            }
        }
        return null;
    }

    public void PlaySFX(string clipName)
    {
       AudioSource source = FindFreeSource();
        if(source == null ) {return; }
        if (!_DicAudioData.ContainsKey(clipName))
        {
            Debug.LogWarning($"����δ�ҵ�: {clipName}");
            return;
        }
        source.clip = _DicAudioData[clipName];
        source.Play();
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    public void PlayMusic(string MusicName)
    {
        if (!_DicAudioData.ContainsKey(MusicName))
        {
            Debug.LogWarning($"����δ�ҵ�: {MusicName}");
            return;
        }
        if (musicSource.isPlaying) {musicSource.Stop(); }
        musicSource.clip=_DicAudioData[MusicName];
        musicSource.Play();
        musicSource.loop=true;
    }
    // ������ֹͣ������Ч
    public void StopAllSFX()
    {
        foreach (AudioSource source in ListClipSource)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    // ������������������
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    // ������������Ч����
    public void SetSFXVolume(float volume)
    {
        foreach (AudioSource source in ListClipSource)
        {
            source.volume = Mathf.Clamp01(volume);
        }
    }
}

