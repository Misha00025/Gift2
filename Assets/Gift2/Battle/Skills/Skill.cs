using UnityEngine;
using UnityEngine.Events;

public interface ISkill
{
    public UnityEvent Completed { get; }
    public SkillInfo Info { get; }
    public void Play();
}

public interface ITargetSkill
{
    public bool IsValidTarget(Character target);
    public void SetTarget(Character target);        
}

public abstract class Skill : MonoBehaviour, ISkill
{
    public UnityEvent Completed {get; private set;} = new();
    public SkillInfo Info => GetInfo();
    protected abstract SkillInfo GetInfo();
    public bool InProgress {get; protected set;} = false;
    
    public abstract void Play();
    
    public void Complete()
    {
        Completed?.Invoke();
        InProgress = false;
    }
}

public abstract class Skill<TCharacter, TConfig> : Skill where TConfig: SkillConfig where TCharacter : Character
{
    [SerializeField] protected TCharacter Caster;
    [SerializeField] protected TConfig Config;

    protected override SkillInfo GetInfo() => Config.GetInfo();
    

    public override void Play()
    {
        if (Caster == null && !TryGetComponent(out Caster)) 
        {
            Debug.LogError("Caster is null");
            return;
        }
        if (InProgress) return;
        Caster.View.CancelAnimation();
        InProgress = true;
        OnPlay();
    }
    
    protected abstract void OnPlay();
    
}

public struct SkillInfo 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Sprite Icon { get; set; }
}