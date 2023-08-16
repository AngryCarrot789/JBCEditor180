namespace BCEdit180.Core.Utils {
    public readonly struct Rect {
        public float X1 { get; }
        public float Y1 { get; }
        public float Width { get; }
        public float Height { get; }

        public float X2 => this.X1 + this.Width;

        public float Y2 => this.Y1 + this.Height;

        public Rect(float x1, float y1, float width, float height) {
            this.X1 = x1;
            this.Y1 = y1;
            this.Width = width;
            this.Height = height;
        }

        public static Rect FromAABB(float x1, float y1, float x2, float y2) {
            return new Rect(x1, y1, x2 - x1, y2 - y1);
        }
    }
}