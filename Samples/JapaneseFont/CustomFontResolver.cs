using System.IO;
using PdfSharpCore.Fonts;
using PdfSharpCore.Utils;

namespace LesserMigraDocCore.Samples.CreateOnTheFly
{
    public class CustomFontResolver : IFontResolver
    {
        private readonly FontResolver delegation = new FontResolver();
        public string DefaultFontName => delegation.DefaultFontName;

        public byte[] GetFont(string faceName)
        {
            var path = Path.Join("fonts", faceName) + ".ttf";

            if (File.Exists(path)) {
                return File.ReadAllBytes(path);
            }
            else
            {
                return delegation.GetFont(faceName);
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            switch (familyName.ToLower())
            {
                case "gen shin gothic":
                    return new FontResolverInfo("GenShinGothicN");
                case "takaomincho":
                    return new FontResolverInfo("TakaoMincho");
                default:
                    return delegation.ResolveTypeface(familyName, isBold, isItalic);
            }
        }
    }
}