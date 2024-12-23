﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class OldFilm2_RLPRO : ScriptableRendererFeature
{
	OldFilm2_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new OldFilm2_RLPROPass(Event);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
#if UNITY_2019 || UNITY_2020
		RetroPass.Setup(renderer.cameraColorTarget);
#else

#endif
        if (renderingData.cameraData.cameraType == CameraType.Game)
            renderer.EnqueuePass(RetroPass);
	}
	public class OldFilm2_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int TempTargetId = Shader.PropertyToID("Glitch1rr");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");

		OldFilm2 retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;
		private float T;

		public OldFilm2_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/OldFilm2Effect_RLPRO");
			if (shader == null)
			{
				Debug.LogError("Shader not found.");
				return;
			}
			RetroEffectMaterial = CoreUtils.CreateEngineMaterial(shader);
		}
#if UNITY_2019 || UNITY_2020

#elif UNITY_2021
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			var renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTarget;
		}
#else
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			var renderer = renderingData.cameraData.renderer;
			currentTarget = renderer.cameraColorTargetHandle;
		}
#endif

		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (RetroEffectMaterial == null)
			{
				Debug.LogError("Material not created.");
				return;
			}
            var stack = VolumeManager.instance.stack;
			retroEffect = stack.GetComponent<OldFilm2>();
            if (!renderingData.cameraData.postProcessEnabled && retroEffect.GlobalPostProcessingSettings.value) return;

            if (retroEffect == null) { return; }
			if (!retroEffect.IsActive()) { return; }

			var cmd = CommandBufferPool.Get(k_RenderTag);
			Render(cmd, ref renderingData);
			context.ExecuteCommandBuffer(cmd);
			CommandBufferPool.Release(cmd);
		}

		public void Setup(in RenderTargetIdentifier currentTarget)
		{
			this.currentTarget = currentTarget;
		}

		void Render(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ref var cameraData = ref renderingData.cameraData;
			var source = currentTarget;
			int destination = TempTargetId;

			int shaderPass = 0;
            RetroEffectMaterial.SetFloat("SepiaValue", retroEffect.SepiaAmount.value);
            RetroEffectMaterial.SetFloat("NoiseValue", retroEffect.NoiseAmount.value);
            RetroEffectMaterial.SetFloat("ScratchValue", retroEffect.ScratchAmount.value);
            RetroEffectMaterial.SetFloat("ScratchSize", retroEffect.ScratchSize.value);
            RetroEffectMaterial.SetFloat("ScratchResolution", retroEffect.ScratchResolution.value);
            RetroEffectMaterial.SetFloat("_Grain", retroEffect.Grain.value);
            RetroEffectMaterial.SetFloat("Fade", retroEffect.Fade.value);
            RetroEffectMaterial.SetFloat("speed", retroEffect.speed.value);
			if (retroEffect.mask.value != null)
			{
				RetroEffectMaterial.SetTexture(_Mask, retroEffect.mask.value);
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 1);
				ParamSwitch(RetroEffectMaterial, retroEffect.maskChannel.value == maskChannelMode.alphaChannel ? true : false, "ALPHA_CHANNEL");
			}
			else
			{
				RetroEffectMaterial.SetFloat(_FadeMultiplier, 0);
			}


			cmd.SetGlobalTexture(MainTexId, source);
			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);
			cmd.Blit(source, destination);
			cmd.Blit(destination, source, RetroEffectMaterial, shaderPass);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}


