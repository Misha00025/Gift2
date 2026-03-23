using Gift2.Core;
using UnityEngine;
using Wof.InventoryManagement.UI;

public class HUDController : MonoBehaviour
{
    public Player Player;
    public ResourcesView ResourcesView;
    
    void Start()
    {
        ResourcesView?.Initialize(Player.ResourcesStorage);
    }
}
