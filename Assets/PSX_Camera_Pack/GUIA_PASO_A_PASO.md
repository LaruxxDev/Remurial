# PSX Camera Effect — Guía paso a paso
### Unity 6 + URP + Cinemachine

---

## 📁 Archivos incluidos

```
PSX_Camera_Pack/
├── Shaders/
│   └── PSX_Camera_Effect.shader    ← Shader de pixelado + dithering
└── Scripts/
    └── PSXCameraFeature.cs         ← Renderer Feature para URP
```

---

## 📋 INSTALACIÓN — 4 pasos

---

### PASO 1 — Copiar archivos al proyecto

Arrastra la carpeta `PSX_Camera_Pack` dentro de `Assets/` en Unity.

```
Assets/
└── PSX_Camera_Pack/         ← aqui
    ├── Shaders/
    └── Scripts/
```

Espera que Unity compile (barra de progreso en la parte inferior).
Si ves errores en rojo en la Console, revisa que los dos archivos estén dentro de `Assets/`.

---

### PASO 2 — Encontrar tu URP Renderer Asset

1. Ve a `Edit > Project Settings > Graphics`
2. Mira el campo **Default Render Pipeline** — ahí verás tu asset (en tu caso se llama `PC_RPAsset`)
3. **Haz clic en ese asset** para seleccionarlo en el Project panel
4. Con el asset seleccionado, míralo en el **Inspector**

> También puedes buscarlo directamente en el Project panel escribiendo `PC_RPAsset` o `UniversalRenderPipelineAsset`.

---

### PASO 3 — Agregar el Renderer Feature

Dentro del **Inspector del URP Renderer Asset**:

1. Busca el apartado **"Renderer Features"** (abajo del todo)
2. Haz clic en el botón **"+ Add Renderer Feature"**
3. En el desplegable, selecciona **"PSX Camera Feature"**

Si no aparece en la lista → Unity aún no terminó de compilar. Espera unos segundos y vuelve a intentarlo.

---

### PASO 4 — Configurar los efectos

Una vez añadido, verás estos sliders en el Inspector:

| Parámetro | Qué hace | Valor PS1 auténtico |
|-----------|----------|---------------------|
| **Target Width** | Resolución horizontal interna | `320` |
| **Target Height** | Resolución vertical interna | `240` |
| **Dither Strength** | Intensidad del tramado Bayer | `0.6` |
| **Color Bits** | Bits de color por canal | `5` |

**Valores recomendados para empezar:**
- `Target Width = 320`, `Target Height = 240`
- `Dither Strength = 0.6`
- `Color Bits = 5`

Entra en **Play Mode** para ver el resultado en tiempo real.

---

## 🎮 ¿Cómo funciona con Cinemachine?

El Renderer Feature se aplica **después de todos los efectos**, incluyendo Cinemachine.
No necesitas tocar nada en Cinemachine — el efecto se ve en la cámara activa automáticamente.

Si tienes **múltiples cámaras Cinemachine** (split-screen, etc.), el efecto se aplica a la cámara principal de Unity que recibe el output de Cinemachine, que es el comportamiento correcto.

---

## ❗ Problemas comunes

### La pantalla se ve negra o en blanco
→ El shader no se encontró. Verifica que `PSX_Camera_Effect.shader` está dentro de `Assets/`
→ Busca en la Console el error `[PSXCameraFeature] No se encontro 'Hidden/PSX_Camera_Effect'`

### "PSX Camera Feature" no aparece al hacer Add Renderer Feature
→ Espera que Unity termine de compilar
→ Verifica que `PSXCameraFeature.cs` no tiene errores (Console sin texto rojo)

### El efecto no se ve en el Game View pero no hay errores
→ Comprueba que añadiste el Feature al Renderer correcto
→ Ve a `Edit > Project Settings > Quality` y verifica qué Render Pipeline Asset usa cada nivel de calidad

### El pixelado se ve borroso en lugar de nítido
→ En el URP Asset, busca **"Anti Aliasing"** y ponlo en **"None"** o **"FXAA"** (no MSAA)

---

## 💡 Tips de ajuste

- **Más auténtico**: `Color Bits = 4` con `Dither Strength = 0.8` da un look muy PS1
- **Más suave**: `Target Width = 480`, `Color Bits = 6` para un PS2 temprano
- **Solo dithering sin pixelado**: Sube `Target Width/Height` a la resolución real de tu pantalla
- Puedes **activar/desactivar el Feature** desde código con `feature.SetActive(false)` para hacer transiciones

---

*Creado para Unity 6.3 LTS + URP*
