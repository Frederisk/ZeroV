using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroV.Game.Elements.Particles;
internal partial class SlideParticle : HittableParticle {
    public SlideParticle(Orbit fatherOrbit) : base(fatherOrbit) {
    }
}
