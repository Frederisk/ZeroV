using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using osu.Framework.Graphics.UserInterface;

namespace ZeroV.Game.Elements;

public partial class ZeroVDropdown<T> : BasicDropdown<T> {
    // public Single HeaderHeight => this.Header.Height;

    // public Action<Boolean>? MenuOpenChanged;

    // protected override DropdownMenu CreateMenu() {
    //     DropdownMenu menu = base.CreateMenu();
    //     menu.StateChanged += state => this.MenuOpenChanged?.Invoke(state is MenuState.Open);
    //     return menu;
    // }
}
