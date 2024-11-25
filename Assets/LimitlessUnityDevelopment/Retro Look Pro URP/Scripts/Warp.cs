﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenu("Retro Look Pro/Warp")]

public class Warp : VolumeComponent, IPostProcessComponent
{
    public BoolParameter enable = new BoolParameter(false);
    [Tooltip("fade adjustment.")]
    public ClampedFloatParameter fade = new ClampedFloatParameter(0f, 0f, 1f, true);
    [Tooltip("Warp mode.")]
    public WarpModeParameter warpMode = new WarpModeParameter();
    [Tooltip("Warp image corners on x/y axes.")]
    public NoInterpVector2Parameter warp = new NoInterpVector2Parameter(new Vector2(0.03125f, 0.04166f));
    [Tooltip("Warp picture center.")]
    public NoInterpFloatParameter scale = new NoInterpFloatParameter(1f);
    [Space]
    [Tooltip("Mask texture")]
    public TextureParameter mask = new TextureParameter(null);
    public maskChannelModeParameter maskChannel = new maskChannelModeParameter();
    [Space]
    [Tooltip("Use Global Post Processing Settings to enable or disable Post Processing in scene view or via camera setup. THIS SETTING SHOULD BE TURNED OFF FOR EFFECTS, IN CASE OF USING THEM FOR SEPARATE LAYERS")]
    public BoolParameter GlobalPostProcessingSettings = new BoolParameter(false);


    public bool IsActive() => (bool)enable;

    public bool IsTileCompatible() => false;
}