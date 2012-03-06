﻿using System.IO;

using Microsoft.Xna.Framework;

using CloudberryKingdom.Blocks;
using CloudberryKingdom.Bobs;

namespace CloudberryKingdom
{
    public enum FallingBlockState { Regular, Touched, Falling, Angry };
    public class FallingBlock : BlockBase, Block
    {
        public void TextDraw() { }

        public bool TouchedOnce, HitGround;

        public AABox MyBox;

        public bool Thwomp;
        public Vector2 AngryAccel;
        public float AngryMaxSpeed;

        public int StartLife, Life;
        //public int StartLife { get { return _StartLife; } set { _StartLife = value; if (value > 20) Console.WriteLine("!");  } }

        public QuadClass MyQuad;
        FallingBlockState State;
        bool EmittedExplosion;
        public Vector2 Offset;
        int ResetTimer;
        public static int ResetTimerLength = 12;

        public AABox Box { get { return MyBox; } }

        public bool Active;
        public bool IsActive { get { return Active; } set { Active = value; } }

        public BlockData CoreData;
        public BlockData BlockCore { get { return CoreData; } }
        public ObjectData Core { get { return CoreData as BlockData; } }
        public void Interact(Bob bob) { }

        public void MakeNew()
        {
            Thwomp = false;

            EmittedExplosion = HitGround = false;

            Core.Init();
            Core.DrawLayer = 3;
            CoreData.MyType = ObjectType.FallingBlock;

            SetState(FallingBlockState.Regular);
        }

        public void Release()
        {
            BlockCore.Release();
            Core.MyLevel = null;
            MyQuad = null;
            MyBox = null;
        }

        public void SetState(FallingBlockState NewState) { SetState(NewState, false); }
        public void SetState(FallingBlockState NewState, bool ForceSet)
        {
            if (State != NewState || ForceSet)
            {
                switch (NewState)
                {
                    case FallingBlockState.Regular:
                        TouchedOnce = HitGround = false;
                        MyQuad.Quad.MyTexture = Tools.TextureWad.FindByName(InfoWad.GetStr("FallingBlock_Regular_Texture"));
                        break;
                    case FallingBlockState.Touched:
                        HitGround = false;
                        MyQuad.Quad.MyTexture = Tools.TextureWad.FindByName(InfoWad.GetStr("FallingBlock_Touched_Texture"));
                        break;
                    case FallingBlockState.Falling:
                        MyQuad.Quad.MyTexture = Tools.TextureWad.FindByName(InfoWad.GetStr("FallingBlock_Falling_Texture"));
                        break;
                    case FallingBlockState.Angry:
                        MyQuad.Quad.MyTexture = Tools.TextureWad.FindByName(InfoWad.GetStr("FallingBlock_Angry_Texture"));
                        break;
                }
            }

            State = NewState;
        }

        public FallingBlock(bool BoxesOnly)
        {
            CoreData = new BlockData();

            MyQuad = new QuadClass();

            MyBox = new AABox();

            MakeNew();

            Core.BoxesOnly = BoxesOnly;
        }

        public void Init(Vector2 center, Vector2 size, int life)
        {
            Active = true;

            Life = StartLife = life;

            CoreData.Layer = .35f;
            MyBox = new AABox(center, size);
            MyQuad.Base.Origin = CoreData.Data.Position = CoreData.StartData.Position = center;

            MyBox.Initialize(center, size);

            SetState(FallingBlockState.Regular, true);
            MyQuad.Base.e1.X = size.X;
            MyQuad.Base.e2.Y = size.Y;

            Update();
        }

        public void Hit(Bob bob) { }
        public void LandedOn(Bob bob)
        {
            // Don't register as a land if the Bob is moving upward.
            if (bob.Core.Data.Velocity.Y > 3)
            {
                BlockCore.StoodOn = false;
                return;
            }

            if (State == FallingBlockState.Regular)
            {
                //if (Thwomp)
                  //  SetState(FallingBlockState.Angry);
                //else
                    SetState(FallingBlockState.Touched);
            }

            if (bob.Core.Data.Velocity.Y < -10)
                Life -= 8;
        }
        public void HitHeadOn(Bob bob) { } public void SideHit(Bob bob) { } 

        public void Reset(bool BoxesOnly)
        {
            CoreData.BoxesOnly = BoxesOnly;

            Active = true;

            Life = StartLife;
            TouchedOnce = false;
            EmittedExplosion = false;

            ResetTimer = 0;

            SetState(FallingBlockState.Regular, true);

            CoreData.Data = CoreData.StartData;

            BlockCore.StoodOn = false;

            MyBox.Current.Center = CoreData.StartData.Position;

            MyBox.SetTarget(MyBox.Current.Center, MyBox.Current.Size);
            MyBox.SwapToCurrent();

            Update();
        }

        public void PhsxStep()
        {
            Active = Core.Active = true;
            if (!Core.Held)
            {
                if (MyBox.Current.BL.X > CoreData.MyLevel.MainCamera.TR.X || MyBox.Current.BL.Y > CoreData.MyLevel.MainCamera.TR.Y)
                    Active = Core.Active = false;
                if (MyBox.Current.TR.X < CoreData.MyLevel.MainCamera.BL.X || MyBox.Current.TR.Y < CoreData.MyLevel.MainCamera.BL.Y - 200)
                    Active = Core.Active = false;
            }


            if (Core.MyLevel.GetPhsxStep() % 2 == 0)
                Offset = Vector2.Zero;

            // Update the block's apparent center according to attached objects
            BlockCore.UseCustomCenterAsParent = true;
            BlockCore.CustomCenterAsParent = Box.Target.Center + Offset;

            if (CoreData.StoodOn)
            {
                ResetTimer = ResetTimerLength;

                TouchedOnce = true;

                if (Core.MyLevel.GetPhsxStep() % 2 == 0)
                    if (Life > 0)
                    {
                        Offset = new Vector2(Tools.Rnd.Next(-10, 10), Tools.Rnd.Next(-10, 10));
                    }
            }
            else
            {
                if (State == FallingBlockState.Touched)
                {
                    ResetTimer--;
                    if (ResetTimer <= 0)
                        SetState(FallingBlockState.Regular);
                }
            }

            if (State == FallingBlockState.Angry)
            {
                CoreData.Data.Velocity.Y += AngryAccel.Y;
                if (CoreData.Data.Velocity.Y > AngryMaxSpeed) CoreData.Data.Velocity.Y = AngryMaxSpeed;
                MyBox.Current.Center.Y += CoreData.Data.Velocity.Y;
            }
            else
            {
                if (CoreData.StoodOn || Life < StartLife / 3) Life--;
                if (Life <= 0)
                {
                    if (State != FallingBlockState.Falling)
                    {
                        if (Thwomp)
                            SetState(FallingBlockState.Angry);
                        else
                            SetState(FallingBlockState.Falling);
                    }
                    CoreData.Data.Velocity.Y -= 1;
                    if (CoreData.Data.Velocity.Y < -20) CoreData.Data.Velocity.Y = -20;
                    MyBox.Current.Center.Y += CoreData.Data.Velocity.Y;
                }

                // Check for hitting bottom of screen
                if (State == FallingBlockState.Falling && 
                    (Core.MyLevel.Geometry != LevelGeometry.Up && Core.MyLevel.Geometry != LevelGeometry.Down))
                {
                    if (MyBox.Current.Center.Y < CoreData.MyLevel.MainCamera.BL.Y - 200)
                    {
                        // Emit a dust plume if the game is in draw mode and there isn't any lava
                        if (Core.MyLevel.PlayMode == 0 && !EmittedExplosion &&
                            !Tools.CurGameData.HasLava)
                        {
                            EmittedExplosion = true;
                            ParticleEffects.DustCloudExplosion(Core.MyLevel, MyBox.Current.Center);
                        }
                    }
                    if (MyBox.Current.Center.Y < CoreData.MyLevel.MainCamera.BL.Y - 500) Active = false;
                }
            }
            Update();

            MyBox.SetTarget(MyBox.Current.Center, MyBox.Current.Size);

            CoreData.StoodOn = false;
        }

        public void PhsxStep2()
        {
            if (!Active) return;

            MyBox.SwapToCurrent();
        }


        public void Update()
        {
            if (CoreData.BoxesOnly) return;
        }

        public void Extend(Side side, float pos)
        {
            switch (side)
            {
                case Side.Left:
                    MyBox.Target.BL.X = pos;
                    break;
                case Side.Right:
                    MyBox.Target.TR.X = pos;
                    break;
                case Side.Top:
                    MyBox.Target.TR.Y = pos;
                    break;
                case Side.Bottom:
                    MyBox.Target.BL.Y = pos;
                    break;
            }

            MyBox.Target.FromBounds();
            MyBox.SwapToCurrent();

            Update();

            CoreData.StartData.Position = MyBox.Current.Center;
        }

        public void Move(Vector2 shift)
        {
            BlockCore.Data.Position += shift;
            BlockCore.StartData.Position += shift;

            Box.Move(shift);

            Update();
        }
        public void Draw()
        {
            bool DrawSelf = true;
            if (!Core.Held)
            {
                if (!Active) DrawSelf = false;

                if (!Core.MyLevel.MainCamera.OnScreen(MyBox.Current.Center, 600))
                //if (!Core.MyLevel.MainCamera.OnScreen(Core.Data.Position, 600))
                    DrawSelf = false;
            }

            if (DrawSelf)
            {
                Update();

                if (Tools.DrawBoxes)
                    MyBox.Draw(Tools.QDrawer, Color.Olive, 15);
            }

            if (Tools.DrawGraphics)
            {
                //if (State != FallingBlockState.Falling) return;

                if (DrawSelf && !CoreData.BoxesOnly)
                if (!CoreData.BoxesOnly)
                {
                    MyQuad.Base.Origin = MyBox.Current.Center + Offset;
                    MyQuad.Draw();
                }

                BlockCore.Draw();
            }
        }

        public void Clone(IObject A)
        {
            Core.Clone(A.Core);

            FallingBlock BlockA = A as FallingBlock;

            Init(BlockA.Box.Current.Center, BlockA.Box.Current.Size, BlockA.StartLife);

            TouchedOnce = BlockA.TouchedOnce;
            StartLife = BlockA.StartLife;
            Life = BlockA.Life;

            EmittedExplosion = BlockA.EmittedExplosion;

            Thwomp = BlockA.Thwomp;
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
