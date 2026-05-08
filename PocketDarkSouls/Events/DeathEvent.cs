using PocketDarkSouls;
public sealed class DeathEvent
{
    public string EntityName { get; }

    public DeathEvent(string entityName)
    {
        EntityName = entityName;
    }
}