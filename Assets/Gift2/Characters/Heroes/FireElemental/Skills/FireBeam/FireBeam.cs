using UnityEngine;

public class FireBeam : Skill<FireElemental, SkillConfig>
{
    public FireBeamView ViewPrefab;
    public Damage Damage = new Damage(){Value = 20, Element = Element.Fire};
    public EffectConfiguredBuilder EffectBuilder;
    public int Count = 5;
    
    private FireBeamView _view = null;
    private int _counts = 0;

    private FireBeamView CreateView()
    {
        var view = Instantiate(ViewPrefab, Caster.transform);
        view.gameObject.SetActive(false);
        view.Completed.AddListener(OnAnimationEnd);
        return view;
    }

    protected override void OnPlay()
    {
        if (_view == null)
            _view = CreateView();
        
        _counts = 0;
        _view.Place(Caster.Target.Pivot.transform.position);
        Caster.CastHandler.Casted.AddListener(OnCastFireBeam);
        Caster.CastHandler.Ended.AddListener(OnCastAnimationEnd);
        Caster.Animator.Play("Cast");
    }
    
    private void OnCastFireBeam()
    {
        Caster.CastHandler.Casted.RemoveListener(OnCastFireBeam);
        _view.gameObject.SetActive(true);
        _view.Cast(OnEffect);
    }
    
    
    private void OnEffect()
    {
        Caster.Target.ApplyDamage(Damage);
        for (int i = 0; i < Count; i++)
        {
            Caster.Target.ApplyEffect(EffectBuilder.Build());
        }
    }
    
    private void OnCastAnimationEnd()
    {
        Caster.CastHandler.Ended.RemoveListener(OnCastAnimationEnd);
        OnAnimationEnd();
    }
    
    public void OnAnimationEnd()
    {
        _counts += 1;
        if (_counts >= 2)
        {
            Complete();
            _view.gameObject.SetActive(false);
        }
    }
}
