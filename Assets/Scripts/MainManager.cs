using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [SerializeField] Text ScoreText;
    [SerializeField] Text bestScore;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject highScoreScreen;
    [SerializeField] TMP_InputField playerName;
    
    private bool m_Started = false;
    private int m_Points;
    
    public bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        bestScore.text = $"Best Score: {GameManager.Instance.topPlayers[0]} - {GameManager.Instance.topScore[0]}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        /*else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }*/
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        
        if (GameManager.Instance.IsHighScore(m_Points))
        {
            // get player name and update the score
            highScoreScreen.SetActive(true);
        }
        else
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void SubmitHighScore()
    {
        if(playerName.text.Length != 0)
        {
            string name = playerName.text;

            GameManager.Instance.UpdateTopPlayers(name, m_Points);

            highScoreScreen.SetActive(false);
            gameOverScreen.SetActive(true);
            
            bestScore.text = $"Best Score: {GameManager.Instance.topPlayers[0]} - {GameManager.Instance.topScore[0]}";
        }
        else
        {
            return;
        }
        
    }

    public void RestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButtonClicked()
    {
        SceneManager.LoadScene(0);
    }

    string GetName()
    {
        string text = null;

        foreach(char c in Input.inputString)
        {
            if (c == '\b') // backspace
            {
                if(text.Length != 0)
                {
                    text = text.Substring(0, text.Length - 1);
                }
            }
            else if((c == '\n') || (c == '\r')) // enter/return
            {
                return text;
            }
            else
            {
                text += c;
            }
        }

        return text;
    }
}
