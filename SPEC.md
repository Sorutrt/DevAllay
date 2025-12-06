# 仕様書: DevAllay

## 1. プロジェクト概要
本プロジェクトは、Minecraft Bedrock Edition (Windows版) の開発・管理支援ツールです。
**「既存のワールドデータの管理」** および **「座標計算などのユーティリティ」** に特化した Windows GUI アプリケーションを構築します。

## 2. 技術スタック
- **ターゲットフレームワーク:** .NET 9.0 (Windows)
- **言語:** C# 13
- **GUIフレームワーク:** Windows Forms (WinForms)
- **依存:** 標準ライブラリのみ（System.Text.Json 等は必要に応じて使用）

### 2.1. .NET 9.0 SDK インストール手順
1. [.NET 9.0 SDK ダウンロードページ](https://dotnet.microsoft.com/download/dotnet/9.0) にアクセス
2. Windows x64 用の SDK インストーラーをダウンロード
3. インストーラーを実行し、指示に従ってインストール
4. ターミナルで `dotnet --version` を実行し、`9.0.x` が表示されることを確認

## 3. 開発環境・運用方針
- **IDE非依存:** 特定のIDE（Visual Studio, VS Code, Windsurf等）に依存しない設計とする。
- **ビルドシステム:** 標準の `.NET CLI` (`dotnet build`, `dotnet run`) で完全に動作すること。
- **UI実装:** **コードベースUI (Code-only UI)**。`.Designer.cs` は使用しない。

## 4. 機能仕様

### 4.1. 機能A: ワールドデータ管理 (World Manager)
Minecraft BE のローカル保存データを操作する機能です。

1.  **ワールド一覧表示:**
    - 以下のパスをスキャンする：
      - Bedrock Edition: `%LOCALAPPDATA%\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds`
      - Education Edition: `%LOCALAPPDATA%\Packages\Microsoft.MinecraftEducationEdition_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds`
    - 各フォルダ内の `levelname.txt` を読み込み、ワールド名をリスト表示する（フォルダ名ではなく、ゲーム内表示名で表示すること）。
    - ワールド名の後にエディション名（[Bedrock] または [Education]）を表示する。
    - *(将来実装)* 最終更新日時などでソートできると望ましい。

2.  **外部ツール連携 (Launcher):**
    - リストで選択したワールドに対し、以下のボタン操作を提供する：
        - **Open Explorer:** エクスプローラーでそのフォルダを開く。
        - **Open Terminal:** その場所で Windows Terminal (`wt.exe`) を開く。
        - **Open VS Code:** その場所で VS Code (`code .`) を開く。

3.  **アイコン変更機能:**
    - 外部からの画像ファイル（.png, .jpg）を、アプリ内のワールドリストへドラッグ＆ドロップすることで、そのワールドの `world_icon.jpeg` を上書き更新する機能。
    - 画像は **800 x 450** ピクセルにリサイズして保存する。
    - アスペクト比が異なる場合は、長い辺を基準に縮小し、中央配置で余白なくクロップする。
    - 確認ダイアログ・バックアップは不要。

### 4.2. 機能B: 座標変換ツール (XYZ Converter)
チャット欄やコマンドログからコピーした座標文字列を、セレクター引数形式に変換する機能です。

1.  **UI:**
    - 入力用テキストボックス（ペースト用）。
    - 結果表示用ラベルまたはテキストボックス。
    - テキスト入力時に自動で変換を実行する（ボタン不要）。

2.  **変換ロジック:**
    - 入力文字列から `/` (スラッシュ) で始まるコマンド部分を除去する。
    - スペース区切りの数値をパースする。
    - **ケース1（3つの座標）:** `x, y, z` の場合
        - 出力: `x={x},y={y},z={z}`
    - **ケース2（6つの座標）:** `x1, y1, z1, x2, y2, z2` の場合
        - 2点間の範囲（Volume）指定とみなす。
        - X, Y, Z それぞれについて、小さい値を始点、大きい値を終点として並べ替える。
        - 差分（dx, dy, dz）を計算する。
        - 出力: `x={minX},y={minY},z={minZ},dx={diffX},dy={diffY},dz={diffZ}`
    - **エラー処理:** 数値パース失敗時や、要素数が合わない場合はエラーメッセージを表示。

3.  **出力:**
    - 変換成功時、結果文字列を自動的にクリップボードへコピーする。

## 5. コード構造戦略 (Code Structure)
- **`MainForm.cs`**:
    - `TabControl` を使用し、「World Manager」タブと「XYZ Converter」タブに画面を分ける。
- **`Features/WorldManager/`**:
    - `WorldScanner.cs`: ワールドフォルダの探索と `levelname.txt` の読み込みロジック。
    - `IconUpdater.cs`: ファイルドロップ時の処理ロジック（リサイズ・保存）。
- **`Features/XyzConverter/`**:
    - `CoordinateParser.cs`: 座標変換ロジック（Split, Sort, Calc）を実装したクラス。
- **`Utils/`**:
    - `AppLauncher.cs`: エクスプローラーやターミナルを起動するヘルパー。

## 6. AIへの実装指示ガイド
AIエージェントは以下の順序で実装を行ってください:
1.  **基盤作成**: プロジェクト作成、`.gitignore`、`global.json` の配置。
2.  **UI骨子**: `MainForm` に `TabControl` を配置し、2つのタブを作る。
3.  **XYZ実装**: 比較的使用ロジックが明確な「XYZ Converter」から実装する（ロジック移植 -> UI結合）。
4.  **World Manager実装**: パス取得ロジック -> 一覧表示 -> 「開く」ボタン -> D&D機能 の順で実装する。
