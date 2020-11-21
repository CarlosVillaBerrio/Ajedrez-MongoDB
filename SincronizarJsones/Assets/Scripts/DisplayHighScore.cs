﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHighScore : MonoBehaviour
{
    private DatabaseAccess databaseAccess;

    public TextMeshProUGUI highScoreOutPut;
    void Start()
    {
        databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
        //highScoreOutPut.GetComponentInChildren<TextMeshPro>();
        Invoke("DisplayHighScoreInTextMesh", 2f);
    }

    void Update()
    {
        
    }

    private async void DisplayHighScoreInTextMesh()
    {
        var task = databaseAccess.GetScoresFromDataBase();
        var result = await task;
        var output = "";
        foreach (var score in result)
        {
            output += score.UserName + " Score: " + score.Score.ToString() + "\n";
        }
        highScoreOutPut.text = output;
    }
}
