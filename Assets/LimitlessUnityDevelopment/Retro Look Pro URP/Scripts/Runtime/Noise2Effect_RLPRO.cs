using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RetroLookPro.Enums;

public class Noise2Effect_RLPRO : ScriptableRendererFeature
{
	Noise2Effect_RLPROPass RetroPass;
	public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;


	public override void Create()
	{
		RetroPass = new Noise2Effect_RLPROPass(Event);
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
	public class Noise2Effect_RLPROPass : ScriptableRenderPass
	{
		static readonly string k_RenderTag = "Renderr Glitch1 Effect";
		static readonly int MainTexId = Shader.PropertyToID("_MainTex");
		static readonly int GuillotineEffectV = Shader.PropertyToID("GuillotineEffect");
		static readonly int _FadeMultiplier = Shader.PropertyToID("_FadeMultiplier");
		static readonly int _Mask = Shader.PropertyToID("_Mask");


		static readonly int TempTargetId = Shader.PropertyToID("Noise2Effect_RLPRO");

		Noise2 retroEffect;
		Material RetroEffectMaterial;
		RenderTargetIdentifier currentTarget;

		public Noise2Effect_RLPROPass(RenderPassEvent evt)
		{
			renderPassEvent = evt;
			var shader = Shader.Find("Hidden/Shader/Noise2Effect_RLPRO");
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
			retroEffect = stack.GetComponent<Noise2>();
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

            RetroEffectMaterial.SetFloat("threshold",1- retroEffect.threshold.value);
				RetroEffectMaterial.SetFloat("Smoother", retroEffect.Smoother.value?1:0);
				RetroEffectMaterial.SetFloat("Fade", retroEffect.fade.value);
				RetroEffectMaterial.SetFloat("waveAmount", retroEffect.waveAmount.value);
				RetroEffectMaterial.SetFloat("tapeLinesAmount", retroEffect.tapeLinesAmount.value);
				RetroEffectMaterial.SetFloat("tapeIntensity", retroEffect.tapeIntensity.value);
				RetroEffectMaterial.SetFloat("tapeSpeed", retroEffect.tapeSpeed.value);
				RetroEffectMaterial.SetInt("NoiseVal", retroEffect.Noise == noiseParam.blackWhite ? 0 : 1);

            cmd.SetGlobalTexture(MainTexId, source);

			cmd.GetTemporaryRT(destination, Screen.width, Screen.height, 0, FilterMode.Point, RenderTextureFormat.Default);

			cmd.Blit(source, destination);

			cmd.Blit(destination, source, RetroEffectMaterial, 0);
		}
		private void ParamSwitch(Material mat, bool paramValue, string paramName)
		{
			if (paramValue) mat.EnableKeyword(paramName);
			else mat.DisableKeyword(paramName);
		}

	}

}


