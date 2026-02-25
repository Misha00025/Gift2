using System.Collections.Generic;
using Gift2.Core;
using UnityEngine;

public class TestInputs : MonoBehaviour
{
    public Player Player;
    
    public List<ItemConfig> Resources;
    
    public int Count = 100;
    
    private Item _currentResource;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _currentResource = Resources[0].Build();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _currentResource = Resources[1].Build();
            
        if (Input.GetKeyDown(KeyCode.A))
            Player.ResourcesStorage.Add(_currentResource, Count);
        if (Input.GetKeyDown(KeyCode.D))
            Player.ResourcesStorage.Remove(_currentResource, Count);
    }
}
