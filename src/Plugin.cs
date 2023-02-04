using BepInEx;
using System.Security.Permissions;

#pragma warning disable CS0618 // Do not remove the following line.
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace SunHat;

[BepInPlugin("sun.hat", "Sun Hat", "1.0.1")]
sealed class Plugin : BaseUnityPlugin
{
    FAtlas atlas;

    public void OnEnable()
    {
        On.RainWorld.OnModsInit += Init;
        On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
    }

    private void Init(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        atlas ??= Futile.atlasManager.LoadAtlas("sprites/sunhat");

        if (atlas == null) {
            Logger.LogWarning("Sun Hat atlas not found! Reinstall the mod.");
        }
    }

    private void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, UnityEngine.Vector2 camPos)
    {
        orig(self, sLeaser, rCam, timeStacker, camPos);

        if (atlas == null) {
            return;
        }

        string name = sLeaser.sprites[3]?.element?.name;
        if (name != null && name.StartsWith("HeadA") && atlas._elementsByName.TryGetValue("Sun" + name, out var element)) {
            sLeaser.sprites[3].element = element;
        }
    }
}
