# DevAllay

Minecraft Bedrock Edition (Windows版) の開発・管理支援ツール

## 概要

DevAllayは、Minecraft Bedrock Editionのワールドデータ管理と座標変換ユーティリティを提供するWindows GUIアプリケーションです。

## 機能

### 1. World Manager（ワールドマネージャー）

- **ワールド一覧表示**: ローカルに保存されているワールドを自動スキャンして表示
- **外部ツール連携**:
  - 📁 **Open Explorer**: ワールドフォルダをエクスプローラーで開く
  - ⌨️ **Open Terminal**: Windows Terminalでワールドフォルダを開く
  - 💻 **Open VS Code**: VS Codeでワールドフォルダを開く
- **アイコン変更**: 画像ファイル(.png, .jpg)をドラッグ&ドロップしてワールドアイコン(`world_icon.jpeg`)を更新
  - 自動的に800x450ピクセルにリサイズ
  - アスペクト比が異なる場合は中央クロップ

### 2. XYZ Converter（座標変換ツール）

チャット欄やコマンドログからコピーした座標を、セレクター引数形式に自動変換します。

**対応フォーマット**:
- **3つの座標** (`x y z`): `x={x},y={y},z={z}` に変換
- **6つの座標** (`x1 y1 z1 x2 y2 z2`): `x={minX},y={minY},z={minZ},dx={dx},dy={dy},dz={dz}` に変換
- コマンドプレフィックス (`/tp` など) は自動除去
- 変換結果は自動的にクリップボードにコピー

## 技術スタック

- **.NET 9.0** (Windows)
- **C# 13**
- **Windows Forms** (コードベースUI)

## 必要環境

- Windows 10/11
- .NET 9.0 SDK
- Minecraft Bedrock Edition (Windows版)

## インストール

### 1. .NET 9.0 SDK のインストール

1. [.NET 9.0 SDK ダウンロードページ](https://dotnet.microsoft.com/download/dotnet/9.0) にアクセス
2. Windows x64 用の SDK インストーラーをダウンロード
3. インストーラーを実行
4. ターミナルで確認:
   ```powershell
   dotnet --version
   # 9.0.x が表示されればOK
   ```

### 2. アプリケーションのビルド

```powershell
cd DevAllay
dotnet build
```

### 3. アプリケーションの実行

```powershell
dotnet run
```

または、ビルド済みの実行ファイルを使用:

```powershell
.\bin\Debug\net9.0-windows\DevAllay.exe
```

## プロジェクト構造

```
DevAllay/
├── MainForm.cs                          # メインフォーム
├── Program.cs                           # エントリーポイント
├── Features/
│   ├── WorldManager/
│   │   ├── WorldInfo.cs                 # ワールド情報モデル
│   │   ├── WorldScanner.cs              # ワールドスキャナー
│   │   ├── IconUpdater.cs               # アイコン更新処理
│   │   └── WorldManagerControl.cs       # World Manager UI
│   └── XyzConverter/
│       ├── CoordinateParser.cs          # 座標変換ロジック
│       └── XyzConverterControl.cs       # XYZ Converter UI
└── Utils/
    └── AppLauncher.cs                   # 外部アプリ起動ヘルパー
```

## 使い方

### World Manager

1. アプリケーションを起動
2. 「World Manager」タブを選択
3. ワールド一覧から操作したいワールドを選択
4. ボタンをクリックして外部ツールを起動
5. 画像ファイルをワールド名にドラッグ&ドロップしてアイコンを変更

### XYZ Converter

1. 「XYZ Converter」タブを選択
2. 座標をテキストボックスに入力またはペースト
3. 自動的に変換され、クリップボードにコピーされます

**入力例**:
```
100 64 200
/tp 100 64 200
100 64 200 150 80 250
```

## ライセンス

このプロジェクトはMITライセンスの下で公開されています。

## 開発者向け

### コードスタイル

- **IDE非依存**: `.Designer.cs` を使用しないコードベースUI
- **標準.NET CLI**: `dotnet build`, `dotnet run` で完全動作
- **Nullable有効**: C# 9.0+ の null 許容参照型を使用

### ビルドコマンド

```powershell
# ビルド
dotnet build

# 実行
dotnet run

# リリースビルド
dotnet build -c Release

# 発行（単一ファイル実行可能ファイル）
dotnet publish -c Release -r win-x64 --self-contained
```

## トラブルシューティング

### ワールドが表示されない

- Minecraft Bedrock Editionがインストールされているか確認
- パス: `%LOCALAPPDATA%\Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang\minecraftWorlds`

### Windows Terminalが起動しない

- Windows Terminalがインストールされているか確認
- Microsoft Storeから「Windows Terminal」をインストール

### VS Codeが起動しない

- VS Codeがインストールされ、PATHに追加されているか確認
- コマンドプロンプトで `code --version` が動作するか確認
