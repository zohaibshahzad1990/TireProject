using FFImageLoading;
using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TireProject
{

    public class FileFontResolver : IFontResolver // FontResolverBase
    {
        public string DefaultFontName => throw new NotImplementedException();

        public byte[] GetFont(string faceName)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FileFontResolver)).Assembly;
           
            using (Stream stream = assembly.GetManifestResourceStream(faceName))
            {
                return stream.ToByteArray();
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("sans-serif", StringComparison.CurrentCultureIgnoreCase))
            {
                if (isBold && isItalic)
                {
                    return new FontResolverInfo("TireProject.Font.Roboto-BoldItalic.ttf");
                }
                else if (isBold)
                {
                    return new FontResolverInfo("TireProject.Font.Roboto-Bold.ttf");
                }
                else if (isItalic)
                {
                    return new FontResolverInfo("TireProject.Font.Roboto-Italic.ttf");
                }
                else
                {
                    return new FontResolverInfo("TireProject.Font.Roboto-Regular.ttf");
                }
            }
            return null;
        }
    }
}
