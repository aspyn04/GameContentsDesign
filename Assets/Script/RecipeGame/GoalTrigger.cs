using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MinigameManager timer = FindObjectOfType<MinigameManager>();
            if (timer != null)
            {
                timer.ReachGoal();
            }
        }
    }
}
