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

    [SerializeField] AudioSource _squish;
    [SerializeField] AudioSource _pickUp;
    [SerializeField] AudioSource _rummage;
    [SerializeField] AudioSource _walking;
    [SerializeField] AudioSource _caught;

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
