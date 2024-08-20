using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public delegate void DestroyedHandler();
    public event DestroyedHandler OnDestroyed;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}