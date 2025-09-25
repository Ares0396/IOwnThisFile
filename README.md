# IOwnThisFile

A lightweight Windows application for protecting files with **AES-GCM encryption** and an **IO-Lock** mechanism.  
Self-contained: just download, run, and protect your files.

---

[![Latest Release](https://img.shields.io/github/v/release/Ares0396/IOwnThisFile?label=Latest%20Release)](https://github.com/Ares0396/IOwnThisFile/releases)
[![License: GPL-3.0](https://img.shields.io/badge/License-GPL-v3-blue.svg)](LICENSE)

---

## 🆕 Latest Release

- **Version:** v1.2.1
- **Published:** September 25, 2025  
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

- Fixed the UI:
   + The app now does not crash when the user IO-Locked, IO-Unlocked and IO-Locked a file again.
   + The selected items for IO-Lock operations now stay fixed instead of being moved to the bottom of the list.
- Fixed a bug in the Debugger: It now also catches exceptions from the WinForms.
- Improved cryptographic operation: Memory is instantly returned to the OS instead of being piled up (with a bit of small overhead).

---

## 🚀 How to Use

1. Download the latest release executable.  
2. Double-click to launch the application.  
3. Select a file to encrypt or decrypt.  
4. Enter your password/key.  
5. Click **Encrypt** or **Decrypt**.  
6. Manage your protected files in **File Management** tab.

> ⚠️ Always test with **dummy files** first to confirm everything works as expected. This project is written and maintained by a **9th grader**. Data corruption is **possible**.

---

## 🛡️ Privacy & Security

- This app only connects to the Internet to **check for newer updates**.
- It **does not collect or transmit** any personal data.  
- All operations happen locally on your machine apart from the **Check For Update** feature

---

## 🛠 Development Notes

- I, as the author of this project, may have to discontinue this project due to academic pressure.
- For the time being, however, I'll try my best to update and maintain this project.
- Good day to you! Written by a Vietnamese 9th grader, tested by a British friend.

---

## 📜 License

Licensed under the [GPL-3.0 License](LICENSE).

---

## 🙌 Contributing

Pull requests are welcome! Open an issue first for major changes.
