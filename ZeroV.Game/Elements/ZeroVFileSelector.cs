using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Elements;
public partial class ZeroVFileSelector : BasicFileSelector {
    public ZeroVFileSelector(String? initialPath = null, String[]? validFileExtensions = null) : base(initialPath, validFileExtensions) {
    }
}
