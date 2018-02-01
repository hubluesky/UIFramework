namespace VBM {
    [System.Serializable]
    public class RewardTypeToSpriteConverter : PropertyConverter {
        public override object Convert(object value) {
            RewardType type = (RewardType) value;
            switch (type) {
                case RewardType.Ore:
                    return Main.iconsSettings.ore;
                case RewardType.Wood:
                    return Main.iconsSettings.wood;
                case RewardType.Food:
                    return Main.iconsSettings.food;
                case RewardType.God:
                    return Main.iconsSettings.god;
                case RewardType.Refine:
                    return Main.iconsSettings.refine;
                case RewardType.Forge:
                    return Main.iconsSettings.forge;
                default:
                    return null;
            }
        }
    }
}