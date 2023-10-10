using UnityEngine;
using System.Collections;
using NotBuyoTeto.Ingame.MultiPlay;

public class RatingCalculator : MonoBehaviour {
    [SerializeField]
    private readonly float rateBase = 10.0f;
    [SerializeField]
    private readonly float scalingFactor = 800.0f;
    [SerializeField]
    private readonly float ratingFactor = 40.0f;

    public float ExpectedScore(int playerRating, int opponentRating) {
        return 1.0f / (1.0f + Mathf.Pow(rateBase, (opponentRating - playerRating) / scalingFactor));
    }

    public float ActualScore(BattleResult result) {
        if (result.Type == BattleResult.ResultType.Win) { return 1.0f; }
        if (result.Type == BattleResult.ResultType.Lose) { return 0.0f; }
        return 0.5f;
    }

    public int NewRating(int playerRating, int opponentRating, BattleResult result) {
        var expectedScore = ExpectedScore(playerRating, opponentRating);
        var actualScore = ActualScore(result);
        var diffRating = ratingFactor * (actualScore - expectedScore);
        if (diffRating > 0f) {
            return playerRating + Mathf.CeilToInt(diffRating);
        }
        return playerRating + Mathf.FloorToInt(diffRating);
    }
}
