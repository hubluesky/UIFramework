namespace VBM {
    public enum ViewShowRule {
        None, // 显示时不作处理
        HideSameLayerView, // 隐藏相同层其它UI
        HideLowLayerView, // 隐藏低于等于自己层其它UI
    }

    public enum ViewHideRule {
        None, // 被隐藏时不作处理
        SaveToStack, // 被隐藏时保存到UI栈上，当上层UI被隐藏后，会再次从栈上恢复显示
        DestroyAsset, // 隐藏时销毁资源
    }
}