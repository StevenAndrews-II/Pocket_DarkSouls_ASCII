public class EntityEvents
{
    private event Action<HealEvent>? OnHeal;
    private event Action<HitEvent>? OnHit;

    // subscribe/unsubscribe methods
    public void subscribeHeal(Action<HealEvent> handler)
    {
        OnHeal += handler;
    }
    public void unsubscribeHeal(Action<HealEvent> handler)
    {
        OnHeal -= handler;
    }
    
    public void subscribeHit(Action<HitEvent> handler)
    {
        OnHit += handler;
    }
    public void unsubscribeHit(Action<HitEvent> handler)
    {
        OnHit -= handler;
    }

    // raise events

    public void RaiseHeal(HealEvent data)
    {
        OnHeal?.Invoke(data);
    }

    public void RaiseHit(HitEvent data)
    {
        OnHit?.Invoke(data);
    }
}