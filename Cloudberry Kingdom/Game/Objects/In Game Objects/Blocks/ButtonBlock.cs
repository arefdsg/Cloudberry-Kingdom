﻿using Microsoft.Xna.Framework;
using Drawing;
using System.IO;
using CloudberryKingdom.Bobs;

namespace CloudberryKingdom.Blocks
{
    public delegate void ButtonCallback();
    public class ButtonBlock : BlockBase
    {
        SimpleQuad MyQuad;
        BasePoint Base;

        public int TypeIndex;
        public bool Correlate;

        public ButtonCallback HitCallback;
        
        public ButtonBlock(ButtonBlock block)
        {
            Active = block.Active;

            BlockCore.MyType = block.BlockCore.MyType;

            MyBox = new AABox(block.Box.Current.Center, block.Box.Current.Size);

            BlockCore.Data = block.BlockCore.Data;
            BlockCore.StartData = block.BlockCore.StartData;

            BlockCore.Layer = block.BlockCore.Layer;

            Base = block.Base;

            MyQuad.Init();
            MyQuad.MyEffect = block.MyQuad.MyEffect;
            MyQuad.MyTexture = block.MyQuad.MyTexture;

            Correlate = block.Correlate;
            TypeIndex = block.TypeIndex;
            Update();
        }

        public ButtonBlock(Vector2 Pos, Vector2 Size)
        {
            Active = true;
            
            BlockCore.MyType = ObjectType.Button;
                        

            Vector2 BoxSize = Size; BoxSize.Y = BoxSize.X * .6f;
            Vector2 BoxPos = Pos; BoxPos.Y -= BoxSize.X * .6f;
            MyBox = new AABox(BoxPos, BoxSize);
            
            BlockCore.Data.Position = BoxPos;
            BlockCore.StartData.Position = BoxPos;

            BlockCore.Layer = 1;

            Base = new BasePoint();
            Base.e1 = new Vector2(Size.X, 0);
            Base.e2 = new Vector2(0, Size.Y);
            //Base.Origin = Pos + new Vector2(0, -12);

            MyQuad.Init();
            MyQuad.MyEffect = Tools.BasicEffect;
            MyQuad.MyTexture = Tools.TextureWad.FindByName("Button");

            Correlate = true;
            TypeIndex = 0;
            Update();
        }

        public void Update()
        {
            Color color = Globals.OnOffBlockColors[TypeIndex];
            if (Active) MyQuad.MyTexture = Tools.TextureWad.FindByName("Button");
            else MyQuad.MyTexture = Tools.TextureWad.FindByName("ButtonDown");
            MyQuad.SetColor(color);

            Base.Origin = Box.Current.Center + new Vector2(0, 24);
        }

        public override void PhsxStep()
        {
            bool HoldActive = Active;

            if (Correlate) Active = !Globals.ColorSwitch[TypeIndex];
            else Active = Globals.ColorSwitch[TypeIndex];

            if (HoldActive != Active) Update();

            Box.SetTarget(BlockCore.Data.Position, Box.Current.Size);
        }

        public override void PhsxStep2()
        {
            Box.SwapToCurrent();
        }

        public override void Draw()
        {
            MyQuad.Update(ref Base);
            Tools.QDrawer.DrawQuad(ref MyQuad);

            if (Tools.DrawBoxes)
            {
                Box.Draw(Tools.QDrawer, Color.Olive, 15);
                Box.DrawT(Tools.QDrawer, Color.Olive, 15);
            }
        }

        public override void Move(Vector2 shift)
        {
            BlockCore.Data.Position += shift;
            BlockCore.StartData.Position += shift;

            Box.Move(shift);

            Update();
        }

        public BlockBase Clone() { return new ButtonBlock(this); }

        public override void LandedOn(Bob bob)
        {
            if (HitCallback != null)
                HitCallback();

            Active = false;
            if (Correlate) Globals.ColorSwitch[TypeIndex] = true;
            else Globals.ColorSwitch[TypeIndex] = false;

            bob.Core.Data.Velocity.Y = 4;

            bob.MyPhsx.LandOnSomething(true, this);

            Update();
        }

        public override void Reset(bool BoxesOnly)
        {
            BlockCore.BoxesOnly = BoxesOnly;

            Active = true;

            BlockCore.Data = BlockCore.StartData;

            MyBox.Current.Center = BlockCore.StartData.Position;
            MyBox.SetTarget(MyBox.Current.Center, MyBox.Current.Size);
            MyBox.SwapToCurrent();

            Update();
        }
    }
}