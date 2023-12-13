using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private TextMeshProUGUI _text;   
    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        _text.text = "Score " + GameManager.Instance.Score;
    }

}
