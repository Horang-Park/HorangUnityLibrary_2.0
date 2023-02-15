# Primary depended Unity packages
1. UniRx: https://github.com/neuecc/UniRx (recommend install package via UMP)
2. UniTask: https://github.com/Cysharp/UniTask (recommend install packge via UPM)
3. Newtonsoft Json: com.unity.nuget.newtonsoft-json (recommend install package via UPM)

# Features
1. Module system like a stacking block.
   1. Audio Module
   2. Asset Bundle Patch Module
   3. Camera Module
   4. External Application Launch Module
   5. Localization Module
   6. Stopwatch Module
2. Managers
   1. DeeplinkManager
   2. ModuleManager
   3. NumberImageFontManager
   4. ObstacleTransparencyManager
   5. RemoteMethodInterfaceManager
   6. RequestManager (API)
   7. UIManager
3. Android native support kind of only can use in Android native.
   1. SendBroadcast
   2. Get device physically SSAID
   3. Get other application version and build version
4. Utilities
   1. Custom attributes
      1. Inspector readonly field
      2. Inspector hide script field
   2. Procedural sequencer
      1. Sync version
      2. Async version
   2. Parser
      1. Json (Using newtonsoft json)
      2. CSV
   3. Compression
   4. Encryption
   5. GetIntentExtraData (Only support Android built application.)
   6. ImageLoader (Remote version is async)
   7. Log (Unity's top menu [Horang/Tools/Debug Mode/Log] can control show log or hide log.)
   8. Permissions (Support Android built application and iOS built application.)
   9. FiniteStateMachine (FSM)
   10. Unity extension
       1. Color
          1. RGBA 256 to Unity color
          2. Unity color to RGBA 256
          3. Web hex color to Unity color
       2. Gizmo
          1. Wire fan shape drawer
   11. Not encrypt operation save and load
       1. Saved file data structure is "key = value".
5. Unity menu
   1. Horang/Tools/Debug Mode/Log: Enable or disable log. (Can save log file when log are disabled.)
   2. Horang/Tools/Transform Randomize: Set random value on selected game object's position, rotation, scale.
   3. Horang/Module/Audio/Create Audio Database: Make audio database for Audio Module. (Generate in Resources/)

# Application build settings
#### Android
   * Please set Android minimum API level is 26 or more later. (Android 8.0 Oreo)

#### iOS & iPadOS
   * None.


# UPM support
Use git url: https://github.com/Horang-Park/HorangUnityLibrary_2.0.git?path=/HorangUnityLibrary_2.0_Package