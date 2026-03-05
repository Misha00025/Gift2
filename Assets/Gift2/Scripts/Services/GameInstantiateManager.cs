using Gift2.Meta;
using UnityEngine;

namespace Gift2
{
    public class GameInstantiateManager : MonoBehaviour
    {
        void Awake()
        {
        }
        
        void OnDestroy()
        {
            QuestsManager.Clear();
        }
    }
}
