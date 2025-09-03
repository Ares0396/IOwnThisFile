# IOwnThisFile

A lightweight Windows application for protecting files with **AES-GCM encryption** and an **IO-Lock** mechanism.  
Self-contained: just download, run, and protect your files.

---

[![Latest Release](https://img.shields.io/github/v/release/Ares0396/IOwnThisFile?label=Latest%20Release)](https://github.com/Ares0396/IOwnThisFile/releases)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL-v3-blue.svg)](LICENSE)

---

## 🆕 Latest Release

- **Version:** v1.1.0
- **Published:** September 3, 2025  
- **Download:** [Release page](https://github.com/Ares0396/IOwnThisFile/releases)  

> No installation required beyond running the executable. Just double-click to start.

---

## 🔒 Features

- File encryption & decryption using **AES-GCM**  
- **Self-contained**: works on Windows 10+ with no extra dependencies  
- Easy-to-use GUI  
- Safe: does **not** collect or transmit any user data  

---

## 📄 Changelog

- Added a new feature: Check for updates.
   + The application will now check for updates on startup by default and notify the user if a new version is available.
   + Users can choose to download the latest version directly from the notification.
   + Users can also choose not to receive any more update notifications in the settings.
   + While updating, users can cancel the update.
- Added a new feature: Setting & Preference.
   + Save settings to registry or file (default is registry)
   + White/black theme (default is white)
   + Auto-check for updates on startup (default is enabled)
- Improved byte unit handling: Everyone can see the file size in KB, MB, GB, etc, without having to peer their eyes at a large number in "byte" unit.
- Enhanced cryptographic operations: Multi-thread operations are now supported, meaning that your files are protected more quickly.
- Fixed a decryption bug: The app now does not accept AES-GCM-encrypted files that were not previously encrypted by the app itself

---

## 🚀 How to Use

1. Download the latest release executable.  
2. Double-click to launch the application.  
3. Select a file to encrypt or decrypt.  
4. Enter your password/key.  
5. Click **Encrypt** or **Decrypt**.  
6. Manage your protected files in **File Management** tab.

> ⚠️ Always test with **dummy files** first to confirm everything works as expected.

---

## 🛡️ Privacy & Security

- This app **does not connect to the internet**.  
- It **does not collect or transmit** any personal data.  
- All operations happen locally on your machine.  

---

## 🛠 Development Notes

- Written in **C#** with **WinForms**
- Uses **.NET 8**
- Encryption: **AES-GCM**  
- Executable is **self-contained**, no extra installations needed
- Source code available on Github

---

## 📜 License

Licensed under the [GPL-3.0 License](LICENSE).

---

## 🙌 Contributing

Pull requests are welcome! Open an issue first for major changes.
