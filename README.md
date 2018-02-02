# View Binding Model UIFramework - Open Source

中文说明请移步[这里](ChineseReadMe.md)

* The following English description is translated by Google, to bring you inconvenience, please forgive my pool english.
## Design
* View Binding Mode (forgive me rough name, VBM) is from the View above, use the Bind function, to BindModel, so using this model is derived from a colleague to demonstrate MVVM function, so I Write this VBM, the reason to bind Model from the View above, thanks to the Unity powerful editor, you can drag in the editor, put the function to do, so VBM is unity in the editor of a UI Prefab, and then add this Prefab `ViewBindingModel`, and then you can select the Model for property binding, the entire binding process full editor, of course, the premise you need to define a own Model, inherited from VBM`Model`, and then add `Property` property, so that the` ViewBindingModel` can reflect the properties of this Model and bind it.
* Binding a Model to a View from a Model to bind the View has the following benefits.
> 1. Never change the name and hierarchy of the UI anymore, as no error will be made as long as the Unity reference is still there.
> 2. Property binding Do not write the code, you can use the editor to edit, showing the binding, but also clear.
> 3. Do not worry about the UI is deleted, re-create with the Model property rebinding things

## Features
1. UI and data binding in the editor, as long as the data is updated, the UI can be updated
2. Support a Model can be bound by multiple UI
View object can hold the first, the UI can be created only when needed
4. Support UI (or UI inside a tab content) and need to display the latest data
5. Support List function, and supports Add, Remove, Insert, AddRange, Swap, Sort, Clear and other operations
6. Canvas uses a layered design, showing hidden rules between different layers will not affect each other
7. View and Model have the appropriate Manager management, life cycle automation
8. UI resources and display level, show hidden rules have a unified configuration
9. Support UI control event callback Model

## How to get started
1. Two examples of project, one is Old "OldExamples", which is a simple example, because it is simple, it is well understood, it is also very good for testing, but also contains examples of the use of [XLua](https://github.com/Tencent/xLua).
    * Open Scene inside OldExamples folder, run the scene, first click the MBV button, which is to do initialization, and then you can choose a random Show button to display the corresponding UI.
2. New Examples, this is my free resources in the upper and lower of the Unity AssetStore, and then integrated into the project, from login, to the hall, and then there are achievements, such as shopping mall UI, suitable for understanding the principles of this project example, of course Can also look at the effect of simple.
    * Open file Examples/Scenes/MainScene, run the scene, enter any account password you can log into the hall.
3. If you have any suggestions or questions, please feel free to leave me a message (https://github.com/hubluesky/UIFramework/issues)

## Feedback
On the issue of feedback, Git [issue](https://github.com/hubluesky/UIFramework/issues)

## License

Copyright (c) hubluesky Personal. All rights reserved.

Licensed under the [MIT](LICENSE.txt) License.