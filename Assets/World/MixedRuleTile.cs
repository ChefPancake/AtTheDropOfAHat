using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DropOfAHat.World {
    [CreateAssetMenu]
    public class MixedRuleTile : RuleTile {
        [SerializeField]
        private TileBase[] _othertiles;

        public override bool RuleMatch(int neighbor, TileBase other) {
            return neighbor switch {
                TilingRule.Neighbor.This =>
                    other == this
                    || _othertiles.Contains(other),
                TilingRule.Neighbor.NotThis =>
                    other != this
                    && !_othertiles.Contains(other),
                var x => throw new InvalidOperationException($"Neighbor value of {x} not supported")
            };
        }
    }
}
