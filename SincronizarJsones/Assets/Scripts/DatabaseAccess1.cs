using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

public class DatabaseAccess1 : MonoBehaviour
{
    private const string MONGO_URI = "mongodb+srv://AjedrezM:Jpablo211@cluster0-grtsl.mongodb.net/Prueba?retryWrites=true&w=majority";
    private const string DATABASE_NAME = "Prueba";
    private const string COLLECTION_NAME = "tbl_estadisticas";

    private MongoClient client;
    private IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;
    void Start()
    {
        client = new MongoClient(new MongoUrl( MONGO_URI));
        database = client.GetDatabase(DATABASE_NAME);
        collection = database.GetCollection<BsonDocument>(COLLECTION_NAME);
        // Con esta linea agregas nuevos datos al Json
        SaveScoreToDataBase("NuevoRegistroDePrueba", 130);

    }

    public async void SaveScoreToDataBase(string userName, int score)
    {
        var document = new BsonDocument { { userName, score } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore1>> GetScoresFromDataBase()
    {
        var allScoreTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoreTask;

        List<HighScore1> highscores = new List<HighScore1>();
        foreach (var score in scoresAwaited.ToList())
        {
            highscores.Add(Deserialize(score.ToString()));
        }

        return highscores;
    }

    private HighScore1 Deserialize(string rawJson)
    {
        // formato para rawJson ---> "{ \"_id\" : ObjectId(\"5fb69d839b96e00338aa53f1\"), \"username\" : 123 }"
        var highScore = new HighScore1();

        // Remover ID: no nos interesa que se vea el ID
        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4);
        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}") - stringWithoutID.IndexOf(":") - 3);

        highScore.UserName = username;
        highScore.Score = Convert.ToInt32(score);

        return highScore;

    }
}

public class HighScore1
{
    public string UserName { get; set; }
    public int Score { get; set; }
}
