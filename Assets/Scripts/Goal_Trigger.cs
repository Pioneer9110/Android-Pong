using UnityEngine;

public class Goal_Trigger : MonoBehaviour
{
    public bool isLeftGoal; // True = Left goal (AI scores), False = Right goal (Player scores)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Game_Manager gm = FindObjectOfType<Game_Manager>();
            gm.ScorePoint(!isLeftGoal);
        }
    }
}
