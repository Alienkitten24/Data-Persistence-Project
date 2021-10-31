using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    
    private bool m_Started = false;
    public int m_Points;
    public string PlayerName;
    public ArrayList HighScores = new ArrayList();

    public static bool m_GameOver = false;
    public static bool m_GamePaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);    
        
        LoadData();
    }

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
        //else if (m_GameOver)
        //{
        //   if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //   }
        //}
 //       if(BrickPrefab == null)
   //     {
     //       m_GameOver = true;
       //     GameOverScreen.SetActive(true);
   //     }

        PauseGame();
    }

    void PauseGame()
    {
        if(Input.GetKeyDown("escape"))
        {
            m_GamePaused = !m_GamePaused;
        }

        if (PauseScreen != null)
        {
            if (m_GamePaused)
            {
                PauseScreen.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                PauseScreen.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        m_Started = false;
        HighScores.Add(m_Points); 
        GameOverScreen.SetActive(true);
    }

    //data persitence part
    [System.Serializable]
    class SaveDataClass
    {
        public int m_Points;
        public string PlayerName;
        public ArrayList HighScores;
    }

    public void SaveData()
    {
        SaveDataClass data = new SaveDataClass();
        data.PlayerName = PlayerName;
        data.m_Points = m_Points;
        data.HighScores = HighScores;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataClass data = JsonUtility.FromJson<SaveDataClass>(json);

            PlayerName = data.PlayerName;
            data.m_Points = m_Points;
            HighScores = data.HighScores;

        }
    }
}
