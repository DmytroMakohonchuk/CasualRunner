using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[System.Serializable]
public class HighScoreReaderWriter : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    private string filePath;
    private PlayerData playerData;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        LoadPlayerData();
        UpdateHighScoreUI();
    }

    private void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            playerData = JsonConvert.DeserializeObject<PlayerData>(json);
        }
        else
        {
            playerData = new PlayerData();
            playerData.highScore = 0;
        }
    }

    private void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(playerData);
        File.WriteAllText(filePath, json);
    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > playerData.highScore)
        {
            playerData.highScore = newScore;
            SavePlayerData();
            UpdateHighScoreUI();
        }
    }

    private void UpdateHighScoreUI()
    {
        highScoreText.text = "Best Score: " + playerData.highScore.ToString();
    }
}



