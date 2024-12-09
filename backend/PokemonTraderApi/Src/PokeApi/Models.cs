namespace PokemonTraderApi.PokeApi;
using System;
using System.Collections.Generic;

public class Ability
{
  public AbilityDetail ability { get; set; }
  public bool IsHidden { get; set; }
  public int Slot { get; set; }
}

public class AbilityDetail
{
  public string Name { get; set; }
  public string Url { get; set; }
}

public class Cry
{
  public string Latest { get; set; }
  public string Legacy { get; set; }
}

public class Form
{
  public string Name { get; set; }
  public string Url { get; set; }
}

public class Move
{
  public MoveDetail move { get; set; }
}

public class MoveDetail
{
  public string Name { get; set; }
  public string Url { get; set; }
}

public class Stat
{
  public int BaseStat { get; set; }
  public int Effort { get; set; }
  public StatDetail stat { get; set; }
}

public class StatDetail
{
  public string Name { get; set; }
  public string Url { get; set; }
}

public class Type
{
  public int Slot { get; set; }
  public TypeDetail type { get; set; }
}

public class TypeDetail
{
  public string Name { get; set; }
  public string Url { get; set; }
}

public class Pokemon
{
  public List<Ability> Abilities { get; set; }
  public int BaseExperience { get; set; }
  public Cry Cries { get; set; }
  public List<Form> Forms { get; set; }
  public int Height { get; set; }
  public int Id { get; set; }
  public bool IsDefault { get; set; }
  public string LocationAreaEncounters { get; set; }
  public List<Move> Moves { get; set; }
  public string Name { get; set; }
  public int Order { get; set; }
  public List<Ability> PastAbilities { get; set; }
  public List<string> PastTypes { get; set; }
  public Species Species { get; set; }
  public List<Stat> Stats { get; set; }
  public List<Type> Types { get; set; }
  public int Weight { get; set; }
}

public class Species
{
  public string Name { get; set; }
  public string Url { get; set; }
}

