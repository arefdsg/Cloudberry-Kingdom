﻿using Microsoft.Xna.Framework;

using CloudberryKingdom.Levels;

namespace CloudberryKingdom
{
    public class OutsideBackground : Background
    {
        public QuadClass BackgroundQuad;
        
        public OutsideBackground()
        {
            MyType = BackgroundType.Outside;
            MyTileSet = TileSets.Terrace;
        }

        public override void Init(Level level)
        {
            MyLevel = level;
            MyCollection = new BackgroundCollection(MyLevel);

            BackgroundQuad = new QuadClass();

            BackgroundQuad.Quad.SetColor(new Color(255, 255, 255));
            BackgroundQuad.Quad.MyTexture = Tools.TextureWad.FindByName(InfoWad.GetStr("OutsideBackground"));
            BackgroundQuad.Quad.MyEffect = Tools.BasicEffect;
        }

        public static void AddVerticalSpan(Vector2 BL, Vector2 TR, Background b)
        {
            float ModCloudSpeed = .2f;

            TR.X += 900;
            BL.X -= 450;

            //base.AddSpan(BL, TR);

            BackgroundFloaterList NewList;
            float Pos;

            NewList = new BackgroundFloaterList();
            NewList.Parralax = .6f;
            Pos = BL.Y;
            while (Pos < TR.Y)
            {
                BackgroundFloater cloud = new BackgroundFloater(b.MyLevel, BL.X - 1700, TR.X + 400);
                cloud.Data.Position = new Vector2(b.Rnd.RndFloat(-1500, 1500)*2.3f, Pos);
                cloud.MyQuad.TextureName = "cloud" + b.Rnd.RndInt(1, 4).ToString();

                cloud.MyQuad.Quad.SetColor(Tools.Gray(.945f));
                cloud.MyQuad.Size = new Vector2(300, 200);
                cloud.Data.Velocity = new Vector2(-55, 0) * ModCloudSpeed;

                Pos += b.Rnd.RndFloat(800, 1400) * .7f;

                NewList.Floaters.Add(cloud);
            }
            b.MyCollection.Lists.Add(NewList);

            NewList = new BackgroundFloaterList();
            NewList.Parralax = .8f;
            Pos = BL.Y;
            while (Pos < TR.Y)
            {
                BackgroundFloater cloud = new BackgroundFloater(b.MyLevel, BL.X - 1700, TR.X + 400);
                cloud.Data.Position = new Vector2(b.Rnd.RndFloat(-1800, 1800)*2.3f, Pos);
                cloud.MyQuad.TextureName = "cloud" + b.Rnd.RndInt(1, 4).ToString(); 
                
                cloud.MyQuad.Size = new Vector2(300, 200);
                cloud.Data.Velocity = new Vector2(-55, 0) * ModCloudSpeed;

                Pos += b.Rnd.RndFloat(800, 1400) * .7f;

                NewList.Floaters.Add(cloud);
            }
            b.MyCollection.Lists.Add(NewList);

            b.MyCollection.SetLevel(b.MyLevel);
        }
        
        public override void AddSpan(Vector2 BL, Vector2 TR)
        {
            if (MyLevel.Geometry == LevelGeometry.Up || MyLevel.Geometry == LevelGeometry.Down)
            {
                AddHorizontalSpan(BL + new Vector2(-7000, 0), TR + new Vector2(7000, 0));
                MyCollection.Lists[0].Clear();
                MyCollection.Lists[2].Clear();
                AddVerticalSpan(BL + new Vector2(0, 4000), TR + new Vector2(0, 4000), this);
            }
            else
                AddHorizontalSpan(BL, TR);
        }

        private void AddHorizontalSpan(Vector2 BL, Vector2 TR)
        {
            TR.X += 900;
            BL.X -= 450;

            base.AddSpan(BL, TR);

            MyCollection.FromInfoWad("Outside", BL, TR, MyLevel);
        }

        public override void Draw()
        {
            Camera Cam = MyLevel.MainCamera;

            MyCollection.PhsxStep();

            base.Draw();            

            BackgroundQuad.Quad.SetColor(new Color(new Vector3(1, 1, 1) * Light));
            BackgroundQuad.FullScreen(Cam);

            Cam.SetVertexCamera();
            //BackgroundQuad.Base.Origin = Cam.EffectivePos;


            BackgroundQuad.Draw();

            MyCollection.Draw();

            Tools.QDrawer.Flush();

            //Tools.EffectWad.SetCameraPosition(HoldCameraPos);
        }
    }
}
