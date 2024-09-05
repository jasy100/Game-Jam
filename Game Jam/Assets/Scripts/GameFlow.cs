using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFlow : MonoBehaviour
{
    [Header("Index")]
    [SerializeField] private int Player1_Index = 0;
    [SerializeField] private int Player2_Index = 1;

    [Header("Settings")]
    [SerializeField] private int ScoreToWin = 2;
    [SerializeField] private TextMeshProUGUI WinningText;
    [SerializeField] private GameObject EndScreen;
    [SerializeField] private Color Player1_color;
    [SerializeField] private Color Player2_color;
    [SerializeField] private TextMeshProUGUI Player1scr;
    [SerializeField] private TextMeshProUGUI Player2scr;

    private int Player1_Score = 0;
    private int Player2_Score = 0;

    private bool GameEnd = false;
    private int WinPlayerNumber = 0;
    public void PlayerDied(int index)
    {
        if (index == Player1_Index)
        {
            Player2_Score++;
            Player2scr.text = Player2_Score.ToString();
        }
        else
        {
            Player1_Score++;
            Player1scr.text = Player1_Score.ToString();
        }

        if (Player1_Score >= ScoreToWin)
        {
            GameEnd = true;
            WinPlayerNumber = 1;
        }
        if (Player2_Score >= ScoreToWin)
        {
            GameEnd = true;
            WinPlayerNumber = 2;
        } 

        if (GameEnd)
        {
            EndScreen.SetActive(true);
            WinningText.text = "Player" + WinPlayerNumber.ToString() + "won !!!" ;

            if (Player1_Score >= ScoreToWin)
            {
                WinningText.color = Player1_color;
            }
            else
            {
                WinningText.color = Player2_color;
            }
        }
    }


}
