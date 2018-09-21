using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CustomRenderPipelineAsset : RenderPipelineAsset
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Render pipeline/Pipeline Asset")]
    static void CreateCustomPipeline()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0, CreateInstance<CreateCustomEndAction>(),
            "Custom pipeline.asset", null, null);
    }

    class CreateCustomEndAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var instance = CreateInstance<CustomRenderPipelineAsset>();
            AssetDatabase.CreateAsset(instance, pathName);
        }
    }

#endif

    protected override IRenderPipeline InternalCreatePipeline()
    {
        return new CustomRenderPipeline();
    }
}
