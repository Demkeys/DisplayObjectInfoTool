# Display Object Info Tool
Display info (Name,Tag, Layer,Position,Rotation,Scale,etc.) of all active GameObjects in the scene.

![alt text](https://github.com/Demkeys/DisplayObjectInfoTool/blob/master/DisplayObjectInfo%20screenshot.png "Display Object Info")

## **Features**
* Show Info - You can choose multiple options between Name, Tag, Layer, Position (Local/Global), Rotation (Local/Global), Scale (Local/Global), Magnitude and Parent Connection
* All text and line colors customizable
* Text size customizable
* Text space customizable
* Text position offset customizable
* Data Persistence - Data is saved whenever OnDisable() is called, this includes closing the Editor Window, and even maximizing any other Editor Window. So if the window is closed, the data will be saved, and the next time it is opened again, the saved data will be loaded.
* Restore Tool To Default - Tool settings can be restored to default.

## **Instructions**
1. Import *DisplayObjectInfo.cs* script into project.
2. Place *DisplayObjectInfo.cs* script in Editor folder.
3. Click **My Tools -> Display Object Info** to open **DisplayObjectInfo** window.
4. Dock window next to any existing tabs. Docking next to Inspector is recommended.

## **Notes**
* Due to some UnityEditor API being deprecated over time, there are two versions of the tool script:
  - Use [DisplayObjectInfoTool.cs](https://github.com/Demkeys/DisplayObjectInfoTool/blob/master/DisplayObjectInfo.cs) for Unity versions 2019 and above (that incldes all Unity versions in the 2019 cycle).
  - Use [DisplayObjectInfoToolDeprecated.cs](https://github.com/Demkeys/DisplayObjectInfoTool/blob/master/DisplayObjectInfoDeprecated.cs) for Unity versions before the 2019 cycle.
* **DisplayObjectInfo** window has to be open (not necessarily in focus) for tool to work.
