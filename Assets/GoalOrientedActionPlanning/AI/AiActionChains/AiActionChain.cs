using System.Collections.Generic;

[System.Serializable]
public class AiActionChain
{
  public List<D_AiAction> D_Actions;
  public List<D_AiAction> D_Current;

  public List<AiAction> CurrentActions;

  public AiAction FinalAction;

  public AiActionChain(TState state, AiAction final)
  {
    CurrentActions = new List<AiAction>();

    D_Actions = new List<D_AiAction>();

    if (final == null) { return; }

    FinalAction = final;
    // initiate the chain by starting with the final goal
    final.Initiate(state);

    var allActions = new List<AiAction>();
    recursiveActionAdd(final, allActions);

    D_Actions.Add(new D_AiAction(final));

    foreach (var action in allActions) {
      D_Actions.Add(new D_AiAction(action));
      if (action.Predecessors.Count == 0) {
        // this action has no predecessors, it is a root action
        CurrentActions.Add(action);
      }
    }
  }

  public bool ProcessActions()
  {
    var completeActions = new List<AiAction>();
    var newActions = new List<AiAction>();

    foreach (var action in CurrentActions) {
      // perform the action if predecessors are complete
      if (action.IsReady()) { action.PerformAction(); }

      // check on the status
      switch (action.Status) {
        case AiAction.STATUS.WAITING:
        case AiAction.STATUS.PROCESSING:
          // do nothing?
          break;
        case AiAction.STATUS.COMPLETE:
          // enqueue the following actions
          foreach (var following in action.Subsequent) {
            // only add a new action if each of its Predecessors are complete
            if (following.IsReady()) { newActions.Add(following); }
          }
          // action is complete, remove from current
          completeActions.Add(action);
          break;
        case AiAction.STATUS.FAILED:
          // return the failure status
          return false;
      }
    }

    // remove the actions we have completed
    foreach (var complete in completeActions) {
      CurrentActions.Remove(complete);
    }

    // add the new actions
    CurrentActions.AddRange(newActions);

    D_Current = new List<D_AiAction>();
    foreach (var current in CurrentActions) {
      D_Current.Add(new D_AiAction(current));
    }

    // return a success if no failure at this point
    return true;
  }

  public bool ActionChainComplete()
  {
    return FinalAction.Status == AiAction.STATUS.COMPLETE;
  }

  private void recursiveActionAdd(AiAction current, List<AiAction> actionList)
  {
    foreach (var action in current.Predecessors) {
      actionList.Add(action);
      recursiveActionAdd(action, actionList);
    }
  }
}

[System.Serializable]
public class D_AiAction
{
  public string ActionType;
  public List<string> Following;
  public List<string> Preceeding;

  public D_AiAction(AiAction axn)
  {
    ActionType = axn.GetType().ToString();
    Following = new List<string>();
    Preceeding = new List<string>();

    foreach (var pre in axn.Predecessors) {
      Preceeding.Add(pre.GetType().ToString());
    }
    foreach (var fol in axn.Subsequent) {
      Following.Add(fol.GetType().ToString());
    }
  }
}
