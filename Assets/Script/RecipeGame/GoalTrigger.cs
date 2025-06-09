using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimeSlider timer = FindObjectOfType<TimeSlider>();
            if (timer != null)
            {
                timer.ReachGoal();
            }
        }
    }
}
