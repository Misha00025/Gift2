// EffectsBar.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsBar : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private Character _character;
    [SerializeField] private EffectIconUI _effectIconPrefab;
    [SerializeField] private int _maxVisibleIcons = 5;
    [SerializeField] private float _spacing = 10f;
    
    [Header("Индикатор переполнения")]
    [SerializeField] private MoreIndicator _moreIndicatorPrefab;
    
    private List<EffectIconUI> _effectIcons = new List<EffectIconUI>();
    private RectTransform _rectTransform;
    private float _iconWidth = 0f;
    private MoreIndicator _moreIndicator;
    private Dictionary<Type, (IEffect Example, int Count)> _groupedEffects;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        var prefabRect = _effectIconPrefab.GetComponent<RectTransform>();
        _iconWidth = prefabRect.rect.width;
        
        CreateIconPool();
        CreateMoreIndicator();
    }

    void CreateIconPool()
    {
        for (int i = 0; i < _maxVisibleIcons; i++)
        {
            var icon = Instantiate(_effectIconPrefab, transform);
            icon.gameObject.SetActive(false);
            _effectIcons.Add(icon);
        }
    }

    void CreateMoreIndicator()
    {
        if (_moreIndicatorPrefab != null)
        {
            _moreIndicator = Instantiate(_moreIndicatorPrefab, transform);
            _moreIndicator.gameObject.SetActive(false);
            
            var indicatorRect = _moreIndicator.GetComponent<RectTransform>();
            indicatorRect.sizeDelta = new Vector2(_iconWidth, _iconWidth);
        }
    }

    void LateUpdate()
    {
        if (_character != null)
        {
            UpdateEffectsDisplay();
        }
    }

    void UpdateEffectsDisplay()
    {
        // Группируем эффекты по типу
        _groupedEffects = EffectGrouper.GroupEffectsByType(_character.Effects);
        
        // Рассчитываем, сколько типов эффектов показываем
        var effectTypes = new List<Type>(_groupedEffects.Keys);
        bool showMoreIndicator = effectTypes.Count > _maxVisibleIcons;
        
        // Если показываем индикатор, то показываем на один тип меньше
        int visibleTypesCount = showMoreIndicator ? _maxVisibleIcons - 1 : effectTypes.Count;
        
        // Обновляем иконки для видимых типов эффектов
        for (int i = 0; i < _maxVisibleIcons; i++)
        {
            bool shouldBeActive = i < visibleTypesCount;
            _effectIcons[i].gameObject.SetActive(shouldBeActive);
            
            if (shouldBeActive)
            {
                var effectType = effectTypes[i];
                var (exampleEffect, count) = _groupedEffects[effectType];
                
                // Инициализируем или обновляем иконку
                if (_effectIcons[i].Effect == null || _effectIcons[i].Effect.GetType() != effectType)
                {
                    _effectIcons[i].Initialize(exampleEffect);
                }
                
                _effectIcons[i].SetCount(count);
                
                // Рассчитываем позицию
                float totalWidth = CalculateTotalWidth(visibleTypesCount + (showMoreIndicator ? 1 : 0));
                float startX = -totalWidth / 2f + _iconWidth / 2f;
                float posX = startX + i * (_iconWidth + _spacing);
                
                _effectIcons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
            }
            else
            {
                // Сбрасываем ссылку на эффект при деактивации
                _effectIcons[i].Initialize(null);
            }
        }
        
        // Обновляем индикатор переполнения
        UpdateMoreIndicator(showMoreIndicator, effectTypes.Count, visibleTypesCount);
    }

    void UpdateMoreIndicator(bool showIndicator, int totalTypes, int visibleTypes)
    {
        if (_moreIndicator == null) return;
        
        if (showIndicator)
        {
            int remainingTypes = totalTypes - visibleTypes;
            _moreIndicator.SetCount(remainingTypes);
            _moreIndicator.gameObject.SetActive(true);
            
            // Позиционируем индикатор
            float totalWidth = CalculateTotalWidth(visibleTypes + 1);
            float startX = -totalWidth / 2f + _iconWidth / 2f;
            float posX = startX + visibleTypes * (_iconWidth + _spacing);
            _moreIndicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        }
        else
        {
            _moreIndicator.gameObject.SetActive(false);
        }
    }

    float CalculateTotalWidth(int itemCount)
    {
        if (itemCount <= 0) return 0;
        return (itemCount * _iconWidth) + ((itemCount - 1) * _spacing);
    }
    
    // Метод для получения информации о группировке (может пригодиться для отладки)
    public string GetGroupingInfo()
    {
        if (_groupedEffects == null) return "No effects grouped";
        
        var info = $"Total effect types: {_groupedEffects.Count}\n";
        
        foreach (var kvp in _groupedEffects)
        {
            info += $"- {kvp.Key.Name}: {kvp.Value.Count} instances\n";
        }
        
        return info;
    }
}


public static class EffectGrouper
{
   
    public static Dictionary<Type, (IEffect Example, int Count)> GroupEffectsByType(IReadOnlyList<IEffect> effects)
    {
        var result = new Dictionary<Type, (IEffect, int)>();
        
        if (effects == null || effects.Count == 0)
            return result;
        
        foreach (var effect in effects)
        {
            if (effect == null) continue;
            
            var effectType = effect.GetType();
            
            if (result.ContainsKey(effectType))
            {
                var (example, count) = result[effectType];
                result[effectType] = (example, count + 1);
            }
            else
            {
                result[effectType] = (effect, 1);
            }
        }
        
        return result;
    }
}