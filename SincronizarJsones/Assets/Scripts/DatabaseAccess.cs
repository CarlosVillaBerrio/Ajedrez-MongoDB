using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

public class DatabaseAccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://testAjedrezM:test1234@cluster0.xx5ym.mongodb.net/HighScoreDB?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    void Start()
    {
        database = client.GetDatabase("HighScoreDB");
        collection = database.GetCollection<BsonDocument>("HighScoreCollection");
        // Con esta linea agregas nuevos datos al Json
        //SaveScoreToDataBase("Alejandro", 130);

    }

    public async void SaveScoreToDataBase(string userName, int score)
    {
        var document = new BsonDocument { { userName, score } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoreTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoreTask;

        List<HighScore> highscores = new List<HighScore>();
        foreach (var score in scoresAwaited.ToList())
        {
            highscores.Add(Deserialize(score.ToString()));
        }

        return highscores;
    }

    private HighScore Deserialize(string rawJson)
    {
        // formato para rawJson ---> "{ \"_id\" : ObjectId(\"5fb69d839b96e00338aa53f1\"), \"username\" : 123 }"
        var highScore = new HighScore();

        // Remover ID: no nos interesa que se vea el ID
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4);
        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}") - stringWithoutID.IndexOf(":") - 3);

        highScore.UserName = username;
        highScore.Score = Convert.ToInt32(score);

        return highScore;

    }
}

public class HighScore
{
    public string UserName { get; set; }
    public int Score { get; set; }
}
