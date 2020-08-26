using HarmonyLib;
using UnityModManagerNet;

namespace DvMod.DerailFix
{
    [EnableReloading]
    public static class Main
    {
        public static UnityModManager.ModEntry mod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            return true;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool active)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            if (active)
                harmony.PatchAll();
            else
                harmony.UnpatchAll(modEntry.Info.Id);

            return true;
        }

        public static void DebugLog(string message)
        {
            mod.Logger.Log(message);
        }
    }
}
