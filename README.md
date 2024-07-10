# Color Converter API

An SPT module mod that extends the functionality of the client, allowing the server to use raw hexcode color values for fields that ordinarily can only support BSG-created color enums.

This mod should be forward-compatible with all new client releases for EFT (unless something very, very significant changes) and does NOT need to be updated.

### How to install

1. Download the latest release here: [link](https://github.com/RaiRaiTheRaichu/ColorConverterAPI/releases) -OR- build from source (instructions below)
2. Simply extract the zip file contents into your root SPT folder (where EscapeFromTarkov.exe is).
3. Your `BepInEx/plugins` folder should now contain a `RaiRai.ColorConverterAPI.dll` file inside.

### Known issues

None at the moment.

### Usage (for modders)

You can check to see if the plugin exists in your server mod's source code any way you'd like, but an easy copy-paste check is here:
```
private static IsPluginLoaded(): boolean 
{
    const fs = require('fs');
    const pluginName = "rairai.colorconverterapi.dll";
        // Fails if there's no ./BepInEx/plugins/ folder
    try 
    {
        const pluginList = fs.readdirSync("./BepInEx/plugins").map(plugin => plugin.toLowerCase());
        return pluginList.includes(pluginName);
    }
    catch 
    {
        return false;
    }
}
```

You can handle the result of this function however you'd like.
Once you have validated that the plugin exists, you can proceed to fill in the fields for color in your item database with hexcodes in lieu of a color enum. For example:
`"TracerColor": "#FF00FF",`
`"BackgroundColor": "#2A2AFF",`

Short 3-byte hexcode colors are also supported like so:
`"BackgroundColor": "#F90",`
Which will be interpreted as:
`"BackgroundColor": "#FF9900",`

As of version 1.1.0, you can optionally pass a fourth hex value for an Alpha (otherwise, it is treated as FF/255).
`"BackgroundColor": "#FF2AFF99",`

### How to build from source

1. Download/clone this repository.
2. Open your current SPT directory and copy all files required under the "Reference list" section to their respective folders.
3. Rebuild the project in the Release configuration.
4. Grab the `RaiRai.ColorConverterAPI.dll` file from the `build/plugins/` folder and use it wherever. Refer to the "How to install" section if you need help here.

### Reference list

Copy to this project's folder (create if it does not exist) `references/EFT/Managed`:
- Assembly-CSharp.dll
- Newtonsoft.Json.dll
- UnityEngine.dll
- UnityEngine.CoreModule.dll

Copy to this project's folder (create if it does not exist) `references/Bepinex`:
- spt-reflection.dll

These can be found in your SPT install's `EscapeFromTarkov_Data/Managed/` folder and your `BepInEx/Plugins/spt/` folder.

### Credits
RaiRaiTheRaichu
Terkoiz
