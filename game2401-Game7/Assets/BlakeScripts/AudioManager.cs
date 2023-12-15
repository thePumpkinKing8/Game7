using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] AudioSource _UIClick;
    [SerializeField] AudioSource _hmm;
    [SerializeField] AudioSource _squish;
    [SerializeField] AudioSource _pickUp;
    [SerializeField] AudioSource _rummage;
    [SerializeField] AudioSource _walking;
    [SerializeField] AudioSource _caught;
    [SerializeField] AudioSource _mainMenuMusic;
    [SerializeField] AudioSource _gameMusic;
    [SerializeField] AudioSource _chaseMusic;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayUIClick()
    {
        _UIClick.Play();
    }

    public void PlayMenuMusic()
    {
        _gameMusic.Stop();
        _chaseMusic.Stop();
        _mainMenuMusic.Play();
    }

    public void PlayGameMusic()
    {
        _gameMusic.Play();
        _chaseMusic.Stop();
        _mainMenuMusic.Stop();
    }

    public void PlayChaseMusic()
    {
        if (!_chaseMusic.isPlaying)
        {
            _chaseMusic.Play();
            _gameMusic.Stop();
            _mainMenuMusic.Stop();
        }
        
    }

    public void PlayHmm()
    {
        _hmm.Play(); // Sound for when a person hears you. Might not use.
    }

    public void PlayFruit()
    {
        _squish.Play();
    }

    public void PlayPickUp()
    {
        _pickUp.Play();
    }

    public void PlayRummage()
    {
        if(_rummage.isPlaying == false)
        {
            _rummage.Play();
        }
    }

    public void StopRummage()
    {
        _rummage.Stop();    
    }

    public void PlayWalk()
    {
        if(_walking.isPlaying == false)
        {
            _walking.Play();
        }
    }

    public void PlayCaught()
    {
        _caught.Play();
    }
}
