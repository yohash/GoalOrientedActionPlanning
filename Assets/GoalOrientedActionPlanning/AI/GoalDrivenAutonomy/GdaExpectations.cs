using System.Collections.Generic;

public enum EXPECTATION
{
  NO_KNOWN_PLAYER_BOMBS,
  PLAYER_BOMBARDMENT_INCOMING,
}

/// <summary>
/// Foundation of this approach is Goal Driven Autonomy (GDA)
/// https://pdfs.semanticscholar.org/2e71/344f49e86be24798f3a1cbe20dded454b763.pdf
/// </summary>
public static class GdaExpectations
{
  public static List<STRATEGY> GenerateFromDiscrepencies(List<DISCREPENCY> discrepencies)
  {
    List<EXPECTATION> expectations = new List<EXPECTATION>();

    foreach (var discrepency in discrepencies) {
      switch (discrepency) {
        case DISCREPENCY.PLAYER_BOMBARDMENT_INCOMING:
          expectations.Add(EXPECTATION.PLAYER_BOMBARDMENT_INCOMING);
          break;
        case DISCREPENCY.PLAYER_BOMBS_ZERO:
          expectations.Add(EXPECTATION.NO_KNOWN_PLAYER_BOMBS);
          break;
      }
    }

    return GdaStrategies.GenerateFromExpectations(expectations);
  }
}
