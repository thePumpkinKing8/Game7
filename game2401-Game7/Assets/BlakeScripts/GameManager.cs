using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] public int _maxScore = 1000;
    private int _score = 0;
    public int Score
    {
        get 
        { 
            return _score; 
        } 
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int value)
    {
        _score += value;
        if(_score >= _maxScore)
        {
            Debug.Log("level done");
            FindObjectOfType<Exit>().Open();
        }
    }

    public void NextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void Lose()
    {
        StartCoroutine(LoseGame());
    }

    IEnumerator LoseGame()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.PlayCaught();
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1f;
        _score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
    
}
