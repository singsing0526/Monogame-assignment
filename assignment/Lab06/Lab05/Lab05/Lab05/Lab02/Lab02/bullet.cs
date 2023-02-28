using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Lab02
{
    class bullet : DrawableGameComponent
    {
        public Color[] data;
        public Texture2D texture;
        public Vector2 position, center, velocity;
        public float rotateAngle, rotateSpeed;
        static Random h = new Random();
        SpriteBatch _spriteBatch;
        public bool hit;

        public bullet(Game g) : base(g)
        {
            hit = false;
        }

        public override void Initialize()
        {
            position.X = h.Next(GraphicsDevice.Viewport.Width);
            position.Y = 0;
            velocity.X = 0;
            velocity.Y = (float)(h.NextDouble() * 2 + 0.4f);
            rotateSpeed = velocity.Y / 10;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Game.Content.Load<Texture2D>("Images\\buttet");
            center.X = texture.Width / 2;
            center.Y = texture.Height / 2;
            //base.LoadContent();
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);

        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
            rotateAngle = (rotateAngle + rotateSpeed) % MathHelper.TwoPi;
            if (position.Y > 225)
            {
                Initialize();
                //hit = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, null, Color.White, rotateAngle, center, 1, SpriteEffects.None, 0.5f);
            _spriteBatch.End();
            base.Draw(gameTime);

        }


    }
}