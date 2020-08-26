using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace DvMod.DerailFix
{
    [HarmonyPatch(typeof(TrainStress), "FixedUpdate")]
    static class TrainStressPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var smoothDampMethods = AccessTools.GetDeclaredMethods(typeof(Vector3)).Where(m => m.Name == "SmoothDamp");
            var smoothDamp5Param = smoothDampMethods.First(m => m.GetParameters().Length == 5);
            var smoothDamp6Param = smoothDampMethods.First(m => m.GetParameters().Length == 6);

            foreach (var inst in instructions)
            {
                if (inst.Calls(smoothDamp5Param))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Time), "fixedDeltaTime"));
                    yield return new CodeInstruction(OpCodes.Call, smoothDamp6Param);
                }
                else
                    yield return inst;
            };
        }
    }
}