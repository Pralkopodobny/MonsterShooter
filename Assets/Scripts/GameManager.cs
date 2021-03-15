using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public interface IListener
{
    void Notify(Diablo diablo);
}

public class GameManager : MonoBehaviour, IListener
{
    private const float MAX_DISTANCE_Y = 400.0f;
    private const float MAX_DISTANCE_X = 600.0f;
    private const int MIN_SIZE = 100;
    private const int MAX_SIZE = 200;
    [SerializeField] private Diablo diabloPrefab;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI congratulationsText, helpText;

    private int enemyCount;
    private bool gameHasEnded;
    private float timer;
    private List<Diablo> diablos;

    
    private void Awake()
    {
        enemyCount = Options.count;
        diablos = new List<Diablo>();
        //controls.Game.Restart.performed += context => RestartGame();
        //controls.Game.GoToMenu.performed += context => GoToMenu();
    }
    void Start()
    {
        InitializeGame();
    }

    private Vector2 randomiseAnchors()
    {
        return new Vector2(Random.Range(0, 2), Random.Range(0, 2));
    }
    private Vector3 randomisePosition(Vector2 anchor, int widthAndHeight)
    {
        int x, y;
        x = anchor.x == 1 ? -1 : 1;
        y = anchor.y == 1 ? -1 : 1;
        return new Vector3(Random.Range(widthAndHeight / 2 + 50, MAX_DISTANCE_X) * x, Random.Range(widthAndHeight / 2 + 50, MAX_DISTANCE_Y) * y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameHasEnded)
        {
            timer += Time.deltaTime;
            timeText.text = string.Format("{0:0.00}s", timer);
        }
    }

    public void Notify(Diablo diablo)
    {
        diablos.Remove(diablo);
        if (--enemyCount <= 0)
        {
            gameHasEnded = true;
            timeText.text = string.Format("{0:0.00}s", timer);
            countText.text = $"Remaining: {enemyCount}";
            congratulationsText.gameObject.SetActive(true);
            helpText.gameObject.SetActive(true);

        }
        else
        {
            countText.text = $"Remaining: {enemyCount}";
        }
    }

    private void InitializeGame()
    {
        timer = 0.0f;
        enemyCount = Options.count;
        gameHasEnded = false;
        for (int i = 0; i < enemyCount; i++)
        {
            var diablo = Instantiate<Diablo>(diabloPrefab, gameObject.transform);
            diablo.SetListener(this);
            var widthAndHeight = Random.Range(MIN_SIZE, MAX_SIZE);
            var rectTransform = diablo.GetComponent<RectTransform>();
            var anchor = randomiseAnchors();
            rectTransform.sizeDelta = new Vector2(widthAndHeight, widthAndHeight);
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.anchoredPosition = randomisePosition(anchor, widthAndHeight);
            diablos.Add(diablo);
        }

        countText.text = $"Remaining: {enemyCount}";
    }

    public void RestartGame(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            foreach (var diablo in diablos)
            {
                GameObject.Destroy(diablo.gameObject);
            }
            diablos.Clear();
            
            InitializeGame();
        }
        congratulationsText.gameObject.SetActive(false);
        helpText.gameObject.SetActive(false);
    }

    public void GoToMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SceneManager.LoadScene(0);
        }
    }
}
