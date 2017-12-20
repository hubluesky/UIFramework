namespace VBM {

    public enum Layer {
        BackgroundLayer = 1000 << ViewLayer.LayerIndexBitCount,
        ForegroundLayer = 2000 << ViewLayer.LayerIndexBitCount,
        NormalLayer = 3000 << ViewLayer.LayerIndexBitCount,
        PopupLayer = 4000 << ViewLayer.LayerIndexBitCount,
        TopLayer = 5000 << ViewLayer.LayerIndexBitCount,
        MostTopLayer = 6000 << ViewLayer.LayerIndexBitCount,
    }

    public static class ViewLayer {
        // Layer的高12表示layer层，低20位表示该层的排序，值越小，实际索引值(Transform的子的索引值)就越小
        public const int LayerMask = 0x1F00000;
        public const int LayerIndexMask = 0xFFFFF;
        public const int LayerBitCount = 12;
        public const int LayerIndexBitCount = 20;
    }
}