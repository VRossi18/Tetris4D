using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioSource fastSound;
    public AudioSource gameOverSound;

    int score;
    int level;
    int layers;

    bool isGameOver;

    public GameObject gameOverWindow;

    float fallSpeed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetScore(score);
        gameOverWindow.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            fastSound.Play();
    }

    public void SetScore(int amount)
    {
        score += amount;
        CalculateLevel();
        UIHandler.instance.UpdateUi(score, level, layers);
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    public void LayersCleared(int amount)
    {
        layers += amount;
        if (amount == 1)
            SetScore(400);
        else if (amount == 2)
            SetScore(800);
        else if (amount == 3)
            SetScore(1200);
        else if (amount == 4)
            SetScore(1600);
        CalculateLevel();
        UIHandler.instance.UpdateUi(score, level, layers);
    }

    void CalculateLevel()
    {
        if (score <= 1000)
        {
            level = 1;
            fallSpeed = 3f;
        }
        else if (score > 1000 && score < 2000)
        {
            level = 2;
            fallSpeed = 2.5f;
        }
        else if (score > 2000 && score < 3000)
        {
            level = 3;
            fallSpeed = 2f;
        }
        else if (score > 3000 && score < 4000)
        {
            level = 4;
            fallSpeed = 1.5f;
        }
        else
        {
            level = 5;
            fallSpeed = 1f;
        }
    }

    public bool ReadIsGameOver()
    {
        return isGameOver;
    }

    public void SetIsGameOver()
    {
        isGameOver = true;
        gameOverWindow.SetActive(true);
        gameOverSound.Play();
    }
}
