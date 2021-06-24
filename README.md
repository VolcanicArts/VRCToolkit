# VRCToolkit
A collection of tools designed to aid in the development of VRC avatar and world content.
Simply install the latest unitypackage in releases section of this repository, and a window will appear at the top of Unity that says VRCToolkit.
From there you can pick the windows you want to open.

## VRCPackageManager
The VRCPackageManager is a collection of commonly used tools and addons, as well as the latest SDKs, which can be installed with a press of a button. This means no more having to go to the VRC website to find the latest SDK.

## GenericUdonScripts
This is a collection of easy to use scripts that can be modified and used to quickly setup objects in a world before branching out into more complicated behaviours. After installing VRCToolkit, Unity will complain that the SDK and UdonSharp aren't installed, so install those via the VRCPackageManager. After UdonSharp has compiled all the scripts, they should work out the box.
The event senders always send events locally so that the updaters can handle the syncing.

### Example
Place the GenericEventSenderInteract on an object with an animator. This will make it interactable. Now create an empty inside the object and place a BooleanAnimatorUpdater script on it. This is the action controller. Then link the GenericEventSenderInteract to the BooleanAnimatorUpdater, and the BooleanAnimatorUpdater to the animator on your object. Next, tell the BooleanAnimatorUpdater what value you want it to update, and the initial state of that value. Finally, decide what you want the GenericEventSenderInteract to send on interact. Since this is only controlling a single object, we can write "Toggle" in the box. This will tell the BooleanAnimatorUpdater to toggle the value each time it receives that event. The BooleanAnimatorUpdater will then handle the ownership and syncing for you.

### Note
The IntegerAnimatorUpdater comes with 4 values out of the box. Feel free to clone the Value(Number) methods as many times as you want to create more values.
