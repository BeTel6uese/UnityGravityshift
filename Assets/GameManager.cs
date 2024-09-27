using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public float totalTime = 120f;
    private bool gameActive = true;
   
    public int totalCollectibles = 5; 
    private int collectedObjects = 0;

    public TMP_Text timerText;           
    public TMP_Text gameStatusText;      
    public TMP_Text remainingCollectiblesText;

    void Start()
    {
        StartCoroutine(StartTimer());    
        UpdateRemainingCollectiblesUI();
    }

    public bool IsGameActive()
    {
        return gameActive;
    }

    // Timer coroutine
    IEnumerator StartTimer()
    {
        while (totalTime > 0 && gameActive)
        {
            yield return new WaitForSeconds(1f);  
            totalTime--;                         
            UpdateTimerUI();                     
        }
     
        if (gameActive)
        {
            GameFailed();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60); 
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }

    public void CollectObject()
    {
        collectedObjects++;                      

        UpdateRemainingCollectiblesUI();

        if (collectedObjects >= totalCollectibles)
        {
            GameCompleted();                    
        }
    }

    void UpdateRemainingCollectiblesUI()
    {
        int remainingCollectibles = totalCollectibles - collectedObjects; 
        remainingCollectiblesText.text = "Cubes Remaining: " + remainingCollectibles.ToString(); 
    }

    void GameCompleted()
    {
        gameActive = false;                          
        gameStatusText.text = "Game Completed!";     
        gameStatusText.gameObject.SetActive(true);  
    }

    void GameFailed()
    {
        gameActive = false;                          
        gameStatusText.text = "Game Failed!";        
        gameStatusText.gameObject.SetActive(true);  
    }
}
