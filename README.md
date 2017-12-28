# View Binding Model UIFramework - Open Source

## 设计初衷
* 在做游戏开发时，使用过，或者没使用过UI框架的时候，对于处理数据与UI的显示同步问题，总是要做许多处理，比喻说，一个角色模块的用户名改了，那对应的UI就需要立即更新，但如果这个时候这个UI还没创建呢，或者是这个UI还处于隐藏状态，或者其它原因，导致了你的数据更新，UI无法跟着立即更新，那就会在当这个UI要显示（或者创建等时机）时，把这个用户名更新的UI上，所以因为UI不能立即更新的原因，导致需要手动更新数据，增加额外的代码，如果有许多个UI都使用了这个数据，还要把更新代码写得到处都是，不但容易出错，维护也不方便。
* 列表更新，对于这个显示在UI上，还用ScrollView来滚动的长列表，表项的数据更新更是五花八门，有时候为了简单，粗暴的在每次打开UI都给他完全刷一遍，或者记个变量说，上一次有没有数据更新时，我UI因为其它原因没有跟着更新的，来刷一遍。这种简单粗暴也有个问题，要要列表很长呢，数据很多呢，或者是用户需要记住上一次滚动的位置呢。

## 设计理念
* View Binding Mode（原谅我粗暴的起了这个名字，简称VBM）就是从View上面，使用Bind功能，去BindModel，这所以用这种模式是来源于一个同事给我演示的MVVM功能，所以我才写了这个VBM，之所以从View上面来绑定Model，这要感谢Unity强大的编辑器，可以在编辑上拖拖两下，就把功能给做了，所以VBM就是在unity上编辑一个UI的Prefab，然后在这个Prefab上增加`ViewBindingModel`，然后就可以选择Model进行属性绑定，整个绑定过程全编辑器，当然，前提你需要定义一个自己的Model，继承自VBM`Model`，然后添加`Property`属性，这样在`ViewBindingModel`上就可以反射出这个Model的属性，进行绑定了。
* 从View来绑定Model比Model来绑定View有以下的好处。
> 1. 再也不用怕UI改名字和改层级结构了，因为只要Unity的引用还在，就不会出错。
> 2. 属性绑定不用再写代码了，使用编辑器就可以编辑了，可示化了绑定，也清晰明了。
> 3. 不用再操心UI被删除后，重新创建跟Model属性重新绑定的事了

## 功能特性
1. UI与数据在编辑器上绑定以后，只要数据有更新，UI都能得到更新
2. 支持一个Model可以被多个UI所绑定
3. 可以先持有View对象，UI可以在需要时才被创建
4. 支持UI（或者UI里面的某一个标签页内容）与在需要显示时才获得最新数据
5. 支持List功能，并且支持Add、Remove、Insert、AddRange、Swap、Sort、Clear等操作
6. Canvas使用分层设计，不同层之间有显示隐藏规则不会互相影响
7. 对于View与Model都有相应的Manager管理，生命周期自动化
8. UI资源与显示层次，显示隐藏规则都有一个统一的配置
9. 支持UI控件的事件回调Model

## 文档说明
doc文档还没开始写，写完会以pdf形式放到工程上，示例场景已经有了，就在项目的Samples下面，Scripts下是源码，Samples的场景上是几个按钮。如果不看代码要想直接跑的话，请看以下说明：
> 1. 点击开始运行场景后，会有一排按钮，要先点VBM按钮，这是做资源配置的加载和Model，View对象的创建。
> 2. 点击ShowRoleInfo，显示与自动加载RoleInfo资源，这时候会看到一个UI，但上面数据显示是空的，因为Model的数据就是空的
> 3. 点击UpdateRoleInfo（或者UpdateRoleInfo2）更新RoleInfo Model的数据，然后就可以看到数据已经被更新到UI上来了，
> 4. Ranking和RoleInfo都是同理的，RoleInfo是演示数据的绑定更新，和延迟更新（即UI需要显示才更新），Ranking是演示List的绑定更新和Sort功能

## 反馈
关于问题反馈，可以在Git上提issues，或者是通过邮件联系我（很抱歉，邮件很少使用）

## License
抄vccode的，请将就着看

Copyright (c) hubluesky Personal. All rights reserved.

Licensed under the [MIT](LICENSE.txt) License.