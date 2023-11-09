using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CoinCounter : MonoBehaviour, ICoinCounter
{
    public Text coinText;
    private int totalCoins = 0;
    private bool ifLastLife;

    private void Start()
    {

    }

    //private void Awake()
    //{
    //    if (_coinCounter == null)
    //        _coinCounter = this;
    //    else if (_coinCounter != this)
    //        Destroy(gameObject);
    //}

    public void ResetCointCount()
    {
        coinText.text = "0";
        totalCoins = 0;
    }

    public int GetCurrentCoins()
    {
        return totalCoins;
    }

    public void MinusCoinsCount()
    {
        if (totalCoins == 0)
        {
            // Гравець програв, додайте код для обробки цього випадку
            ifLastLife = true;
            // Наприклад, можна викликати метод, який відповідає за завершення гри.
            // GameManager.Instance.GameOver();
        }
        else
        {
            totalCoins = 0;
            UpdateCoinText();
        }
    }


    public bool IsLastLife()
    {
        return ifLastLife;
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        ifLastLife = false;
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        coinText.text = "Coins: " + totalCoins.ToString();
    }
    
    public bool CoinsEqualOrLessZero()
    {
        if(totalCoins <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
