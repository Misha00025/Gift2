using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    public SkillUICard SkillPrefab;
    public float Space;
    
    private List<SkillUICard> _cards = new();
    
    public void SetupSkills(List<(Skill skill, Action play)> skills)
    {
        float position = 0f;
        foreach (var e in skills)
        {
            if (e.skill == null) continue;
            var card = Instantiate(SkillPrefab, transform);
            card.Button.onClick.AddListener(e.play.Invoke);
            card.SetupSkill(e.skill.Info);
            var rt = card.GetComponent<RectTransform>();
            rt.anchoredPosition = rt.anchoredPosition + new Vector2(0, position);
            position = rt.anchoredPosition.y - Space;
        }
    }
}
