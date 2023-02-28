using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Lab02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D bgTexture, cursor, hpicon,bull;

        Rock[] rocks;
        heal[] heals;
        bullet[] bullets;
        private RunningMan character;

        SpriteFont font;

        int score,life;
        bool iswin = false;

        private bool isGameOver;
        //private bool win;
        private double lasCollistionTime;
        private const int dealy = 900;

        SoundEffect gunshot,explode;
        Song bgmusic;


        ButtonState LastMouseButtonState = ButtonState.Released;
        Vector2 mouse;
        bool clicked;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.TargetElapsedTime = TimeSpan.FromMilliseconds(1000/25f); //this.IsFixedTimeStep = false;           
        }

        protected override void Initialize()
        {
            //rock
                rocks = new Rock[4];
                for (int i = 0; i < 4; i++)
                {
                    rocks[i] = new Rock(this);
                    Components.Add(rocks[i]);
                }

            //bullet
                bullets = new bullet[2];

                for (int i = 0; i <2; i++)
                {
                    bullets[i] = new bullet(this);
                    Components.Add(bullets[i]);
                }

            //heals
            heals = new heal[2];
            for (int i = 0; i < 2; i++)
            {
                heals[i] = new heal(this);
                Components.Add(heals[i]);
            }

            character = new RunningMan(this);
            Components.Add(character);// TODO: Add your initialization logic here

            score = 0;
            life = 10;
            isGameOver = false;
            //win = false;
            base.Initialize();
            IsMouseVisible = false;
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            bgTexture = Content.Load<Texture2D>("Images\\bgi");
            cursor = Content.Load<Texture2D>("Images\\arrow_l");
            hpicon = Content.Load<Texture2D>("Images\\hp");
            bull = Content.Load<Texture2D>("Images\\buttet");

            //scream = Content.Load<SoundEffect>("Sounds//scream");
            explode = Content.Load<SoundEffect>("Sounds//explode");
            gunshot = Content.Load<SoundEffect>("Sounds//gunshot");
            bgmusic = Content.Load<Song>("Sounds//bgm");

            MediaPlayer.Play(bgmusic);
            MediaPlayer.IsRepeating = true;



            font = Content.Load<SpriteFont>("MyFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (CheckCollision()) {
                character.manColor = Color.Red;
                if(gameTime.TotalGameTime.TotalMilliseconds - lasCollistionTime > dealy)
                {
                    if(life > 0)
                    {
                        lasCollistionTime = gameTime.TotalGameTime.TotalMilliseconds;
                        life --;
                    }
                }
            }

            else {
                character.manColor = Color.White;
            }

            //rock
            foreach(Rock r in rocks)
            {

                if(r.hit) score ++;
                r.hit = false;

            }

            //bullet
            foreach (bullet b in bullets)
            {

                if (b.hit) score ++;
                b.hit = false;
            }

            //heal
            foreach (heal h in heals)
            {

                if (h.hit) life++;
                h.hit = false;

            }


            if (life > 0) {

                base.Update(gameTime);
                if (score >=100)
                {
                    iswin = true;
                }
            }
            else
            {
                isGameOver = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(bgTexture,
                GraphicsDevice.Viewport.Bounds, Color.White);

            _spriteBatch.DrawString(font, "Score : " + score, new Vector2(20, GraphicsDevice.Viewport.Height - 60), Color.White);
            _spriteBatch.DrawString(font, "Try to get 100 score!", new Vector2(250, 300), Color.White);


            string msg = "HP: " + life;
            _spriteBatch.Draw(hpicon,
                new Vector2(
                GraphicsDevice.Viewport.Width - font.MeasureString(msg).X - 90, 400), Color.White);
            _spriteBatch.DrawString(font, msg, new Vector2(
                GraphicsDevice.Viewport.Width - font.MeasureString(msg).X - 20, 400), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);

            _spriteBatch.Begin();

            if (isGameOver == true)
            {
                string gameover = " GAME OVER !!";
                _spriteBatch.DrawString(font, gameover, new Vector2(GraphicsDevice.Viewport.Width / 2 - font.MeasureString(gameover).X / 2, GraphicsDevice.Viewport.Height / 2 - font.MeasureString("A").Y / 2), Color.Red);
            }
            if (iswin == true)
            {
                string win = " YOU WIN!!";
                _spriteBatch.DrawString(font, win, new Vector2(GraphicsDevice.Viewport.Width / 2 - font.MeasureString(win).X / 2, GraphicsDevice.Viewport.Height / 2 - font.MeasureString("A").Y / 2), Color.Red);
            }

            _spriteBatch.Draw(cursor, mouse, Color.White);

            _spriteBatch.End();

            UpdateInput();

            base.Draw(gameTime);
        }

        public static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
        {

            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private bool CheckCollision()
        {

            Rectangle personRect = new Rectangle((int)character.position.X, (int)character.position.Y, 
                                                    character.frameRect.Width, character.frameRect.Height);
            Matrix personTransform = Matrix.CreateTranslation(new Vector3(character.position, 0));

            Rectangle personRect2 = new Rectangle((int)character.position.X, (int)character.position.Y,
                                                    character.frameRect.Width, character.frameRect.Height);
            Matrix personTransform2 = Matrix.CreateTranslation(personRect2.X, personRect2.Y, 0);

            foreach (Rock r in rocks)
            {
                Rectangle rockRect = new Rectangle(0,0, r.texture.Width, r.texture.Height);
                Matrix transform = Matrix.CreateTranslation(new Vector3(-r.center, 0)) * 
                    Matrix.CreateRotationZ(r.rotateAngle) * Matrix.CreateTranslation(new Vector3(r.position, 0));
                rockRect = CalculateBoundingRectangle(rockRect, transform);

                //mouse
                if (clicked) {
                    Vector2 mouseInRock = Vector2.Transform(mouse, Matrix.Invert(transform));
                    int xB = (int)Math.Round(mouseInRock.X);
                    int yB = (int)Math.Round(mouseInRock.Y);
                    // if the pixel is within the rock’s sprite rectangle
                    if (0 <= xB && xB < r.texture.Width && 0 <= yB && yB < r.texture.Height)
                    {
                        // Get the color of that pixel and check if alpha channel = 0
                        Color color = r.data[xB + yB * r.texture.Width];
                        if (color.A != 0)
                        {
                            score ++;
                            r.Initialize(); //reset postition
                            explode.Play(0.5f,0,0);
                            // mouse hit rock, reset the rock position, add mark
                        }
                    }
                }


                if (personRect.Intersects(rockRect)) 
                {
                    if(PixelCollision(personTransform, character.frameRect, character.texture.Width, ref character.data,
                        transform, r.texture.Width, r.texture.Height, ref r. data))
                    return true;
                    //scream.Play(0.2f,0,0);
                }
                if (personRect2.Intersects(rockRect))
                {
                    if (PixelCollision(personTransform2, character.frameRect, character.texture.Width, ref character.data,
                        transform, r.texture.Width, r.texture.Height, ref r.data))
                        return true;
                    //scream.Play(0.2f, 0, 0);
                }
            }

            foreach (heal h in heals)
            {
                Rectangle healRect = new Rectangle(0, 0, h.texture.Width, h.texture.Height);
                Matrix transform = Matrix.CreateTranslation(new Vector3(-h.center, 0)) *
                    Matrix.CreateRotationZ(h.rotateAngle) * Matrix.CreateTranslation(new Vector3(h.position, 0));
                healRect = CalculateBoundingRectangle(healRect, transform);

                //mouse
                if (clicked)
                {
                    Vector2 mouseInHeal = Vector2.Transform(mouse, Matrix.Invert(transform));
                    int xB = (int)Math.Round(mouseInHeal.X);
                    int yB = (int)Math.Round(mouseInHeal.Y);
                    // if the pixel is within the rock’s sprite rectangle
                    if (0 <= xB && xB < h.texture.Width && 0 <= yB && yB < h.texture.Height)
                    {
                        // Get the color of that pixel and check if alpha channel = 0
                        Color color = h.data[xB + yB * h.texture.Width];
                        if (color.A != 0)
                        {
                            life++;
                            h.Initialize(); //reset postition
                            explode.Play(0.5f, 0, 0);
                            // mouse hit rock, reset the rock position, add mark
                        }
                    }
                }


                if (personRect.Intersects(healRect))
                {
                    if (PixelCollision(personTransform, character.frameRect, character.texture.Width, ref character.data,
                        transform, h.texture.Width, h.texture.Height, ref h.data))
                        return true;
                    //scream.Play(0.2f,0,0);
                }
                if (personRect2.Intersects(healRect))
                {
                    if (PixelCollision(personTransform2, character.frameRect, character.texture.Width, ref character.data,
                        transform, h.texture.Width, h.texture.Height, ref h.data))
                        return true;
                    //scream.Play(0.2f, 0, 0);
                }
            }

            //bullet
            foreach (bullet b in bullets)
            {
                Rectangle bulletRect = new Rectangle(0, 0, b.texture.Width, b.texture.Height);
                Matrix transform = Matrix.CreateTranslation(new Vector3(-b.center, 0)) *
                    Matrix.CreateRotationZ(b.rotateAngle) * Matrix.CreateTranslation(new Vector3(b.position, 0));
                bulletRect = CalculateBoundingRectangle(bulletRect, transform);

                //mouse
                if (clicked)
                {
                    Vector2 mouseInHeal = Vector2.Transform(mouse, Matrix.Invert(transform));
                    int xB = (int)Math.Round(mouseInHeal.X);
                    int yB = (int)Math.Round(mouseInHeal.Y);
                    // if the pixel is within the rock’s sprite rectangle
                    if (0 <= xB && xB < b.texture.Width && 0 <= yB && yB < b.texture.Height)
                    {
                        // Get the color of that pixel and check if alpha channel = 0
                        Color color = b.data[xB + yB * b.texture.Width];
                        if (color.A != 0)
                        {
                            b.Initialize(); //reset postition
                            explode.Play(0.5f, 0, 0);
                            // mouse hit rock, reset the rock position, add mark
                        }
                    }
                }


                if (personRect.Intersects(bulletRect))
                {
                    if (PixelCollision(personTransform, character.frameRect, character.texture.Width, ref character.data,
                        transform, b.texture.Width, b.texture.Height, ref b.data))
                        return true;
                    //scream.Play(0.2f,0,0);
                }
                if (personRect2.Intersects(bulletRect))
                {
                    if (PixelCollision(personTransform2, character.frameRect, character.texture.Width, ref character.data,
                        transform, b.texture.Width, b.texture.Height, ref b.data))
                        return true;
                    //scream.Play(0.2f, 0, 0);
                }
            }
            return false;
        }

        private bool PixelCollision(Matrix transformA, Rectangle rectA, int widthA, ref Color[] dataA, Matrix transformB, int widthB, int heightB, ref Color[] dataB)
        {
            Matrix AToB = transformA * Matrix.Invert(transformB);
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, AToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, AToB);
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, AToB);
            for (int yA = rectA.Top; yA < rectA.Bottom; yA++)
            {   // For each row in A
                Vector2 posInB = yPosInB; // At the beginning of the row
                for (int xA = rectA.Left; xA < rectA.Right; xA++)
                { // For each pixel in the row
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);
                    if (0 <= xB && xB < widthB && 0 <= yB && yB < heightB)
                    {
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];
                        if (colorA.A != 0 && colorB.A != 0) return true;
                    }
                    posInB += stepX; // Move to next pixel in the row
                }
                yPosInB += stepY; // Move to the next row
            }
            return false; // No intersection found
        }

        private void UpdateInput()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Left))
            {
                character.velocity.X = -RunningMan.MAN_SPEED;
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                character.velocity.X = RunningMan.MAN_SPEED;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                character.velocity.X = 0.0f;
            }

            MouseState ms = Mouse.GetState();
            mouse.X = ms.X;
            mouse.Y = ms.Y;
            if(ms.LeftButton == ButtonState.Pressed &&
            ms.LeftButton != LastMouseButtonState)
            {
                clicked = true;
                gunshot.Play(0.5f,0,0);

            }
            else
            {
                clicked = false;
            }
        }
    }

}

