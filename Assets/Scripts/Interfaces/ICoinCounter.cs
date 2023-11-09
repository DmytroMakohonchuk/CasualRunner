using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoinCounter 
{
    int GetCurrentCoins();

    void ResetCointCount();

    void AddCoins(int coinValue);

    void UpdateCoinText();
    void MinusCoinsCount();
    bool CoinsEqualOrLessZero();
    bool IsLastLife();
}
