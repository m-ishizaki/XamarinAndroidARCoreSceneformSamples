# XamarinAndroidARCoreSceneformSamples

Xamarin.Android の ARCore サンプルです。  
ARCore の Sceneform のサンプルプロジェクト ( https://github.com/google-ar/sceneform-android-sdk/tree/master/samples ) を Xamarin.Android 用に書き換えて動作させようとしています。  

#### Sceneform サンプル
このプロジェクトは Apache 2.0 License のサンプルプルジェクトのコードを C# に書き換えることで作成しています。  

* Sceneform サンプル ( https://developers.google.com/ar/develop/java/sceneform/ )
* ライセンス ( https://github.com/google-ar/sceneform-android-sdk/blob/master/LICENSE )

## 実ファイルの無いファイル群

**Sceneform のライブラリは NuGet パッケージをインストールするようになりました。 *.aar のコピーや各バインディングライブラリのビルドは不要です。**  

~~classes.jar ファイルは次のアドレスでダウンロードした .aar を .zip にリネームして展開した中の .jar~~
~~https://dl.google.com/dl/android/maven2/com/google/ar/core/1.9.0/core-1.9.0.aar~~  
  
~~各 .aar ファイルは次のアドレスでダウンロード~~  
~~https://dl.google.com/dl/android/maven2/com/google/ar/sceneform/core/1.9.0/core-1.9.0.aar~~  
~~https://dl.google.com/dl/android/maven2/com/google/ar/sceneform/filament-android/1.9.0/filament-android-1.9.0.aar~~  
~~https://dl.google.com/dl/android/maven2/com/google/ar/sceneform/rendering/1.9.0/rendering-1.9.0.aar~~  
~~https://dl.google.com/dl/android/maven2/com/google/ar/sceneform/sceneform-base/1.9.0/sceneform-base-1.9.0.aar~~  
~~https://dl.google.com/dl/android/maven2/com/google/ar/sceneform/ux/sceneform-ux/1.9.0/sceneform-ux-1.9.0.aar~~  

*.sfb ファイルは Android Studio で動かしたプロジェクトの中からコピーします。  
Sceneform ライブラリのバージョンが変わると生成される *.sfb ファイルが変わるようで、別バージョンのものでは動かないことがあります。Android Studio で 1.11.0 で動かして生まれたものをコピーしてください。  








