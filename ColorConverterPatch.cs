using System;
using System.Linq;
using System.Reflection;
using Aki.Reflection.Patching;
using JsonType;
using UnityEngine;

namespace RaiRai.ColorConverter
{
    public class ColorConverterPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.AbstractGame).Assembly.GetTypes()
                .Single(type => type.GetMethod("ToColor", BindingFlags.Public | BindingFlags.Static) != null)
                .GetMethod("ToColor", BindingFlags.Public | BindingFlags.Static);
        }

        private static Color32 HexToColor(string hexColor)
        {
            var num = Convert.ToByte(hexColor.Substring(0, 2), 16);
            var num2 = Convert.ToByte(hexColor.Substring(2, 2), 16);
            var num3 = Convert.ToByte(hexColor.Substring(4, 2), 16);
            return new Color32(num, num2, num3, 255);
        }

        [PatchPrefix]
        private static bool PrePatch(ref Color __result, JsonType.TaxonomyColor taxonomyColor) 
        {
            if (Enum.IsDefined(typeof(JsonType.TaxonomyColor), taxonomyColor)) 
            {
               return true;
            }

            var colorCodeAsInt = (int)taxonomyColor;
            colorCodeAsInt -= Enum.GetValues(typeof(TaxonomyColor)).Length;

            var colorCode = colorCodeAsInt.ToString("X6");
            __result = HexToColor(colorCode);

            return false;

        }
    }
}