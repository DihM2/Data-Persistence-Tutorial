using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string[] topPlayers = new string[3];
    public int[] topScore = new int[3];

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    public void UpdateTopPlayers(string playerName, int newScore)
    {
        for (int index = 0; index < topScore.Length; index++)
        {
            if (newScore > topScore[index])
            {
                // Save the old top player
                string oldName = topPlayers[index];
                int oldScore = topScore[index];

                // Add the new top player to the rank
                topPlayers[index] = playerName;
                topScore[index] = newScore;

                // See if the old top player is still on the top 3
                if (IsHighScore(oldScore))
                {
                    UpdateTopPlayers(oldName, oldScore);
                }

                return;
            }
        }
    }

    public bool IsHighScore(int score)
    {
        for(int index = (topScore.Length - 1); index >= 0; index--)
        {
            if(score > topScore[index])
            {
                return true;
            }
        }

        return false;
    }

    [Serializable]
    class MemoryData
    {
        public string[] top3Names;
        public int[] top3Scores;
    }

    public void SaveData()
    {
        MemoryData data = new MemoryData();

        data.top3Names = topPlayers;
        data.top3Scores = topScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    void LoadData()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            MemoryData data = JsonUtility.FromJson<MemoryData>(json);

            topPlayers = data.top3Names;
            topScore = data.top3Scores;
        }
    }

}
