﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedEdit
{
    public partial class GridBox : PictureBox
    {
        private Color hoverColor = Color.White;
        private Color selectedColor = Color.Red;
        private Size selectionSize = new Size(16, 16);
        private int selectedIndex = -1;
        public bool canSelect = true;
        public bool canHover = true;
        private bool allowMultiSelection = false;
        private Rectangle selectionRectangle = new Rectangle(0, 0, 1, 1);
        private int hoverIndex = -1;
        private int lastHoverIndex = -1;
        private int startSelection = -1;
        private Size canvas = new Size(128, 128);
        private Pen selectionPen = new Pen(Color.Red, 2);
        private InterpolationMode mode = InterpolationMode.NearestNeighbor;

        private Pen chestPen = new Pen(Color.Yellow, 2);
        public Point chestHighlightPoint = new Point(-1, -1);
        public Point listObjectHighlightPoint = new Point(-1, -1);
        public Point warpHighlightPoint = new Point(-1, -1);

        public GridBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public GridBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        [Description("The window's update mode."), Browsable(true)]
        public InterpolationMode InterpolationMode
        {
            get { return mode; }
            set { mode = value; Invalidate(); }
        }

        [Description("The hover color."), Browsable(true)]
        public Color HoverColor
        {
            get { return hoverColor; }
            set { hoverColor = value; Invalidate(); }
        }

        [Description("The selection color."), Browsable(true)]
        public Color SelectionColor
        {
            get { return selectedColor; }
            set { selectedColor = value; selectionPen = new Pen(selectedColor, 2); this.Invalidate(); }
        }

        [Description("The box size."), Browsable(true)]
        public Size BoxSize
        {
            get { return selectionSize; }
            set { selectionSize = value; Invalidate(); }
        }

        [Description("The selected index."), Browsable(true)]
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; selectionRectangle = new Rectangle((value % (canvas.Width / selectionSize.Width)), (value / (canvas.Height / selectionSize.Height)), 1, 1); Invalidate(); }
        }

        [Description("Determines whether or not items can be selected."), Browsable(true)]
        public bool Selectable
        {
            get { return canSelect; }
            set { canSelect = value; Invalidate(); }
        }

        [Description("Determines whether or not the hoverbox will be shown."), Browsable(true)]
        public bool HoverBox
        {
            get { return canHover; }
            set { canHover = value; Invalidate(); }
        }

        [Description("The canvas size."), Browsable(true)]
        public Size CanvasSize
        {
            get { return canvas; }
            set { canvas = value; }
        }

        [Browsable(false)]
        public int HoverIndex
        {
            get { return hoverIndex; }
        }

        [Browsable(false)]
        public Rectangle SelectionRectangle
        {
            get { return selectionRectangle; }
            set { selectionRectangle = value; Invalidate(); }
        }

        [Description("Determines whether or not more than one items can be selected."), Browsable(true)]
        public bool AllowMultiSelection
        {
            get { return allowMultiSelection; }
            set { allowMultiSelection = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = mode;
            try
            {
                base.OnPaint(e);
            }
            catch (Exception) { }
        }

        private void GridBox_Paint(object sender, PaintEventArgs e)
        {
            if (canSelect)
            {
                if (!allowMultiSelection)
                {
                    if (selectedIndex != -1)
                    {
                        Point p = GetIndexPoint(selectedIndex);
                        e.Graphics.DrawRectangle(selectionPen, p.X, p.Y, selectionSize.Width, selectionSize.Height);
                    }
                    //else
                    //{
                    //    Point p = GetIndexPoint(selectedIndex);
                    //    e.Graphics.DrawRectangle(selectionPen, p.X, p.Y, selectionSize.Width * selectionRectangle.Width, selectionSize.Height * selectionRectangle.Height);
                    //}

                    if (chestHighlightPoint.X != -1)
                    {
                        e.Graphics.DrawRectangle(chestPen, chestHighlightPoint.X * 16 + 2, chestHighlightPoint.Y * 16 + 2, 12, 12);
                    }

                    if (listObjectHighlightPoint.X != -1)
                    {
                        e.Graphics.DrawLine(selectionPen, listObjectHighlightPoint.X - 5, listObjectHighlightPoint.Y - 5, listObjectHighlightPoint.X + 5, listObjectHighlightPoint.Y + 5);
                        e.Graphics.DrawLine(selectionPen, listObjectHighlightPoint.X + 5, listObjectHighlightPoint.Y - 5, listObjectHighlightPoint.X - 5, listObjectHighlightPoint.Y + 5);
                    }

                    if (warpHighlightPoint.X != -1)
                    {
                        e.Graphics.DrawLine(selectionPen, warpHighlightPoint.X - 15, warpHighlightPoint.Y, warpHighlightPoint.X + 15, warpHighlightPoint.Y);
                        e.Graphics.DrawLine(selectionPen, warpHighlightPoint.X, warpHighlightPoint.Y - 15, warpHighlightPoint.X, warpHighlightPoint.Y + 15);
                    }
                }

                if (canHover)
                {
                    if (hoverIndex != -1)
                    {
                        Point p = GetIndexPoint(hoverIndex);
                        e.Graphics.DrawRectangle(new Pen(hoverColor), p.X, p.Y, selectionSize.Width - 1, selectionSize.Height - 1);
                    }
                }
            }
        }

        public Point GetIndexPoint(int i)
        {
            int width = (canvas.Width / selectionSize.Width);
            int height = (canvas.Height / selectionSize.Height);
            int x = i % width;
            int y = i / width;
            return new Point(x * selectionSize.Width, y * selectionSize.Height);
        }

        private void GridBox_MouseMove(object sender, MouseEventArgs e)
        {
            int x;
            int y;
            int width;
            int height;
            if (allowMultiSelection && startSelection != -1)
            {
                x = e.X / selectionSize.Width;
                y = e.Y / selectionSize.Height;
                width = x - selectionRectangle.X + 1;
                height = y - selectionRectangle.Y + 1;
                if (width > 0)
                    selectionRectangle.Width = width;
                else
                    selectionRectangle.Width = 1;
                if (height > 0)
                    selectionRectangle.Height = height;
                else
                    selectionRectangle.Height = 1;
                if (selectionRectangle.X + selectionRectangle.Width > canvas.Width / selectionSize.Width)
                    selectionRectangle.Width = (canvas.Width / selectionSize.Width) - selectionRectangle.X;
                if (selectionRectangle.Y + selectionRectangle.Height > canvas.Height / selectionSize.Height)
                    selectionRectangle.Height = (canvas.Height / selectionSize.Height) - selectionRectangle.Y;
                Invalidate();
                return;
            }

            if (e.X < 0 || e.Y < 0 || e.X >= canvas.Width || e.Y >= canvas.Height)
            {
                if (hoverIndex != -1)
                {
                    hoverIndex = -1;
                    lastHoverIndex = -1;
                    Invalidate();
                }
                return;
            }

            width = (canvas.Width / selectionSize.Width);
            height = (canvas.Height / selectionSize.Height);
            x = e.X / selectionSize.Width;
            y = e.Y / selectionSize.Height;
            hoverIndex = x + y * width;

            if (canHover)
            {
                if (lastHoverIndex != hoverIndex)
                {
                    lastHoverIndex = hoverIndex;
                    Invalidate();
                }
            }
        }

        private void GridBox_MouseLeave(object sender, EventArgs e)
        {
            if (canHover)
            {
                hoverIndex = -1;
                lastHoverIndex = -1;
                Invalidate();
            }
        }

        private void GridBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (canSelect && hoverIndex != -1)
                {
                    startSelection = hoverIndex;
                    selectionRectangle = new Rectangle((e.X / selectionSize.Width), (e.Y / selectionSize.Height), 1, 1);
                    selectedIndex = hoverIndex;
                    Invalidate();
                }
            }
        }

        private void GridBox_MouseUp(object sender, MouseEventArgs e)
        {
            startSelection = -1;
        }

        private void GridBox_Resize(object sender, EventArgs e)
        {
            if (Image != null)
            {
                canvas.Width = Image.Width;
                canvas.Height = Image.Height;
                Invalidate();
            }
        }
    }
}
