using System;
using System.Drawing;

namespace Uniterm
{
    public class UnitermDesigner
    {
        private int minWidth = 770;
        private int minHeight = 300;
        private Uniterm uniterm;
        private Pen pen;
        private Font font;
        private Bitmap bitmap;
        private Graphics g;

        public UnitermDesigner(Uniterm uniterm)
        {
            this.uniterm = uniterm;
        }

        public Bitmap Draw()
        {
            font = new Font(uniterm.fontFamily, uniterm.fontSize);
            pen = new Pen(Brushes.SteelBlue, (int)Math.Log(uniterm.fontSize, 3));

            SizeF size = getUnitermSize();
            int margin = uniterm.fontSize * 4;
            int bmpWidth = ((int)(size.Width + margin) > minWidth) ? (int)(size.Width + margin) : minWidth;
            int bmpHeight = ((int)(size.Height + margin) > minHeight) ? (int)(size.Height + margin) : minHeight;

            bitmap = new Bitmap(bmpWidth, bmpHeight);
    
            g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (uniterm.switched != ' ')
            {
                drawSwitched(new Point(uniterm.fontSize + 20, uniterm.fontSize));
            }
            else
            {
                if (uniterm.sA != "")
                {
                    drawSek(new Point(uniterm.fontSize + 20, uniterm.fontSize));
                }
                if (uniterm.eA != "")
                {
                    drawZrown(new Point(uniterm.fontSize + 20, uniterm.fontSize * 7));
                }
            }
            return bitmap;
        }


        private int drawSek(Point p0)
        {
            int textHeight = drawVertText(
                new Point(p0.X + 2, p0.Y), 
                uniterm.sA, uniterm.sOp, uniterm.sB
                );

            drawVertBezier(p0, textHeight);

            return textHeight;
        }

        private int drawZrown(Point p0)
        {
            int textHeight = drawVertText(
                new Point(p0.X + 5, p0.Y),
                uniterm.eA, uniterm.eOp, uniterm.eB
                );

            DrawVert(p0, textHeight);

            return textHeight;
        }

        private void drawSwitched(Point p0)
        {
            if (uniterm.sA == "" || uniterm.sOp == ""
                || uniterm.eA == "" || uniterm.eB == "" || uniterm.eOp == "")
                return;

            int offset = uniterm.fontSize / 3;
            int textHeight = 0;

            if (uniterm.switched == 'A')
            {
                textHeight += drawZrown(new Point(p0.X + offset, p0.Y + offset));
                textHeight += drawVertText(new Point(p0.X + offset - 3, p0.Y + textHeight), 
                    "", uniterm.sOp, uniterm.sB);
            }
            else if (uniterm.switched == 'B')
            {
                textHeight += drawVertText(new Point(p0.X + offset, p0.Y),
                    uniterm.sA, uniterm.sOp, "");
                textHeight += drawZrown(new Point(p0.X + offset, p0.Y + offset + textHeight));
            }

            drawVertBezier(p0, textHeight);
        }


        private void drawVertBezier(Point p0, int length)
        {
            int a = (int)Math.Sqrt(length) + 2;

            Point p1 = new Point()
            {
                X = p0.X - a,
                Y = p0.Y + (int)(length * 0.25)
            };

            Point p2 = new Point()
            {
                X = p0.X - a,
                Y = p0.Y + (int)(length * 0.75)
            };

            Point p3 = new Point()
            {
                X = p0.X,
                Y = p0.Y + length
            };            

            g.DrawBezier(pen, p0, p1, p2, p3);
        }

        private void DrawVert(Point p0, int length)
        {
            int a = (int)Math.Sqrt(length) + 2;

            g.DrawLine(pen, p0, new Point(p0.X, p0.Y + length));
            g.DrawLine(pen, new Point(p0.X, p0.Y), new Point(p0.X + a, p0.Y));
            g.DrawLine(pen, new Point(p0.X, p0.Y + length), new Point(p0.X + a, p0.Y + length));

        }


        private int drawVertText(Point p0, string opA, string opS, string opB)
        {
            string text = opA +
                Environment.NewLine.ToString() +
                opS +
                Environment.NewLine.ToString() +
                opB;

            g.DrawString(text, font, Brushes.Black, p0);

            int textHeight = (int)g.MeasureString(text, font).Height;

            return textHeight;            
        }

        private SizeF getUnitermSize()
        {
            string text = "";
            Bitmap bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);

            if (uniterm.switched == 'A')
            {
                text = uniterm.eA + Environment.NewLine + uniterm.eOp + Environment.NewLine +
                    uniterm.eB + Environment.NewLine + "" + Environment.NewLine + uniterm.sOp + 
                    Environment.NewLine + uniterm.sB;
                
            }
            else if (uniterm.switched == 'B')
            {
                text = uniterm.sA + Environment.NewLine + uniterm.sOp + Environment.NewLine +
                    "" + Environment.NewLine + uniterm.eA + Environment.NewLine + uniterm.eOp +
                    Environment.NewLine + uniterm.eB;
            }
            else
            {
                if (uniterm.sA != "" && uniterm.sB != "")
                {
                    text += uniterm.sA + Environment.NewLine + uniterm.sOp + Environment.NewLine +
                        uniterm.sB + Environment.NewLine;
                }

                if (uniterm.eA != "" && uniterm.eB != "")
                {
                    text += uniterm.eA + Environment.NewLine + uniterm.eOp + Environment.NewLine +
                        uniterm.eB + Environment.NewLine;
                }
            }

            return g.MeasureString(text, font);
        }
    }
}