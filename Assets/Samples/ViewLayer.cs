using VBM;

[PropertyToEnumAttribute(typeof(ViewConfig), "layer")]
public enum ViewLayer {
    BackgroundLayer,
    ForegroundLayer,
    NormalLayer,
    PopupLayer,
    TopLayer,
    MostTopLayer,
}