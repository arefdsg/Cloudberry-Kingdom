﻿using System;
using System.IO;

using Microsoft.Xna.Framework;

using CloudberryKingdom.Spikes;
using CloudberryKingdom.Bobs;
using CloudberryKingdom.Levels;

namespace CloudberryKingdom.Blocks
{
    public class Wall : BlockBase, Block
    {
        bool Horizontal;
        public static Wall MakeWall(LevelGeometry geometry)
        {
            Wall wall = new Wall(false); wall.MakeNew();
            wall.Horizontal = geometry == LevelGeometry.Right;

            wall.Init();
            wall.StampAsUsed(0);
            wall.Speed = 15;
            wall.InitialDelay = 78;


            if (!wall.Horizontal)
            {
                wall.MoveBack(new Vector2(0, 30000));
                wall.Speed *= .7f;
            }

            return wall;
        }

        bool Spiked = false;
        public void Spikify()
        {
            Spiked = true;

            if (Horizontal)
            {
                //MoveBack(new Vector2(320, 0));
                MoveBack(new Vector2(-90, 0));
            }
            else
            {
                MoveBack(new Vector2(0, -90));
            }

            // Spikes
            int count = 0;
            if (Horizontal)
            {
                for (float y = Box.BL.Y; y < Box.TR.Y; y += 100)
                {
                    MakeSpike(count, y);

                    count++;
                }
            }
            else
            {
                for (float x = Box.BL.X; x < Box.TR.X; x += 100)
                {
                    MakeSpike(count, x);

                    count++;
                }
            }
        }

        private void MakeSpike(int count, float pos)
        {
            Spike spike = (Spike)Core.MyLevel.Recycle.GetObject(ObjectType.Spike, false);


            if (Horizontal)
            {
                float SpikeSideOffset = InfoWad.GetFloat("Spike_SideOffset");
                float x = Box.Target.TR.X + SpikeSideOffset;
                spike.SetDir(3);
                spike.Core.Data.Position = new Vector2(x, pos);
            }
            else
            {
                float SpikeTopOffset = InfoWad.GetFloat("Spike_TopOffset");
                float y = Box.Target.TR.Y + SpikeTopOffset;
                spike.SetDir(0);
                spike.Core.Data.Position = new Vector2(pos, y);
            }

            
            spike.SetPeriod(100);

            spike.SetPeriod(50);
            spike.Offset = count % 2 == 0 ? 0 : 50 / 2;

            spike.SetParentBlock(this);
            Core.MyLevel.AddObject(spike);
        }

        public float Speed = 16.5f;
        public float Accel = .2f;
        public int InitialDelay = 60;

        public void TextDraw() { }

        public NormalBlockDraw MyDraw;

        public void MakeNew()
        {
            BlockCore.Init();
            CoreData.MyType = ObjectType.Wall;
            Core.DrawLayer = 9;

            Active = false;

            CoreData.Layer = .7f;

            Core.RemoveOnReset = false;
            CoreData.HitHead = true;
            CoreData.NoComputerTouch = true;

            Core.EditHoldable = Core.Holdable = true;
        }

        public void Release()
        {
            Core.MyLevel = null;

            MyDraw.Release();
            MyDraw = null;

            MyBox = null;
        }

        public Wall(bool BoxesOnly)
        {
            CoreData = new BlockData();

            MyBox = new AABox();
            MyDraw = new NormalBlockDraw();

            MakeNew();

            Core.BoxesOnly = BoxesOnly;
        }

        public void ResetPieces()
        {
            MyDraw.Init(this, PieceQuad.Castle);
        }

        public float StartOffset = -400;
        public void Init()
        {
            Vector2 size, center;

            if (Horizontal)
            {
                size = new Vector2(2000 * 3, 2000);
                center = -new Vector2(size.X, 0);
            }
            else
            {
                size = new Vector2(2000, 2000 * 3);
                center = -new Vector2(0, size.Y);
            }

            MyBox.Initialize(center, size);
            Core.Data.Position = BlockCore.Data.Position = BlockCore.StartData.Position = center;

            if (!Core.BoxesOnly)
                ResetPieces();

            Update();
        }

        public void MoveBack(Vector2 shift)
        {
            Core.Data.Position += shift;
            if (Horizontal)
                StartOffset += shift.X;
            else
                StartOffset += shift.Y;

            Box.Move(shift);

            Update();
        }

        public void Move(Vector2 shift)
        {
            BlockCore.Data.Position += shift;
            BlockCore.StartData.Position += shift;
            StartOffset += shift.X;

            Box.Move(shift);

            Update();
        }

        public void Hit(Bob bob) { }
        public void LandedOn(Bob bob) { }
        public void HitHeadOn(Bob bob) { } public void SideHit(Bob bob) { } 

        public void Reset(bool BoxesOnly)
        {
            CoreData.BoxesOnly = BoxesOnly;

            if (!Core.BoxesOnly)
                ResetPieces();

            Core.Data = BlockCore.Data = BlockCore.StartData;
            if (Horizontal)
                Core.Data.Position += new Vector2(StartOffset, 0);
            else
                Core.Data.Position += new Vector2(0, StartOffset);

            MyBox.Current.Center = Core.Data.Position;
            MyBox.SetTarget(MyBox.Current.Center, MyBox.Current.Size);
            MyBox.SwapToCurrent();

            Update();

            Active = false;
        }

        float ShakeIntensity = 25.5f, CurShakeIntensity;
        float MinShakeIntensity = 7.5f;
        float ShakeLength = 50;
        Vector2 Offset;
        void Shake()
        {
            int Step = Core.MyLevel.CurPhsxStep;
            if (Step < ShakeLength)
                CurShakeIntensity = ShakeIntensity;
            else
                CurShakeIntensity *= .98f;
            Tools.Restrict(MinShakeIntensity, ShakeIntensity, ref CurShakeIntensity);

            int Wait = 2;
            if (CurShakeIntensity < MinShakeIntensity + 2)
                Wait = 4;

            if (Step % Wait == 0)
                Offset = new Vector2(Tools.RndFloat(-CurShakeIntensity, CurShakeIntensity),
                                     Tools.RndFloat(-CurShakeIntensity, CurShakeIntensity));
        }

        Vector2 CalcPosition(float t)
        {
            if (t < InitialDelay) return Core.Data.Position;

            if (Horizontal)
                Core.Data.Velocity.X =
                    Tools.Restrict(0, Speed, Core.Data.Velocity.X + Accel);
            else
                Core.Data.Velocity.Y =
                    Tools.Restrict(0, Speed, Core.Data.Velocity.Y + Accel);

            return Core.Data.Position + Core.Data.Velocity;
            //return BlockCore.StartData.Position + new Vector2(t * Speed, 0);
        }

        public enum BufferType { Push, Space };
        public BufferType MyBufferType = BufferType.Space;
        public float Space = 40;

        public void DoInteraction(Bob bob)
        {
            bob.Box.CalcBounds();
            float dif;
            Vector2 difvec;

            if (Horizontal)
            {
                dif = Box.Current.TR.X - 10 - bob.Box.BL.X;
                difvec = new Vector2(dif, 0);
            }
            else
            {
                dif = Box.Current.TR.Y - 10 - bob.Box.BL.Y;
                difvec = new Vector2(0, dif);
            }


            switch (MyBufferType)
            {
                case BufferType.Push:
                    if (dif > 0) bob.Move(difvec); break;

                case BufferType.Space:
                    if (Core.MyLevel.PlayMode != 2)
                    {
                        if (dif > 0) bob.Move(difvec);
                    }
                    else
                    {
                        dif += Space;
                        if (dif > 0) MoveBack(-difvec);
                    }
                    break;
            }
        }

        public void PhsxStep()
        {
            //Vector2 DesireCamPos = new Vector2(Box.TR.X + 800, Core.MyLevel.MainCamera.Pos.Y);
            //if (DesireCamPos.X > Core.MyLevel.MainCamera.Pos.X)
            //    Core.MyLevel.MainCamera.Pos = DesireCamPos;

            if (Core.MyLevel.PlayMode == 0)
            {
                if (!Spiked) Spikify();
                Shake();
            }

            Core.SkippedPhsx = false;
            Active = true;
            
            if (!Core.Held)
                Core.Data.Position = CalcPosition(Core.GetPhsxStep());

            MyBox.Target.Center = Core.Data.Position + Offset;

            Update();

            MyBox.SetTarget(MyBox.Target.Center, MyBox.Current.Size);

            MyBox.CalcBounds();
            foreach (Bob bob in Core.MyLevel.Bobs)
                DoInteraction(bob);
        }

        public void PhsxStep2()
        {
            if (!Active) return;

            MyBox.SwapToCurrent();
        }


        public void Update()
        {
            if (CoreData.BoxesOnly) return;

            MyDraw.Update();
        }

        public void Draw()
        {
            Update();

            if (Tools.DrawBoxes)
                MyBox.Draw(Tools.QDrawer, Color.Olive, 15);

            if (Tools.DrawGraphics)
            {
                if (!CoreData.BoxesOnly)
                {
                    MyDraw.Draw();
                    Tools.QDrawer.Flush();
                }

                BlockCore.Draw();
            }
        }

        public void Extend(Side side, float pos)
        {
            MyBox.Invalidated = true;

            MyBox.Extend(side, pos);

            Update();

            if (!Core.BoxesOnly)
                ResetPieces();

            CoreData.StartData.Position = MyBox.Current.Center;

            ResetPieces();
        }

        public void Interact(Bob bob) { }
        
        public void Clone(IObject A)
        {
            Wall BlockA = A as Wall;

            Init();

            Core.Clone(A.Core);

            Speed = BlockA.Speed;
        }

        public void Write(BinaryWriter writer)
        {
            BlockCore.Write(writer);
        }
        public void Read(BinaryReader reader) { Core.Read(reader); }
//StubStubStubStart
public void OnUsed() { }
public void OnMarkedForDeletion() { }
public void OnAttachedToBlock() { }
public bool PermissionToUse() { return true; }
public Vector2 Pos { get { return Core.Data.Position; } set { Core.Data.Position = value; } }
public GameData Game { get { return Core.MyLevel.MyGame; } }
public void Smash(Bob bob) { }
public bool PreDecision(Bob bob) { return false; }
//StubStubStubEnd7
    }
}
