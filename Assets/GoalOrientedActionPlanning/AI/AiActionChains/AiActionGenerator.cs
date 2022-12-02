public static class AiActionGenerator
{
  public static AiAction FromStrategy(STRATEGY strat)
  {
    switch (strat) {
      case STRATEGY.DEFEND_AGAINST_BOMBARDMENT:
        return new FireBombardmentDefensive();
      case STRATEGY.SCOUT_WITH_BOMBS:
        return new FireBombardmentScouting();
      default:
        return null;
    }
  }
}
