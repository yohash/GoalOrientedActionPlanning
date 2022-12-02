using System.Collections.Generic;
/// <summary>
/// The final output of the GDA process is a list of
///   - AI_STRATEGIES
///       - final strategies based on state discrepencies
///
/// We convert the strategies to a list of
///   - AI_GOALS
///       - these describe goals the AI should aspire to
///
/// The GoapFinalState static class will convert AI_GOALS to
///   - GOAP_STATE
///       - this is final state, that we will then build
///         chains of actions to satisfy this final state
/// </summary>
public static class GoapGenerator
{
  public static Dictionary<AI_GOALS, List<GOAP_STATE>> ByAiGoal =
    new Dictionary<AI_GOALS, List<GOAP_STATE>>() {
      {AI_GOALS.BOMBARDMENT_SCOUTING,
        new List<GOAP_STATE>() {
          GOAP_STATE.LAUNCH_BOMBARDMENT_SCOUTING
        }
      },
      {AI_GOALS.BOMBARDMENT_DEFENSIVE,
        new List<GOAP_STATE>() {
          GOAP_STATE.LAUNCH_BOMBARDMENT_DEFENSIVE
        }
      },
      {AI_GOALS.BOMBARDMENT_PROBING,
        new List<GOAP_STATE>() {
          GOAP_STATE.LAUNCH_BOMBARDMENT_PROBING
        }
      },
      {AI_GOALS.BOMBARDMENT_CORE,
        new List<GOAP_STATE>() {
          GOAP_STATE.LAUNCH_BOMBARDMENT_CORE
        }
      },
    };
}
