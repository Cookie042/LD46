using UnityEngine;
public class LifetimeDisolve : MonoBehaviour
{
    public float Lifetime = 5;
    
    void Update()
    {
        Lifetime -= Time.deltaTime;
        
        
    }
}
