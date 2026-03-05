using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gift2.Core; // если используете TextMeshPro

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;      // для иконки предмета
    [SerializeField] private TextMeshProUGUI amountText;      // или TMP_Text для количества
    
    // Скрытые поля
    [HideInInspector] public int Index;            // индекс слота в инвентаре
    [HideInInspector] public Inventory Inventory;  // ссылка на инвентарь (для взаимодействия)
    
    private InventorySlot currentSlot;
    
    // Можно добавить обработчики кликов, перетаскивания и т.п.
    
    /// <summary>
    /// Обновляет отображение слота на основе данных из InventorySlot.
    /// </summary>
    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;
        
        if (slot == null || slot.IsEmpty)
        {
            // Пустой слот
            if (iconImage != null) iconImage.enabled = false;
            if (amountText != null) amountText.text = "";
        }
        else
        {
            // Слот с предметом
            if (iconImage != null)
            {
                iconImage.sprite = slot.Item.Config.Icon;
                iconImage.enabled = true;
            }
            
            if (amountText != null)
            {
                amountText.SetText(slot.Amount > 1 ? slot.Amount.ToString() : "");
            }
        }
    }
    
    // Пример обработчика клика (можно подключить в инспекторе к Button)
    public void OnSlotClicked()
    {
        Debug.Log($"Слот {Index} нажат. Предмет: {currentSlot?.Item.Key}");
        // Здесь можно вызвать метод использования предмета из Inventory
    }
}