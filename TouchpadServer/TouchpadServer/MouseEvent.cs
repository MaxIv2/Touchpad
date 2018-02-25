using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouchpadServer {
    public class MouseEvent {
        public enum ActionCodes { MOVE = 0, LEFTBUTTON = 1, RIGHTBUTTON = 2, SCROLL = 3, ZOOM = 4 }

        public byte actionCode;
        public Description description;

        public MouseEvent(byte actionCode, Description description) {
            this.actionCode = actionCode;
            this.description = description;
        }
    }

    public abstract class Description {
        public class MoveDescription : Description {
            public sbyte dx { get; set; }
            public sbyte dy { get; set; }

            public MoveDescription(sbyte dx, sbyte dy) {
                this.dx = dx;
                this.dy = dy;
            }
        }

        public class ButtonEventDescription : Description {
            public byte state { get; set; }

            public ButtonEventDescription(byte state) {
                this.state = state;
            }
        }

        public class ScrollDescription : Description {
            public sbyte scroll { get; set; }

            public ScrollDescription(sbyte scroll) {
                this.scroll = scroll;
            }
        }

        public class ZoomDescription : Description {
            public sbyte zoom { get; set; }

            public ZoomDescription(sbyte zoom) {
                this.zoom = zoom;
            }
        }
    }
}
