namespace Tanks.Bases {
    using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class TeamBaseView : MonoBehaviour {
        private new SpriteRenderer renderer;

        private void Awake() {
            renderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(Color color) {
            renderer.color = color;
        }
    }
}