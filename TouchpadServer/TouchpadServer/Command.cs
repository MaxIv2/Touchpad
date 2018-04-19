using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    abstract class Command {
        public enum ActionCode : byte { MOVE = 0, LEFTBUTTON = 1, RIGHTBUTTON = 2, SCROLL = 3, ZOOM = 4 };
        public ActionCode code { get; protected set; }

        protected Command(ActionCode code) {
            this.code = code;
        }

        public sealed class Move : Command {
            public Move(sbyte dx, sbyte dy) : base(ActionCode.MOVE) {
                this.dx = dx;
                this.dy = dy;
            }

            public sbyte dx;
            public sbyte dy;
        }
        public sealed class LeftButton : Command {
            public LeftButton(byte state) : base(ActionCode.LEFTBUTTON) {
                this.state = state;
            }

            public byte state;
        }
        public sealed class RightButton : Command {
            public RightButton(byte state) : base(ActionCode.RIGHTBUTTON) {
                this.state = state;
            }

            public byte state;
        }
        public sealed class Scroll : Command {
            public Scroll(sbyte scroll) : base(ActionCode.SCROLL) {
                this.scroll = scroll;
            }
            public sbyte scroll;
        }
        public sealed class Zoom : Command {
            public Zoom(sbyte scroll) : base(ActionCode.ZOOM) {
                    this.zoom = scroll;
            }
            public sbyte zoom;
        }
    }
}
