using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public Text scoreText; 

    [ContextMenu("increase score")]
    public void increaseScore()
    {
        playerScore += 1;
        scoreText.text = playerScore.ToString();
    }
}
