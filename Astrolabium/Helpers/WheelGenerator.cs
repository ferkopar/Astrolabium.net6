using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Astrolabium.Models;
using System.IO;
using Astrolabium.Models;

namespace Astrolabium.Helpers
{
    public static class WheelGenerator
    {
        public static Image GenerateWheel(EphemerisResult r,int x,int y)
        {
            var bitmap = new Bitmap(x, y);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawLine(Pens.White, 1, 1, x - 1, y - 1);
            }
            return bitmap;
        }
    }
}
