using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using FrooxEngine.UIX;
using BaseX;

namespace OpenLinkedVariableSpace
{
    public class OpenLinkedVariableSpace : NeosMod
    {
        public override string Name => "OpenLinkedVariableSpace";
        public override string Author => "HinanoAira";
        public override string Version => "1.0.1";
        public override string Link => "https://github.com/HinanoAira/OpneLinkedVariableSpace";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("jp.hinasense.OpenLinkedVariableSpace");
            harmony.PatchAll();
        }
        [HarmonyPatch(typeof(WorkerInspector))]
        private static class WorkerInspectorPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(WorkerInspector.BuildInspectorUI))]
            private static void BuildInspectorUIPostfix(Worker worker, UIBuilder ui)
            {
                if (!(worker is IDynamicVariable variable))
                    return;

                var namesButton = ui.Button("Open Dynamic Variable Space");
                namesButton.LocalPressed += (button, data) => OpenSpace(variable);
                namesButton.RequireLockInToPress.Value = true;
            }
            private static void OpenSpace(IDynamicVariable variable)
            {
                InspectorHelper.OpenInspectorForTarget((IWorldElement)Traverse.Create(variable).Field("handler").Field("_currentSpace").GetValue(), null, true);
            }
        }
    }
}
