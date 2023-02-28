using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Lab02
{
    class RunningMan : DrawableGameComponent
    {
        public Color [] data;
        public Texture2D texture;
        public Vector2 position, velocity;
        int frameCount, currentFrame;
        float frameElapsedTime, frameTimeStep;
        public Rectangle frameRect;
        SpriteBatch _spriteBatch;

        public const float MAN_SPEED = 4.5f;

        public Color manColor = Color.White;
       
        public RunningMan(Game g) : base(g) { }

        public override void Initialize()
        {
            position.X = GraphicsDevice.Viewport.Width / 2;
            position.Y = 160;
            velocity.X = 0.0f;
            velocity.Y = 0;
            frameCount = 10;
            frameTimeStep = 1000 / 25f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("Images\\plane");
            frameRect = new Rectangle(0,0, texture.Width, texture.Height);
            //base.LoadContent();
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);

        }

        public override void Update(GameTime gameTime)
        {
            // increment elapsed frame time
            frameElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            // is it time to move on to the next frame?
            position += velocity;
            //if (frameElapsedTime >= frameTimeStep)
            //{
            //    position += velocity;
            //    if (velocity.X != 0)
            //        currentFrame = (currentFrame + 1) % frameCount;
            //    // update the frame rectangle (only x-coordinate needed)
            //    frameRect.X = currentFrame * frameRect.Width;
            //    frameElapsedTime = 0; // reset the elapsed counter
            //                          // checking for screen edge
            //}
            if (position.X > GraphicsDevice.Viewport.Width)
                position.X = 0;
            if (position.X < 0)
                position.X = GraphicsDevice.Viewport.Width;
            base.Update(gameTime);
        }

        SpriteEffects direction = SpriteEffects.None;

        public override void Draw(GameTime gameTime)
        {
            //if(velocity.X <0)direction = SpriteEffects.FlipHorizontally;
            //else if(velocity.X > 0) direction = SpriteEffects.None;


             _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position,     // position of running man
                frameRect, manColor, 0, Vector2.Zero, 1, direction, 0); // the frame rectangle of source texture
            _spriteBatch.Draw(texture, new Vector2(position.X - GraphicsDevice.Viewport.Width, position.Y), frameRect, manColor, 0, Vector2.Zero, 1, direction, 0);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}

