using UnityEngine;
using System.Collections;

public class BattleResult {
    public enum ResultType {
        Win,
        Lose,
        Draw,
    }

    public ResultType Type { get; private set; }

    public BattleResult(ResultType type) {
        this.Type = type;
    }
}
