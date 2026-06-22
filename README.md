# GDPS Maker

A desktop utility for repackaging Geometry Dash clients with custom GDPS server URLs.

<img width="917" height="677" alt="image" src="https://github.com/user-attachments/assets/17dacf0e-074b-47b1-a5f0-a74afd3f0929" />

## Features

- Repackages Windows, iOS, and Android Geometry Dash binaries
- Drag and drop source files
- Ability to assign a bundle ID, application name, and automatic keystore generation
- Automatic keystore generation for Android builds
- Built-in log viewer

## Requirements

- Windows 10 or later (64-bit)
- Java (only required for Android APK builds)

## Installation

Download `GDPS Maker.exe` from the releases page. The application is fully self-contained and does not require a separate .NET installation.

## Usage

1. Launch `GDPS Maker.exe`
2. Enter your GDPS server URL
3. Drag a Geometry Dash binary into the source field or click Browse
4. Click BUILD GDPS CLIENT
5. The repackaged client will appear next to the source file

## Folder Layout

The `apk` folder must be placed next to the executable. It contains:
- `tool.jar` (apktool)
- `signer.jar` (uber-apk-signer)

## Build from Source

Requires .NET 6 SDK.

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
```

The output will be in `bin/Release/net6.0-windows/win-x64/publish/`.

## License

Open source.

---

# GDPS Maker (RU)

Десктопная утилита для пересборки клиентов Geometry Dash с собственным адресом GDPS сервера.

## Возможности

- Пересборка бинарников Windows, iOS и Android
- Поддержка drag and drop
- Возможность назначить bundle ID, имя приложения и автоматическая генерация keystore
- Автоматическая генерация keystore для Android
- Встроенный лог сборки

## Требования

- Windows 10 или новее (64-бит)
- Java (только для сборки Android APK)

## Установка

Скачайте `GDPS Maker.exe` из раздела релизов. Приложение полностью автономное и не требует отдельной установки .NET.

## Использование

1. Запустите `GDPS Maker.exe`
2. Введите адрес вашего GDPS сервера
3. Перетащите бинарник Geometry Dash в поле источника или нажмите Browse
4. Нажмите BUILD GDPS CLIENT
5. Готовый клиент появится рядом с исходным файлом

## Структура папок

Папка `apk` должна находиться рядом с исполняемым файлом. Внутри:
- `tool.jar` (apktool)
- `signer.jar` (uber-apk-signer)

## Сборка из исходников

Требуется .NET 6 SDK.

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
```

Результат будет в `bin/Release/net6.0-windows/win-x64/publish/`.

## Лицензия

Открытый исходный код.
