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
    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject highScoreScreen;
    [SerializeField] GameObject inputNameScreen;
    [SerializeField] TMP_InputField playerName;
    [SerializeField] TMP_InputField inputPlayerName;
    
    private bool m_Started = false;
    private int m_Points;
    private string activePlayerName;
    private bool hasName = false;
    
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
        if (!m_Started && hasName)
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
        if(activePlayerName != null)//(playerName.text.Length != 0)
        {
            //string name = playerName.text;

            GameManager.Instance.UpdateTopPlayers(activePlayerName, m_Points);

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

    public void GetName()
    {
        if (inputPlayerName.text.Length != 0)
        {
            activePlayerName = inputPlayerName.text;

            inputNameScreen.SetActive(false);
            playerNameText.SetText(activePlayerName);
            hasName = true;

        }
        else
        {
            return;
        }
    }
}
