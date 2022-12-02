using System.Collections.Generic;

/// <summary>
/// Processing order:
///   (1) ComputeCost() with state to store local
///       vars and simultaneously compute cost of this
///       action
///   (2) A call to perform action will use the stored
///       state variables to perform actions
///   (3) when the call to perform action determines
///       this action has satisfied completion requirements
///       we will set completion var complete
/// </summary>
public abstract class GoapAction
{
  public List<GOAP_STATE> Preconditions;
  public List<GOAP_STATE> Effects;

  protected bool complete;

  public float Cost = 0;

  public GoapAction()
  {
    Preconditions = new List<GOAP_STATE>();
    Effects = new List<GOAP_STATE>();
  }
  public bool IsComplete()
  {
    return complete;
  }

  public abstract void ComputeCost(TState state);
  public abstract void ComputeCost(TState state, GoapAction subsequent);
  public abstract void PerformAction();
}
