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
2. Android native support kind of only can use in Android native.
   1. SendBroadcast
   2. Get device physically SSAID
   3. Get other application version and build version
3. Utilities.
   1. Custom attributes
      1. Inspector readonly field
      2. Inspector hide script field
   2. Procedural sequencer
      1. Sync version
      2. Async version
   2. Json parser (depends on Newtonsoft Json)
   3. Compression
   4. Encryption
   5. GetIntentExtraData (Only support Android built application.)
   6. ImageLoader (Remote version is async)
   7. Log (Unity's top menu [Horang/Tools/Debug Mode/Log] can control show log or hide log.)
   8. Permissions (Support Android built application and iOS built application.)

# Application build settings
#### Android
   * Please set Android minimum API level is 26 or more later. (Android 8.0 Oreo)

#### iOS & iPadOS
   * None.


# UPM support
Use git url: https://github.com/Horang-Park/HorangUnityLibrary_2.0.git?path=/HorangUnityLibrary_2.0_Package