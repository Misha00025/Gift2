using Gift2.Core;
using UnityEngine;

public abstract class ResourcesView : MonoBehaviour
{
    protected ResourcesStorage ResourcesStorage { get; private set; }

    public void Initialize(ResourcesStorage storage)
    {
        ResourcesStorage = storage;
        OnInitialize();
    }
    
    protected abstract void OnInitialize();
}
