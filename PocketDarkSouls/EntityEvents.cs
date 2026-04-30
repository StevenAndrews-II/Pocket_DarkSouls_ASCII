public class EntityEvents
{
    public event Action<HealEvent>? OnHeal;
    public event Action<HitEvent>? OnHit;

    public void RaiseHeal(HealEvent data)
    {
        OnHeal?.Invoke(data);
    }

    public void RaiseHit(HitEvent data)
    {
        OnHit?.Invoke(data);
    }
}