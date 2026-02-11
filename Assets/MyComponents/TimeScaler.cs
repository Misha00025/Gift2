using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float FastSpeed = 3f;
    
    public void ToggleSpeed()
    {
        if (Time.timeScale > 1f)
            SetNormalSpeed();
        else
            SetFastSpeed();
    }
    
    public void SetNormalSpeed()
    {
        Time.timeScale = 1f;
    }
    
    public void SetFastSpeed()
    {
        Time.timeScale = FastSpeed;
    }
    
    public void OnDestroy()
    {
        SetNormalSpeed();
    }
}
