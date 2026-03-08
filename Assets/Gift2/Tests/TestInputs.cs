using System;
using System.Collections.Generic;
using Gift2;
using Gift2.Core;
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
    public QuestDialer QuestDialer;
    public CharacterMover CharacterMover;
    public List<KeyItem> Resources;
    public Joystick Joystick;
    private Interactor _interactor;
    
    private bool BlockReadInputs => DialogueUI.instance.gameObject.activeSelf;
    
    private InputAction _moveAction;
    private InputAction _interactAction;
    
    void Start()
    {
        var collector = FindAnyObjectByType<ItemsCollector>();
        collector?.SetStorage(Player.ResourcesStorage);
        _interactor = FindAnyObjectByType<Interactor>();
        
        _moveAction = InputSystem.actions.FindActionMap("Player").FindAction("Move");
        _interactAction = InputSystem.actions.FindActionMap("Player").FindAction("Interact");
    }
    
    void Update()
    {
        if (_interactAction.IsPressed())
            Use();
            
        HandleMoving();
    }
    
    private void HandleMoving()
    {
        if (BlockReadInputs) return;
        var moveInput = _moveAction.ReadValue<Vector2>();
        Debug.Log($"Move input: {moveInput.x}, {moveInput.y}");
        
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
    
    public void Use()
    {
        if (DialogueUI.instance.gameObject.activeSelf)
        {
            DialogueUI.instance.NextSentenceSoft();
            return;
        }
    
        if (BlockReadInputs) return;
        
        var obj = _interactor.GetSelectedObject();
        
        obj?.Use();
    }
}
