# VRCToolkit
A collection of tools designed to aid in the development of VRC avatar and world content.

# Install Instructions
If you're wanting to use the VRCPackageManager on its own, install that first and then install anything else you want from it.
If you're wanting to use the VRCPackageManager and UdonBehaviours, install VRCPackageManager, then SDK3World, then UdonSharp, then UdonBehaviours.

# Editor
## VRCPackageManager
The VRCPackageManager is a collection of commonly used tools and addons, as well as the latest SDKs, which can be installed with a press of a button. This means no more having to go to the VRC website to find the latest SDK.

# Udon
## UdonBehaviours
This is a collection of easy to use scripts that can be modified and used to quickly setup objects in a world before branching out into more complicated behaviours. After installing VRCPackageManager, install the world SDK3 and UdonSharp. Next, install the UdonBehaviours and after UdonSharp has compiled all the scripts they should work out the box.
The event senders always send events locally so that the updaters can handle the syncing.

### Note
Anything that uses values, such as the IntegerAnimaterController, comes with 4 values out of the box. Feel free to clone the Value(Number) methods as many times as you want to create more values. I plan on creating a system to get around this in the future.
