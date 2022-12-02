using System.Collections.Generic;

public enum DISCREPENCY
{
  MY_UNITS_DECREASED,
  MY_UNITS_INCREASED,
  MY_UNITS_ZERO,
  MY_BOMBARDMENT_INCOMING,
  MY_BOMBS_ZERO,
  PLAYER_UNITS_DECREASED,
  PLAYER_UNITS_INCREASED,
  PLAYER_UNITS_ZERO,
  PLAYER_BOMBARDMENT_INCOMING,
  PLAYER_BOMBS_ZERO,
  PLAYER_FORCE_ADVANTAGE,
  MY_FORCE_ADVANTAGE,
}

/// <summary>
/// Foundation of this approach is Goal Driven Autonomy (GDA)
/// https://pdfs.semanticscholar.org/2e71/344f49e86be24798f3a1cbe20dded454b763.pdf
/// </summary>
public static class GdaDiscrepencies
{
  private static List<DISCREPENCY> discrepencies = new List<DISCREPENCY>();

  public static void GenerateFromState(TState oldState, TState newState)
  {
    discrepencies.Clear();

    if (newState.WorldObjectState.PlayerUnits.Count == 0) {
      discrepencies.Add(DISCREPENCY.PLAYER_UNITS_ZERO);
    } else if (oldState.WorldObjectState.PlayerUnits.Count > newState.WorldObjectState.PlayerUnits.Count) {
      discrepencies.Add(DISCREPENCY.PLAYER_UNITS_DECREASED);
    } else if (oldState.WorldObjectState.PlayerUnits.Count < newState.WorldObjectState.PlayerUnits.Count) {
      discrepencies.Add(DISCREPENCY.PLAYER_UNITS_INCREASED);
    }

    if (newState.WorldObjectState.PlayerBombs.Count == 0) {
      discrepencies.Add(DISCREPENCY.PLAYER_BOMBS_ZERO);
    } else {
      discrepencies.Add(DISCREPENCY.PLAYER_BOMBARDMENT_INCOMING);
    }

  }

  public static List<STRATEGY> StrategiesFromDiscrepencies()
  {
    return GdaExpectations.GenerateFromDiscrepencies(discrepencies);
  }
}
