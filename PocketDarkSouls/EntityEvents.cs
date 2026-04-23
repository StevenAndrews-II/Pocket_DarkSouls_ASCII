public class EntityEvents
{
    public event Action<int> OnHealRequested;
    public event Action<int> OnHitRequested;
    public void RaiseHeal(int amount) => OnHealRequested?.Invoke(amount);
    public void RaiseHit(int amount) => OnHitRequested?.Invoke(amount);
}