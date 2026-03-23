using Gift2.Core;
using Gift2.Meta;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wof.InventoryManagement.UI;

namespace Gift2
{
    public class GameInstantiateManager : MonoBehaviour
    {
        void Start()
        {
            var player = FindAnyObjectByType<Player>();
            var inventoryView = FindAnyObjectByType<InventoryDisplay>();
            inventoryView?.Initialize(player.Inventory);
            var camera = Camera.main; 
            camera.transparencySortMode = TransparencySortMode.CustomAxis; camera.transparencySortAxis = Vector3.up;
        }
        
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        void OnDestroy()
        {
            QuestsManager.Clear();
        }
    }
}
