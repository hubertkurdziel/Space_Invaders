using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private GameObject winUI;

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;
    private bool isGameWon = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();

        NewGame();
    }


    private void Update()
    {
        if ((lives <= 0 || isGameWon) && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        isGameWon = false;
        Time.timeScale = 1f;

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);
        ResetAllBunkers();

        if (!mysteryShip.spawned)
        {
            mysteryShip.ResetTimer();
        }

        RespawnPlayer();
    }

    private void ContinueRound()
    {
        Projectile[] projectiles = FindObjectsOfType<Projectile>();
        foreach (var projectile in projectiles)
        {
            Destroy(projectile.gameObject);
        }

        if (mysteryShip != null && !mysteryShip.spawned)
        {
            mysteryShip.ResetTimer();
        }

        RespawnPlayer();
    }


    private void RespawnPlayer()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    public void ResetAllBunkers()
    {
        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(4, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = this.lives.ToString();
    }

    public void AddLife()
    {
        SetLives(this.lives + 1);
    }

    public void OnPlayerKilled(Player player)
    {
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0)
        {
            Invoke(nameof(ContinueRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    public void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.score);

        invaders.IncrementKilledCount();

        if (invaders.amountAlive == 0)
        {
            WinGame();
        }
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        SetScore(score + mysteryShip.score);
    }

    public void OnBoundaryReached()
    {
        if (invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
    }

    private void WinGame()
    {
        winUI.SetActive(true);
        isGameWon = true;

        Time.timeScale = 0f;

        invaders.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
    }
}