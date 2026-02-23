# Flutter Quick Setup - Windows

Super quick guide to get Flutter running in 10 minutes!

---

## 🚀 Quick Steps

### 1️⃣ Move Flutter (1 minute)

```
Your extracted folder → C:\flutter
```

**Steps:**
1. Open File Explorer
2. Find your extracted `flutter` folder
3. Copy it
4. Paste to `C:\` drive
5. Final path: `C:\flutter`

---

### 2️⃣ Add to PATH (2 minutes)

**Quick Method:**

1. Press `Windows + R`
2. Type: `sysdm.cpl`
3. Press Enter
4. Click "Advanced" tab
5. Click "Environment Variables"
6. Under "User variables", select "Path"
7. Click "Edit"
8. Click "New"
9. Type: `C:\flutter\bin`
10. Click OK, OK, OK

**Close all terminals and open a new one!**

---

### 3️⃣ Verify Installation (1 minute)

Open **new** Command Prompt or PowerShell:

```bash
flutter --version
```

✅ **See Flutter version? Success!**
❌ **Error? Restart terminal and try again**

---

### 4️⃣ Run Flutter Doctor (1 minute)

```bash
flutter doctor
```

You'll see what's missing. Don't worry about ✗ marks yet!

---

### 5️⃣ Install Git (2 minutes)

1. Go to: https://git-scm.com/download/win
2. Download and install
3. Use default settings

Verify:
```bash
git --version
```

---

### 6️⃣ Install Android Studio (Optional - 15 minutes)

**Only if you want Android development**

1. Download: https://developer.android.com/studio
2. Install with default settings
3. Wait for SDK download

---

### 7️⃣ Test Your BCS App (2 minutes)

```bash
# Navigate to your project
cd C:\Users\User\Desktop\BCS\mobile

# Get dependencies
flutter pub get

# Run on Chrome (easiest)
flutter run -d chrome
```

---

## 🎯 Minimum Setup (For Web Only)

If you just want to test on Chrome:

```bash
# 1. Add Flutter to PATH (see step 2 above)

# 2. Verify
flutter --version

# 3. Install Git
# Download from: https://git-scm.com/download/win

# 4. Navigate to project
cd C:\Users\User\Desktop\BCS\mobile

# 5. Get dependencies
flutter pub get

# 6. Run on Chrome
flutter run -d chrome
```

✅ **Done! Your app runs in Chrome**

---

## 🔧 Quick Fixes

### "flutter is not recognized"

**Fix:**
1. Check Flutter is at `C:\flutter`
2. Check PATH includes `C:\flutter\bin`
3. **Restart terminal** (important!)
4. Try again

### "pub get failed"

**Fix:**
```bash
flutter clean
flutter pub get
```

### "No devices available"

**Fix:**
```bash
# Run on Chrome (no setup needed)
flutter run -d chrome
```

---

## 📱 Device Options

### Option 1: Chrome (Easiest - No Setup)
```bash
flutter run -d chrome
```
✅ Works immediately
✅ No emulator needed
⚠️ Web version only

### Option 2: Android Emulator (Requires Android Studio)
```bash
flutter run -d android
```
✅ Full Android experience
❌ Requires Android Studio setup

### Option 3: Physical Android Device
```bash
# Enable USB debugging on phone
# Connect via USB
flutter run
```
✅ Real device testing
⚠️ Requires USB debugging

---

## ✅ Success Checklist

Minimum for web development:
- [ ] Flutter at `C:\flutter`
- [ ] Flutter in PATH
- [ ] `flutter --version` works
- [ ] Git installed
- [ ] `flutter pub get` works
- [ ] `flutter run -d chrome` works

---

## 🎉 You're Ready!

Once `flutter run -d chrome` works, you can:
- ✅ Develop your BCS Exam app
- ✅ Test in Chrome browser
- ✅ Make changes and hot reload
- ✅ Build for production later

---

## 📚 Full Guide

For complete installation with Android Studio:
- See: `FLUTTER_INSTALLATION_GUIDE.md`

---

**Quick Start Command:**

```bash
cd C:\Users\User\Desktop\BCS\mobile
flutter pub get
flutter run -d chrome
```

---

*Last Updated: February 23, 2026*
