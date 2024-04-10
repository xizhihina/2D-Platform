# 2D-Platform

tree /f /a | findstr /v ".meta"

```
+---ProPlatformer
|   |   PlayerParam.asset
|   |
|   +---Plugins
|   |   |
|   |   \---Demigiant
|   |       |
|   |       \---DOTween
|   |           |   DOTween.dll
|   |           |   DOTween.XML
|   |           |   readme.txt
|   |           |
|   |           +---Editor
|   |           |       DOTweenEditor.dll
|   |           |       DOTweenEditor.XML
|   |           |
|   |           \---Modules
|   |                   DOTweenModuleAudio.cs
|   |                   DOTweenModulePhysics.cs
|   |                   DOTweenModulePhysics2D.cs
|   |                   DOTweenModuleSprite.cs
|   |                   DOTweenModuleUI.cs
|   |                   DOTweenModuleUnityVersion.cs
|   |                   DOTweenModuleUtils.cs
|   |
|   +---Scenes
|   |       Main.unity
|   |
|   +---_Arts
|   |   |
|   |   +---Materials
|   |   |   |
|   |   |   +---Clouds
|   |   |   |       cloud.mat
|   |   |   |       cloud_2x2_hard.mat
|   |   |   |       cloud_2x2_hardshadow.mat
|   |   |   |       cloud_2x2_soft.mat
|   |   |   |       cloud_2x2_softshadow.mat
|   |   |   |       cloud_magic.mat
|   |   |   |       cloud_soft.mat
|   |   |   |       Dust.mat
|   |   |   |
|   |   |   \---Effects
|   |   |           DashFlux.mat
|   |   |           DashLine.mat
|   |   |           Distort.mat
|   |   |           FallDust.mat
|   |   |           JumpDust.mat
|   |   |           MoveDust.mat
|   |   |           Ripple.mat
|   |   |           Trail.mat
|   |   |           TrailSpriteEffect.mat
|   |   |
|   |   +---Shaders
|   |   |       Player.shader
|   |   |       PostEffects.shader
|   |   |       PostFXMask.shader
|   |   |
|   |   \---Textures
|   |       |
|   |       +---Effect
|   |       |   |   dash_line.png
|   |       |   |   dash_orb.png
|   |       |   |   distort.png
|   |       |   |   dust.png
|   |       |   |   dust_2x2_hard.png
|   |       |   |   dust_2x2_hardshadow.png
|   |       |   |   dust_2x2_soft.png
|   |       |   |   dust_2x2_softshadow.png
|   |       |   |   dust_magic.png
|   |       |   |   dust_soft.png
|   |       |   |   SpeedRing.png
|   |       |   |
|   |       |   \---keyframes
|   |       |       |   long_wood_spike.png
|   |       |       |   small_wood_spike.png
|   |       |       |
|   |       |       |
|   |       |       +---long_wood
|   |       |       |       long_wood_spike_01.png
|   |       |       |       long_wood_spike_02.png
|   |       |       |       long_wood_spike_03.png
|   |       |       |       long_wood_spike_04.png
|   |       |       |       long_wood_spike_05.png
|   |       |       |
|   |       |       |
|   |       |       \---small_wood
|   |       |               small_wood_spike_01.png
|   |       |               small_wood_spike_02.png
|   |       |               small_wood_spike_03.png
|   |       |               small_wood_spike_04.png
|   |       |
|   |       +---Level
|   |       |       Blue.png
|   |       |       gradient.png
|   |       |       Green.png
|   |       |       Pink.png
|   |       |       White.png
|   |       |       Yellow.png
|   |       |
|   |       \---Player
|   |               Ex05.png
|   |               Ex06.png
|   |               Hair01.png
|   |               Hair01_B.png
|   |               Hair01_R.png
|   |               Hair01_W.png
|   |               Hair02_B.png
|   |               Hair02_R.png
|   |               Hair02_W.png
|   |               Player01.png
|   |
|   +---_Prefabs
|   |   |   Camera.prefab
|   |   |   PlayerRenderer.prefab
|   |   |   SceneEffectManager.prefab
|   |   |   Trail.prefab
|   |   |   TrailSprite.prefab
|   |   |
|   |   \---Effects
|   |           DashFlux.prefab
|   |           DashImpulseSource.prefab
|   |           DashLine.prefab
|   |           JumpDust.prefab
|   |           LandDust.prefab
|   |           MoveDust.prefab
|   |           RippleEffect.prefab
|   |           SpeedRing.prefab
|   |
|   \---_Scripts
|       |   Constants.cs
|       |   Game.cs
|       |   GameInput.cs
|       |   Player.cs
|       |   PlayerRenderer.cs
|       |   SceneCamera.cs
|       |
|       +---Common
|       |       AssetHelper.cs
|       |       Calc.cs
|       |       Ease.cs
|       |       Log.cs
|       |       ScreenResolutionUtil.cs
|       |
|       +---Components
|       |       JumpCheck.cs
|       |       WallBoost.cs
|       |
|       +---Configs
|       |       PlayerParams.cs
|       |
|       +---Core
|       |   |   PlayerController.Collider.cs
|       |   |   PlayerController.cs
|       |   |   PlayerController.Gizmo.cs
|       |   |   PlayerController.Renderer.cs
|       |   |   PlayerController.Scene.cs
|       |   |
|       |   \---States
|       |           ClimbState.cs
|       |           Coroutine.cs
|       |           DashState.cs
|       |           FiniteStateMachine.cs
|       |           NormalState.cs
|       |
|       +---Effects
|       |       EffectComponent.cs
|       |       SceneEffectManager.cs
|       |       TrailSnapshot.cs
|       |       VfxEffect.cs
|       |
|       +---Level
|       |       Ground.cs
|       |       Level.cs
|       |
|       \---PostProcess
|               DashImpulse.cs
|               PostEffectsController.cs
|               RippleEffect.cs
|
\---Resources
        DOTweenSettings.asset
        PlayerParam.asset
        PlayerRenderer.prefab
```