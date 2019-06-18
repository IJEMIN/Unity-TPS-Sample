using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();
            
            return instance;
        }
    }

    private int score;
    public bool isGameover { get; private set; }

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }
    
    public void AddScore(int newScore)
    {
        if (!isGameover)
        {
            score += newScore;
            UIManager.Instance.UpdateScoreText(score);
        }
    }
    
    public void EndGame()
    {
        isGameover = true;
        UIManager.Instance.SetActiveGameoverUI(true);
    }
}