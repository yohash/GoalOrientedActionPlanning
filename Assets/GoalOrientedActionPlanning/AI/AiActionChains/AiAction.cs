using System.Collections.Generic;

public abstract class AiAction
{
  public List<AiAction> Predecessors { get; private set; }
  public List<AiAction> Subsequent { get; private set; }

  public enum STATUS { WAITING, PROCESSING, COMPLETE, FAILED }
  public STATUS Status;

  public AiAction()
  {
    Predecessors = new List<AiAction>();
    Subsequent = new List<AiAction>();
    Status = STATUS.WAITING;
  }

  public abstract void Initiate(TState state);
  public abstract void Initiate(TState state, AiAction subsequent);
  public abstract void PerformAction();

  public void PostInitiate(TState state)
  {
    foreach (var pre in Predecessors) {
      pre.Initiate(state, this);
    }
  }

  public bool IsReady()
  {
    bool ready = true;
    foreach (var pre in Predecessors) {
      ready &= pre.Status == STATUS.COMPLETE;
    }
    return ready;
  }
}
