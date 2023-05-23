#region

using UnityEngine;

#endregion

namespace Units.Player
{
    public class EnemyDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Enemy.Enemy>(out var enemy))
            {
                enemy.PlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Enemy.Enemy>(out var enemy))
            {
                enemy.PlayerInRange = false;
            }
        }
    }
}
