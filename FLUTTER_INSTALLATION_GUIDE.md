# Flutter Installation Guide for Windows

Complete step-by-step guide to install Flutter on Windows.

---

## 📋 Prerequisites

Before installing Flutter, you need:
- ✅ Windows 10 or later (64-bit)
- ✅ At least 2.5 GB of disk space
- ✅ Git for Windows (we'll install this)

---

## 🚀 Step-by-Step Installation

### Step 1: Move Flutter to Permanent Location

You've already extracted Flutter. Now move it to a permanent location.

**Recommended location:** `C:\flutter`

1. Open File Explorer
2. Navigate to where you extracted Flutter
3. Copy the entire `flutter` folder
4. Paste it to `C:\` drive
5. Final path should be: `C:\flutter`

✅ **Result:** Flutter is now at `C:\flutter`

---

### Step 2: Add Flutter to System PATH

This allows you to run `flutter` command from anywhere.

#### Method 1: Using System Settings (Recommended)

1. **Open System Environment Variables:**
   - Press `Windows + R`
   - Type: `sysdm.cpl`
   - Press Enter

2. **Edit Environment Variables:**
   - Click **"Advanced"** tab
   - Click **"Environment Variables"** button

3. **Edit PATH Variable:**
   - Under **"User variables"** section
   - Find and select **"Path"**
   - Click **"Edit"**

4. **Add Flutter to PATH:**
   - Click **"New"**
   - Type: `C:\flutter\bin`
   - Click **"OK"**
   - Click **"OK"** again
   - Click **"OK"** to close

5. **Restart Terminal:**
   - Close all open Command Prompt/PowerShell windows
   - Open a new one

#### Method 2: Using PowerShell (Quick)

Open PowerShell as Administrator and run:

```powershell
[Environment]::SetEnvironmentVariable("Path", $env:Path + ";C:\flutter\bin", "User")
```

Then restart PowerShell.

✅ **Result:** Flutter is now in your PATH

---

### Step 3: Verify Flutter Installation

Open a **new** Command Prompt or PowerShell and run:

```bash
flutter --version
```

**Expected Output:**
```
Flutter 3.x.x • channel stable • https://github.com/flutter/flutter.git
Framework • revision xxxxx
Engine • revision xxxxx
Tools • Dart 3.x.x
```

✅ **If you see this, Flutter is installed!**

❌ **If you see "flutter is not recognized":**
- Check if PATH was added correctly
- Restart your terminal
- Verify Flutter is at `C:\flutter`

---

### Step 4: Run Flutter Doctor

This checks what else you need to install:

```bash
flutter doctor
```

**Expected Output:**
```
Doctor summary (to see all details, run flutter doctor -v):
[✓] Flutter (Channel stable, 3.x.x)
[✗] Android toolchain - develop for Android devices
    ✗ Unable to locate Android SDK
[✗] Chrome - develop for the web
[✗] Visual Studio - develop Windows apps
[✗] Android Studio (not installed)
[✗] VS Code (not installed)
[!] Connected device
```

Don't worry about the ✗ marks! We'll fix them.

---

### Step 5: Install Git (If Not Installed)

Flutter needs Git to work properly.

1. **Download Git:**
   - Go to: https://git-scm.com/download/win
   - Download "64-bit Git for Windows Setup"

2. **Install Git:**
   - Run the installer
   - Use default settings (just keep clicking "Next")
   - Click "Install"

3. **Verify Git:**
   ```bash
   git --version
   ```

✅ **Result:** Git is installed

---

### Step 6: Install Android Studio (For Android Development)

#### Download Android Studio

1. Go to: https://developer.android.com/studio
2. Download Android Studio
3. Run the installer

#### Install Android Studio

1. **Welcome Screen:**
   - Click "Next"

2. **Choose Components:**
   - ✅ Android Studio
   - ✅ Android Virtual Device
   - Click "Next"

3. **Install Location:**
   - Use default: `C:\Program Files\Android\Android Studio`
   - Click "Next"

4. **Start Menu Folder:**
   - Use default
   - Click "Install"

5. **Wait for Installation** (5-10 minutes)

6. **Finish:**
   - ✅ Check "Start Android Studio"
   - Click "Finish"

#### Setup Android Studio

1. **Import Settings:**
   - Select "Do not import settings"
   - Click "OK"

2. **Welcome:**
   - Click "Next"

3. **Install Type:**
   - Select "Standard"
   - Click "Next"

4. **UI Theme:**
   - Choose your preference (Light/Dark)
   - Click "Next"

5. **Verify Settings:**
   - Click "Next"

6. **License Agreement:**
   - Click "Accept"
   - Click "Finish"

7. **Downloading Components** (10-20 minutes)
   - Wait for Android SDK download
   - Wait for Android SDK Platform-Tools
   - Wait for Android Emulator

✅ **Result:** Android Studio is installed

---

### Step 7: Configure Flutter for Android

Run Flutter doctor again:

```bash
flutter doctor
```

If you see Android license issues:

```bash
flutter doctor --android-licenses
```

- Type `y` and press Enter for each license
- Accept all licenses

✅ **Result:** Android toolchain configured

---

### Step 8: Install VS Code (Recommended)

#### Download VS Code

1. Go to: https://code.visualstudio.com/
2. Download for Windows
3. Run installer

#### Install VS Code

1. Accept license
2. Use default settings
3. ✅ Check "Add to PATH"
4. Click "Install"

#### Install Flutter Extension

1. Open VS Code
2. Click Extensions icon (left sidebar) or press `Ctrl+Shift+X`
3. Search: "Flutter"
4. Click "Install" on "Flutter" by Dart Code
5. This will also install Dart extension

✅ **Result:** VS Code with Flutter is ready

---

### Step 9: Create Android Emulator

#### Open Android Studio

1. Click "More Actions" → "Virtual Device Manager"
2. Click "Create Device"

#### Choose Device

1. Select "Phone" category
2. Select "Pixel 5" (recommended)
3. Click "Next"

#### Choose System Image

1. Click "Download" next to latest Android version (e.g., "Tiramisu" or "UpsideDownCake")
2. Wait for download
3. Click "Finish"
4. Select the downloaded image
5. Click "Next"

#### Verify Configuration

1. Keep default settings
2. Click "Finish"

✅ **Result:** Android emulator created

---

### Step 10: Final Verification

Run Flutter doctor one more time:

```bash
flutter doctor
```

**Expected Output:**
```
Doctor summary:
[✓] Flutter (Channel stable, 3.x.x)
[✓] Android toolchain - develop for Android devices (Android SDK version x.x.x)
[✓] Chrome - develop for the web
[✓] Visual Studio - develop Windows apps (optional)
[✓] Android Studio (version x.x)
[✓] VS Code (version x.x.x)
[✓] Connected device (1 available)
[✓] Network resources
```

✅ **All green checkmarks = Perfect!**

⚠️ **Some yellow warnings are OK** (like Visual Studio for Windows apps)

---

## 🧪 Test Flutter Installation

### Test 1: Check Flutter Version

```bash
flutter --version
```

### Test 2: Check Dart Version

```bash
dart --version
```

### Test 3: Create Test App

```bash
# Create test app
flutter create test_app

# Navigate to app
cd test_app

# Run app
flutter run
```

**Choose device when prompted:**
- Type `1` for Android emulator
- Or `2` for Chrome

✅ **If app runs, Flutter is working!**

---

## 🎯 Now Test Your BCS Exam App

### Navigate to Your Project

```bash
cd C:\Users\User\Desktop\BCS\mobile
```

### Get Dependencies

```bash
flutter pub get
```

**Expected Output:**
```
Running "flutter pub get" in mobile...
Resolving dependencies...
✓ Got dependencies!
```

### Check for Issues

```bash
flutter doctor
```

### Run the App

```bash
flutter run
```

**Choose device:**
- Type `1` for Android emulator
- Type `2` for Chrome (web)

✅ **Your BCS Exam app should start!**

---

## 🔧 Troubleshooting

### Issue: "flutter is not recognized"

**Solution:**
1. Verify Flutter is at `C:\flutter`
2. Check PATH: `echo %PATH%` (should include `C:\flutter\bin`)
3. Restart terminal
4. If still not working, add to PATH again

### Issue: "Android SDK not found"

**Solution:**
1. Open Android Studio
2. Go to: File → Settings → Appearance & Behavior → System Settings → Android SDK
3. Note the SDK location (e.g., `C:\Users\YourName\AppData\Local\Android\Sdk`)
4. Run: `flutter config --android-sdk "C:\Users\YourName\AppData\Local\Android\Sdk"`

### Issue: "Unable to locate Android SDK"

**Solution:**
```bash
flutter doctor --android-licenses
```
Accept all licenses by typing `y`

### Issue: "No devices available"

**Solution:**
1. Start Android emulator from Android Studio
2. Or connect physical Android device with USB debugging enabled
3. Or use Chrome for web: `flutter run -d chrome`

### Issue: "Gradle build failed"

**Solution:**
```bash
cd android
./gradlew clean
cd ..
flutter clean
flutter pub get
flutter run
```

### Issue: "pub get failed"

**Solution:**
```bash
flutter clean
flutter pub cache repair
flutter pub get
```

---

## 📱 Running on Different Platforms

### Android Emulator

```bash
# List devices
flutter devices

# Run on Android
flutter run -d android
```

### Chrome (Web)

```bash
# Run on Chrome
flutter run -d chrome
```

### Windows Desktop

```bash
# Enable Windows desktop
flutter config --enable-windows-desktop

# Run on Windows
flutter run -d windows
```

---

## ✅ Installation Checklist

- [ ] Flutter extracted to `C:\flutter`
- [ ] Flutter added to PATH
- [ ] `flutter --version` works
- [ ] Git installed
- [ ] Android Studio installed
- [ ] Android SDK installed
- [ ] Android licenses accepted
- [ ] VS Code installed
- [ ] Flutter extension installed
- [ ] Android emulator created
- [ ] `flutter doctor` shows green checkmarks
- [ ] Test app runs successfully
- [ ] BCS Exam app dependencies installed

---

## 🎓 Quick Reference

### Common Commands

```bash
# Check Flutter version
flutter --version

# Check what's missing
flutter doctor

# Check detailed info
flutter doctor -v

# List available devices
flutter devices

# Create new app
flutter create app_name

# Get dependencies
flutter pub get

# Clean project
flutter clean

# Run app
flutter run

# Run on specific device
flutter run -d chrome
flutter run -d android

# Build APK
flutter build apk

# Build for release
flutter build apk --release
```

---

## 📚 Useful Links

- **Flutter Docs:** https://docs.flutter.dev/
- **Flutter Installation:** https://docs.flutter.dev/get-started/install/windows
- **Android Studio:** https://developer.android.com/studio
- **VS Code:** https://code.visualstudio.com/
- **Git:** https://git-scm.com/

---

## 🆘 Still Having Issues?

### Check Flutter Installation

```bash
flutter doctor -v
```

This shows detailed information about what's wrong.

### Common Fixes

```bash
# Repair Flutter
flutter doctor

# Update Flutter
flutter upgrade

# Repair pub cache
flutter pub cache repair

# Clean everything
flutter clean
```

---

## 🎉 Success!

Once you see:
```bash
flutter doctor
```

With mostly green checkmarks, you're ready to develop!

---

**Next Steps:**
1. ✅ Flutter is installed
2. ✅ Run `flutter pub get` in your mobile folder
3. ✅ Run `flutter run` to start your BCS Exam app
4. ✅ Start developing!

---

*Last Updated: February 23, 2026*
*For: Windows 10/11*
