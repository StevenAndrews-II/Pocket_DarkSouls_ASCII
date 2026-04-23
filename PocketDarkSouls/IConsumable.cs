using System;

public interface IConsumable
{

    void Consume(EntityEvents events, int amount);
}
