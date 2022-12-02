using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class GoapChain
{
  public int Current;

  public int Total;
  public List<GoapAction> ActionChain;

  public GoapAction FinalGoal;
  public float ChainCost;

  public bool Complete = false;

  // debugging vars
  public string D_FinalGoal;
  public string D_CurrentGoal;
  public List<string> D_GoalList;
  public List<float> D_Costs;

  public GoapChain(List<GoapAction> chain, TState state)
  {
    ActionChain = chain;
    FinalGoal = chain.Last();

    // initialize the index tracker
    Current = 0;
    // calculate the chain's total cost
    ChainCost = 0;

    // initiate first action in the chain solely from state
    chain.Last().ComputeCost(state);
    UnityEngine.Debug.Log("Building chain, final: " + chain.Last().GetType().ToString());
    for (int i = chain.Count - 2; i >= 0; i--) {
      // compute costs backwards seeding each action with its following

      UnityEngine.Debug.Log("\t"+chain[i].GetType().ToString());
      chain[i].ComputeCost(state, chain[i + 1]);
    }

    // compute aggregate cost
    ChainCost = chain.Sum(s => s.Cost);
    Total = chain.Count;

    D_GoalList = new List<string>();
    D_Costs = new List<float>();
    foreach (var axn in chain) {
      D_GoalList.Add(axn.GetType().ToString());
      D_Costs.Add(axn.Cost);
    }
    D_FinalGoal = FinalGoal.GetType().ToString();
    D_CurrentGoal = chain[Current].GetType().ToString();
  }

  public void AssessGoals()
  {
    if (Current >= ActionChain.Count) { return; }

    ActionChain[Current].PerformAction();

    if (ActionChain[Current].IsComplete()) {
      Current++;
      Complete = Current >= ActionChain.Count;

      D_CurrentGoal = Complete ? "" : ActionChain[Current]?.GetType().ToString();
    }
  }
}
