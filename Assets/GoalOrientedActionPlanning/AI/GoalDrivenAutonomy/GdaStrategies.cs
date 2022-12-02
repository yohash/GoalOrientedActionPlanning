using System.Collections.Generic;

// AI goals are strategic level,
// goals are derived from these strategies
public enum STRATEGY
{
  BUILD_UNITS,
  BUILD_DEFENSES,
  DEFEND_AGAINST_BOMBARDMENT,
  DEFEND_AGAINST_UNIT_ATTACK,
  ATTACK_BOMBS_BOMBARDMENT,
  ATTACK_BOMBS_HEAVY,
  ATTACK_BOMBS_SPARSE,
  SCOUT_WITH_BOMBS,
}

/// <summary>
/// Foundation of this approach is Goal Driven Autonomy (GDA)
/// https://pdfs.semanticscholar.org/2e71/344f49e86be24798f3a1cbe20dded454b763.pdf
/// The output of GDA is goals, here we're just calling them "Strategies"
/// </summary>
public static class GdaStrategies
{
  public static List<STRATEGY> GenerateFromExpectations(List<EXPECTATION> expectations)
  {
    List<STRATEGY> strategy = new List<STRATEGY>();

    foreach (var expectation in expectations) {
      switch (expectation) {
        case EXPECTATION.NO_KNOWN_PLAYER_BOMBS:
          strategy.Add(STRATEGY.SCOUT_WITH_BOMBS);
          break;
        case EXPECTATION.PLAYER_BOMBARDMENT_INCOMING:
          strategy.Add(STRATEGY.DEFEND_AGAINST_BOMBARDMENT);
          break;
      }
    }

    return strategy;
  }
}
