using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;
    public Text scoreText;
    public Text levelText;
    public Text layersText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUi(int score, int level, int layers)
    {
        scoreText.text = $"Score: {score}";
        layersText.text = $"Layers: {layers}";
        levelText.text = $"Level: {level}";
    }
}
