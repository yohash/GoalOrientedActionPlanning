using System.Collections.Generic;

// likely deprecated
public enum AI_GOALS
{
  BOMBARDMENT_SCOUTING,
  BOMBARDMENT_PROBING,
  BOMBARDMENT_DEFENSIVE,
  BOMBARDMENT_CORE,
}

/// <summary>
/// Foundation of this approach is Goal Driven Autonomy (GDA)
/// https://pdfs.semanticscholar.org/2e71/344f49e86be24798f3a1cbe20dded454b763.pdf
/// Goals are derived from Strategies in this class
/// </summary>
public static class GdaGoals
{
  // likely deprecated
  public static Dictionary<STRATEGY, List<AI_GOALS>> ByStrategy =
    new Dictionary<STRATEGY, List<AI_GOALS>>() {
      {STRATEGY.DEFEND_AGAINST_BOMBARDMENT,
        new List<AI_GOALS>() {
          AI_GOALS.BOMBARDMENT_DEFENSIVE,
        }
      },
      {STRATEGY.ATTACK_BOMBS_BOMBARDMENT,
        new List<AI_GOALS>() {
          AI_GOALS.BOMBARDMENT_PROBING,
        }
      },
      {STRATEGY.BUILD_DEFENSES,
        new List<AI_GOALS>() { }
      },
      {STRATEGY.BUILD_UNITS,
        new List<AI_GOALS>() { }
      }
    };
}
