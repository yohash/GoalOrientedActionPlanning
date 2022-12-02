using System.Collections.Generic;
using System.Linq;

public static class GoapPlanner
{
  // big list of all possible goap actions
  private static List<GoapAction> allActions = new List<GoapAction>() {
    new _FireBombardmentScouting(),
    new _FireBombardmentDefensive(),
    new _SteerLauncher(),
    new _FireTerrabombs(),
    new _HasTerralauncher(),
    new _HasMatter(),
  };

  public static List<List<GoapAction>> ActionChainsFromAiGoals(List<AI_GOALS> aiGoals)
  {
    List<GoapAction> desiredFinalActions = new List<GoapAction>();

    foreach (var aiGoal in aiGoals) {
      // (1) from AI_GOALS, extract GOAP_STATE
      foreach (var goapState in GoapGenerator.ByAiGoal[aiGoal]) {
        // (2) from list of GOAP_STATEs, add all goap actions that have
        //     this state as one of their Effects
        desiredFinalActions.AddRange(
          allActions.Where(action => action.Effects.Any(effect => effect == goapState))
        );
      }
    }
    // (3) return chains that accomplish each final action
    return PlanActionChainsFromFinalAction(desiredFinalActions);
  }

  public static List<List<GoapAction>> PlanActionChainsFromFinalAction(List<GoapAction> finalActions)
  {
    // create a list of goap action chains
    var actionChain = new List<List<GoapAction>>();

    // back-propagate from each final action to find lists of viable actions
    foreach (var action in finalActions) {
      // foreach desired final action, create a new list in the action chain
      actionChain.Add(new List<GoapAction>() { action });
    }

    // create a list of indeces that have no solution
    var remove = new List<int>();

    // scroll through each desired final action state to build a chain
    for (int i = 0; i < actionChain.Count; i++) {
      bool keepSearching = true;
      do {
        // find out which preconditions are not yet satisfied
        List<GOAP_STATE> unsatisfied = unsatisfiedPreconditions(actionChain[i]);

        if (unsatisfied.Count == 0) {
          // all preconditions are satisfied
          keepSearching = false;
        } else {
          // sweep through the list once, adding actions to satisfy preconditions
          foreach (var precondition in unsatisfied) {
            List<GoapAction> satisfies = new List<GoapAction>();
            // If there are any remaining preconditions, scroll through the actions
            foreach (var action in allActions) {
              // find any action where effect == precondition
              foreach (var effect in action.Effects) {
                if (precondition == effect) {
                  // store this action as satisfying a precondition
                  satisfies.Add(action);
                }
              }
            }

            // (1) there are multiple solutions for this precondition
            if (satisfies.Count > 1) {
              // make N-1 copies of the action chain as-is, and append
              // satifying actions 1 through N to each new chain
              for (int k = 1; k < satisfies.Count; k++) {
                // make sure the
                if (!actionChain[i].Contains(satisfies[k])) {
                  // duplicate the list and append this new action
                  List<GoapAction> copy = new List<GoapAction>();
                  copy.AddRange(actionChain[i]);
                  copy.Add(satisfies[k]);
                  // add to our list of action chains as a new chain
                  actionChain.Add(copy);
                }
              }
            }

            // (2) if there is at least 1 satisfactory action, add it to the chain
            if (satisfies.Count > 0) {
              if (!actionChain[i].Contains(satisfies[0])) {
                actionChain[i].Add(satisfies[0]);
              }
            }

            // (3) if there are no actions that satisfy the preconditions, do we quit?
            if (satisfies.Count == 0) {
              if (!remove.Contains(i)) { remove.Add(i); }
              keepSearching = false;
            }
          }
        }
      } while (keepSearching);
    }

    // purge all action chains that cannot be satisfied
    for (int i = remove.Count - 1; i >= 0; i--) {
      actionChain.RemoveAt(remove[i]);
    }

    // reverse each action chain so they are in order
    for (int i = 0; i < actionChain.Count; i++) {
      actionChain[i].Reverse();
    }

    return actionChain;
  }

  private static List<GOAP_STATE> unsatisfiedPreconditions(List<GoapAction> chain)
  {
    List<GOAP_STATE> remaining = new List<GOAP_STATE>();
    // list all the preconditions
    foreach (var comparer in chain) {
      foreach (var precond in comparer.Preconditions) {
        if (!remaining.Contains(precond)) {
          remaining.AddRange(comparer.Preconditions);
        }
      }
    }
    // list all the effects
    foreach (var comparand in chain) {
      foreach (var effect in comparand.Effects) {
        // take the "difference"
        if (remaining.Contains(effect)) {
          remaining.RemoveAt(remaining.IndexOf(effect));
        }
      }
    }

    // anything that remains needs to be satisfied
    return remaining;
  }
}
