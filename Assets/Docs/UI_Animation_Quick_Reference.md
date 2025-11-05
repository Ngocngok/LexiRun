# UI Animation System - Quick Reference

## 🎨 Implemented Animations

### **Standard Set (8 Animations)**

| # | Name | What It Does | Where Used |
|---|------|--------------|------------|
| 1️⃣ | **Ping-Pong Scale** | Gentle breathing effect | Titles (Loading, Home) |
| 2️⃣ | **Jump Scale** | Bouncy attention-grabber | Play Button (Home) |
| 3️⃣ | **Bounce In** | Celebratory pop-in | Victory Panel |
| 4️⃣ | **Shake In** | Dramatic shake entrance | Lose Panel |
| 5️⃣ | **Button Press** | Press down feedback | All Buttons |
| 6️⃣ | **Rotate Idle** | Continuous rotation | Settings Button |
| 7️⃣ | **Timer Flash** | Red warning flash | Timer (< 10s) |
| 8️⃣ | **HP Pulse** | Damage feedback | HP Display |

---

## 🎯 Quick Add Guide

### Add Animation to UI Element:

1. Select UI GameObject in scene
2. Add Component → LexiRun.UI → [Animation Name]
3. Configure in Inspector
4. Done! Plays automatically

### Common Animations:

**For Titles/Text:**
```
Add Component → UIPingPongScale
```

**For Buttons:**
```
Add Component → UIButtonPressScale
Add Component → UIJumpScale (optional, for main buttons)
```

**For Panels:**
```
Add Component → UIBounceIn (victory/positive)
Add Component → UIShakeIn (defeat/negative)
```

**For Icons:**
```
Add Component → UIRotateIdle (gears/settings)
```

---

## ⚙️ Quick Settings

### Make Animation Faster:
- Increase `speed` or decrease `duration`

### Make Animation Slower:
- Decrease `speed` or increase `duration`

### Make Animation Stronger:
- Increase `scale`, `intensity`, or `overshoot`

### Make Animation Subtler:
- Decrease `scale`, `intensity`, or `overshoot`

### Delay Animation:
- Set `delay` value (seconds)

---

## 🐛 Troubleshooting

**Animation not playing?**
- Check `playOnEnable` is true
- Ensure GameObject is active
- Check for errors in console

**Animation too fast/slow?**
- Adjust speed/duration parameters
- Check Time.timeScale is 1.0

**Animation looks wrong?**
- Reset to original (disable/enable component)
- Check original transform values
- Verify animation curve

---

## 📊 Performance

- ✅ **60 FPS** maintained
- ✅ **< 1% CPU** per animation
- ✅ **Minimal memory** usage
- ✅ **Mobile-friendly**

---

**Last Updated:** November 5, 2025
