using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AiEngine
{
  [SerializeField] private List<AiActionChain> actionChains;

  [SerializeField] private List<STRATEGY> strategies;

  public bool AllActionsComplete {
    get { return actionChains.Count == 0; }
  }

  public AiEngine()
  {
    actionChains = new List<AiActionChain>();
    strategies = new List<STRATEGY>();
  }

  public void ProcessActionChains()
  {
    var toDelete = new List<AiActionChain>();
    // go through every chain
    foreach (var chain in actionChains) {
      if (!chain.ProcessActions()) {
        // An Action in the chain has failed
        toDelete.Add(chain);
      }

      if (chain.ActionChainComplete()) {
        // the chain has completed processing
        toDelete.Add(chain);
      }
    }

    // remove complete/failures
    foreach (var del in toDelete) {
      actionChains.Remove(del);
    }
  }

  public void UpdateAiStrategies(TState oldState, TState newState)
  {
    // generate discrepencies
    GdaDiscrepencies.GenerateFromState(oldState, newState);

    // obtain new strategies from discrepencies
    strategies.Clear();
    strategies.AddRange(GdaDiscrepencies.StrategiesFromDiscrepencies());

    // update action chains from our strategies
    UpdateActionsFromStrategies(newState);
  }

  public void UpdateActionsFromStrategies(TState newState)
  {
    // go over each strategy
    foreach (var strat in strategies) {
      // store the desired final action
      var final = AiActionGenerator.FromStrategy(strat);
      // see if any of our action chains contain this final goal
      if (!actionChains.Any(ch => ch.FinalAction.GetType() == final.GetType())) {
        actionChains.Add(new AiActionChain(newState, final));
      }
    }
  }
}
