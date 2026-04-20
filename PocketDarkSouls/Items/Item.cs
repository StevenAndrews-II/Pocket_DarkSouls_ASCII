using System;

public abstract class Item  // add description
{
	#nullable enable // context modifier 
    public string id { get; init; }
	public int numberOf { get; set; }
	public double mass { get; init; }

	public double price { get; init; }

	public abstract override string ToString();

}
