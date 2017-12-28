# View Binding Model UIFramework - Open Source

## 设计初衷
* 在做游戏开发时，使用过，或者没使用过UI框架的时候，对于处理数据与UI的显示同步问题，总是要做许多处理，比喻说，一个角色模块的用户名改了，那对应的UI就需要立即更新，但如果这个时候这个UI还没创建呢，或者是这个UI还处于隐藏状态，或者其它原因，导致了你的数据更新，UI无法跟着立即更新，那就会在当这个UI要显示（或者创建等时机）时，把这个用户名更新的UI上，所以因为UI不能立即更新的原因，导致需要手动更新数据，增加额外的代码，如果有许多个UI都使用了这个数据，还要把更新代码写得到处都是，不但容易出错，维护也不方便。
* 列表更新，对于这个显示在UI上，还用ScrollView来滚动的长列表，表项的数据更新更是五花八门，有时候为了简单，粗暴的在每次打开UI都给他完全刷一遍，或者记个变量说，上一次有没有数据更新时，我UI因为其它原因没有跟着更新的，来刷一遍。这种简单粗暴也有个问题，要要列表很长呢，数据很多呢，或者是用户需要记住上一次滚动的位置呢。

## 设计理念
* View Binding Mode（原谅我粗暴的起了这个名字，简称VBM）就是从View上面，使用Bind功能，去BindModel，这所以用这种模式是来源于一个同事给我演示的MVVM功能，所以我才写了这个VBM，之所以从View上面来绑定Model，这要感谢Unity强大的编辑器，可以在编辑上拖拖两下，就把功能给做了，所以VBM就是在unity上编辑一个UI的Prefab，然后在这个Prefab上增加`ViewBindingModel`，然后就可以选择Model进行属性绑定，整个绑定过程全编辑器，当然，前提你需要定义一个自己的Model，继承自VBM`Model`，然后添加`Property`属性，这样在`ViewBindingModel`上就可以反射出这个Model的属性，进行绑定了。
* 从View来绑定Model比Model来绑定View有两个好处。



设计思路
1. 数据使用Database
2. 通用UI与战斗数据
3. 通用大厅UI与战斗UI
4. 支持特殊UI，比喻新手引导
5. 使用数据与UI绑定方式，数据更新，UI跟着更新
6. 支持延迟加载，即一个UI可以分多次加载
7. UI要支持排序，数据内容更新，重新加载等所有功能
8. 有一个通用的View，如果有特殊可以继承这个View来做特殊功能
9. 一个Model可以绑定多个View，一个View可以来源于多个Model
10. View在显示时，所有数据实时更新
11. View隐藏后，再次显示，会有Refrese重新刷新所有数据。（只刷新用到的，比喻有多个标签数据，只刷新一个标签数据）
12. 数据刷新以标签内容为单位
13. 有一个ViewList，用来处理列表类的显示数据
14. Model与View的绑定数据结构为树形结构，当View需要Refrash时，会指定树层级结构来刷新

## License

Copyright (c) hubluesky Personal. All rights reserved.

Licensed under the [MIT](LICENSE.txt) License.