using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class gameUI : MonoBehaviour
{
    public static gameUI Instance { get; private set; }
    [SerializeField] private Button startButton;
    [Header("Scripts")]
    [SerializeField] private GameManager gameManager; //instead of using inspector bc of GM instance
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject songEndUI;
    [SerializeField] private songMenu songMenu;
    [SerializeField] private playerStats playerStats;
    [Header("Text Elements")]
    [SerializeField] private TMP_Text earlyMissText;
    [SerializeField] private TMP_Text earlyText;
    [SerializeField] private TMP_Text perfectText;
    [SerializeField] private TMP_Text lateText;
    [SerializeField] private TMP_Text lateMissText;
    [SerializeField] private TMP_Text accuracyText;

    [Header("Graph Elements")]
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform earlyMissBarSpawnPoint;
    [SerializeField] private Transform earlyBarSpawnPoint;
    [SerializeField] private Transform perfectBarSpawnPoint;
    [SerializeField] private Transform lateBarSpawnPoint;
    [SerializeField] private Transform lateMissBarSpawnPoint;
    public GameObject pauseMenuCanvas;
    [Header("UI Elements")]
    [SerializeField] private Slider songProgressBar;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button gameOverBtn;
    [SerializeField] private Button currentBtn;
    [SerializeField] private Button arrowVisibilityBtn;
    [SerializeField] private Button arrowVisibilityOnBtn;
    [SerializeField] private Button levelEndLevelMenuBtn;
    [SerializeField] private Button levelEndStatsBtn;
    [SerializeField] private Button levelEndMenuBtn;
    [SerializeField] private TMP_Text currentBtnText;
    private bool waitingForKeyInput = false;
    private bool endBtnPressed;
    void Start()
    {
        // Find the GameManager instance
        gameManager = FindObjectOfType<GameManager>();
        spawnManager = GameObject.Find("GameMaster").GetComponent<SpawnManager>();
        playerStats = GameObject.Find("GameMaster").GetComponent<playerStats>();
        scoreManager = GameObject.Find("GameMaster").GetComponent<ScoreManager>();
        songMenu = GameObject.Find("GameMaster").GetComponent<songMenu>();
        songEndUI.SetActive(false);
        endBtnPressed = false;
    }

    void Update()
    {
        if (GameManager.Instance.songDuration())
        {
            songEnded();
        }
        pauseMenuKey();
        inputKeyChoice();
        if (GameManager.Instance.songStatus())
        {
            songProgress();
            accuracy();
        }
    }

    public void songEnded()
    {
        if (endBtnPressed == false)
        {
            gameOverBtn.onClick.Invoke();
        }
    }
    public void pauseMenu()
    {
        if (pauseMenuCanvas.activeSelf == false)
        {
            pauseBtn.onClick.Invoke();
            pauseBtn.enabled = false;
        }
        else
        {
            resumeBtn.onClick.Invoke();
            pauseBtn.enabled = true;
            SpawnManager.Instance.playSpawn();
            GameManager.Instance.ResumeGame();
        }
    }
    public void pauseMenuBtn() //ui pause button
    {
        GameManager.Instance.PauseGame();
        SpawnManager.Instance.stopSpawn();
        Debug.Log("pause btn pressed");
    }
    public void pauseMenuKey()//press 'P' while playing
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu();
        }
    }
    public void settingsMenuBtn()
    {
        GameManager.Instance.PauseGame();
        SpawnManager.Instance.stopSpawn();
    }
    public void resumeLevel()
    {
        SpawnManager.Instance.playSpawn();
        GameManager.Instance.ResumeGame();
    }
    public void startBtn()
    {
        GameManager.Instance.StartGame();
        SpawnManager.Instance.findObjects();
        SpawnManager.Instance.initialize();
        SpawnManager.Instance.playSpawn();
    }
    public void endBtn()
    {
        songEndUI.SetActive(true);
        scoreGraph();
        endBtnPressed = true;
        SpawnManager.Instance.stopSpawn();
        SpawnManager.Instance.destroyArrows();
        Debug.Log("end btn pressed");
        levelEndLevelMenuBtn.interactable = false;
        levelEndStatsBtn.interactable = false;
        levelEndMenuBtn.interactable = false;
        StartCoroutine(EnableButtonAfterDelay(5f)); //ensure song2 has fully finished to prevent errors
    }


    private IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        levelEndLevelMenuBtn.interactable = true;
        levelEndStatsBtn.interactable = true;
        levelEndMenuBtn.interactable = true;
    }
    public void lvlSelectPracBtn()
    {
        // GameManager.Instance.resetLevel();
        GameManager.Instance.gameEndBtn();
        SpawnManager.Instance.levelReset();
        ScoreManager.Instance.clearScores();
        songMenu.practiceMenu();
    }
    public void lvlSelectGameBtn()
    {
        // GameManager.Instance.resetLevel();
        GameManager.Instance.gameEndBtn();
        SpawnManager.Instance.levelReset();
        ScoreManager.Instance.clearScores();
        songMenu.gameMenu();
    }
    public void mainMenuBtn()
    {
        GameManager.Instance.gameEndBtn();
        SpawnManager.Instance.levelReset();
        ScoreManager.Instance.clearScores();
        songMenu.mainMenu();
    }
    public void statsBtn()
    {
        GameManager.Instance.gameEndBtn();
        SpawnManager.Instance.levelReset();
        ScoreManager.Instance.clearScores();
        songMenu.playerStatsMenu();
    }
    public void scoreGraph() // calcs and instatiates score graph
    {
        float totalPoints = ScoreManager.Instance.perfect + ScoreManager.Instance.early + ScoreManager.Instance.late + ScoreManager.Instance.earlyMiss + ScoreManager.Instance.lateMiss;
        float perfectWidth = (ScoreManager.Instance.perfect / totalPoints) * 100f;
        float earlyWidth = (ScoreManager.Instance.early / totalPoints) * 100f;
        float lateWidth = (ScoreManager.Instance.late / totalPoints) * 100f;
        float earlyMissWidth = (ScoreManager.Instance.earlyMiss / totalPoints) * 100f;
        float lateMissWidth = (ScoreManager.Instance.lateMiss / totalPoints) * 100f;

        InstantiateCube(perfectWidth, perfectBarSpawnPoint);
        InstantiateCube(earlyWidth, earlyBarSpawnPoint);
        InstantiateCube(lateWidth, lateBarSpawnPoint);
        InstantiateCube(earlyMissWidth, earlyMissBarSpawnPoint);
        InstantiateCube(lateMissWidth, lateMissBarSpawnPoint);
        earlyMissText.text = scoreManager.earlyMiss.ToString();
        earlyText.text = scoreManager.early.ToString();
        perfectText.text = scoreManager.perfect.ToString();
        lateText.text = scoreManager.late.ToString();
        lateMissText.text = scoreManager.lateMiss.ToString();

        string levelName = PlayerPrefs.GetString("songName");
        string gameMode = PlayerPrefs.GetString("gameState");
        int earlyScore = 0;
        int earlyMissScore = 0;
        int perfectScore = 0;
        int lateScore = 0;
        int lateMissScore = 0;
        int totalScore = 0;
        string timestamp = System.DateTime.Now.ToString();
        ScoreManager.Instance.getScores(levelName, gameMode, earlyScore, earlyMissScore, perfectScore, lateScore, lateMissScore, totalScore, timestamp);
    }

    private void InstantiateCube(float width, Transform spawnPoint)
    {
        int widthInt = Mathf.RoundToInt(width + 1f);
        Vector3 barOrigin = spawnPoint.position + new Vector3(width / 2f, 0f, 0f);
        GameObject bar = Instantiate(barPrefab, barOrigin, Quaternion.identity);
        bar.transform.localScale = new Vector3(widthInt, 5f, 1f);
    }
    public void setInputKey(KeyCode key)
    {
        GameManager.Instance.setInputKey(key);
    }

    public KeyCode getInputKey()
    {
        return GameManager.Instance.getInputKey();
    }
    public void SetNextInputKey(Button button)
    {
        currentBtnText.GetComponent<TMP_Text>();
        if (!waitingForKeyInput)
        {
            currentBtn = button;
            currentBtnText.text = "Press Key...";
            waitingForKeyInput = true;
        }
    }
    public void inputKeyChoice()
    {
        currentBtnText.GetComponent<TMP_Text>();
        if (currentBtn != null && Input.anyKeyDown && waitingForKeyInput)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    gameManager.setInputKey(keyCode);
                    currentBtnText.text = keyCode.ToString();
                    currentBtn = null;
                    waitingForKeyInput = false;
                    break;
                }
            }
        }
    }
    public void resetKey()
    {
        setInputKey(KeyCode.Space);
        currentBtnText.text = "SPACE";
    }

    public void songProgress()
    {
        float currentTime = (float)GameManager.Instance.currentDuration;
        float totalDuration = (float)GameManager.Instance.lastNote;
        float progress = currentTime / totalDuration;
        songProgressBar.value = progress;
    }
    public void accuracy()
    {
        accuracyText.text = AccuracyManager.Instance.accuracyResult;
    }
    public void arrowVisibility()
    {
        SpawnManager.Instance.setArrowVisibility();
    }
    public void arrowVisibilityOn()
    {
        SpawnManager.Instance.setArrowVisibility();
    }
}
