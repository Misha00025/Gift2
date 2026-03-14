using System;
using System.Collections.Generic;
using Gift2;
using Gift2.Core;
using Gift2.Meta;
using HeneGames.DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputs : MonoBehaviour
{
    [Serializable]
    public struct KeyItem
    {
        public KeyCode Key;
        public ItemConfig Item;
    }

    public Player Player;
    public CharacterMover CharacterMover;
    public List<KeyItem> Resources;
    public ShopView Shop;
    public Joystick Joystick;
    private Interactor _interactor;
    
    private bool BlockReadInputs => DialogueUI.instance.gameObject.activeSelf || (Shop != null && Shop.gameObject.activeSelf);
    
    private InputAction _moveAction;
    private InputAction _interactAction;
    private InputAction _cheatAction;
    private InputAction _cancelAction;
    
    void Start()
    {
        var collector = FindAnyObjectByType<ItemsCollector>();
        collector?.SetStorage(Player.ResourcesStorage);
        _interactor = FindAnyObjectByType<Interactor>();
        var playerActions = InputSystem.actions.FindActionMap("Player");
        var uiActions = InputSystem.actions.FindActionMap("UI");
        playerActions.Enable();
        uiActions.Enable();
        _moveAction = playerActions.FindAction("Move");
        _interactAction = playerActions.FindAction("Interact");
        _cheatAction = playerActions.FindAction("Cheat");

        _cancelAction = uiActions.FindAction("Cancel");
    }
    
    void Update()
    {
        if (_cheatAction.WasPressedThisFrame())
            Cheat();
    
        if (_interactAction.WasPressedThisFrame())
            UseOrSkip();
            
        if (_cancelAction.WasPressedThisFrame())
            Shop.CloseShop();
            
        HandleMoving();
    }
    
    private void Cheat()
    {
        var quests = QuestsManager.Instance.Quests;
        for (int i = quests.Count -1; i>=0; i--)
            quests[i].Complete();
    }
    
    private void HandleMoving()
    {
        if (BlockReadInputs) 
        {
            CharacterMover.Move(Vector2.zero);
            return;
        }
        var moveInput = _moveAction.ReadValue<Vector2>();
        
        var xDirection = moveInput.x;
        var yDirection = moveInput.y;
        if (Joystick != null)
        {
            xDirection += Joystick.Horizontal;
            yDirection += Joystick.Vertical;
        }
        var direction = new Vector2(xDirection, yDirection);
        
        CharacterMover.Move(direction);
    }
    
    private void UseOrSkip()
    {
        if (DialogueUI.instance.gameObject.activeSelf)
        {
            DialogueUI.instance.NextSentenceSoft();
        }
        else
        {
            Use();
        }
    }
    
    public void Use()
    {
        if (BlockReadInputs) return;
        
        var obj = _interactor.GetSelectedObject();
        
        obj?.Use();
    }
}
