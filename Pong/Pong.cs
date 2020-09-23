﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.GameClasses;
using System;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Pong : Game
    {
        //Monogame sprites
        readonly GraphicsDeviceManager graphics;
        private SpriteFont cms;
        private SpriteFont arial;
        SpriteBatch spriteBatch;


        //Object variables
        Player redPlayer;
        Player bluePlayer;
        Ball ball;
        Settings st;

        //variables for the lives rectangles
        Texture2D single_pixel;

        //variables for gameStates
        private bool player_turn;
        private bool has_moved;
        private bool running;

        //settings
        bool hasPressedLeft = false;
        bool hasPressedRight = false;
        bool hasPressedUp = false;
        bool hasPressedDown = false;

        //the actual gamestates
        enum GameStates
        {
            Menu,
            GameOver,
            Game,
            Settings
        }

        GameStates currentState;
        allSettings selectedSetting;

        //random class
        readonly static Random rng = new Random();

        public Pong()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Change window size to a larger size.
            graphics.PreferredBackBufferWidth = 900;  
            graphics.PreferredBackBufferHeight = 600;   
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            //create a new ball(starting position, starting speed, image, colortint)
            ball = new Ball(new Vector2(450,300), new Vector2(3.0f, 3.0f), Content.Load<Texture2D>("ball"), Color.White);

            redPlayer = new Player(3, Color.IndianRed, Content.Load<Texture2D>("red_player"), new Vector2(0, 300),"West");
            bluePlayer = new Player(3, Color.CornflowerBlue, Content.Load<Texture2D>("blue_player"), new Vector2(884, 300), "East");

            st = new Settings();

            //create spritefont
            cms = Content.Load<SpriteFont>("ComicSans");
            arial = Content.Load<SpriteFont>("Menu");
            
            //set texturecolor
            single_pixel = new Texture2D(GraphicsDevice, 1, 1);
            single_pixel.SetData(new[] { Color.White });

            //set gameStates
            //true = blue | false = red
            player_turn = true;
            has_moved = false;

            currentState = GameStates.Menu;
            selectedSetting = 0;
            ResetBall();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch(currentState)
            {
                case GameStates.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentState = GameStates.Game;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        currentState = GameStates.Settings;
                    }
                    break;
                case GameStates.GameOver:
                    running = false;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentState = GameStates.Menu;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentState = GameStates.Game;
                        ResetBall();
                    }
                    //GAME OVER
                    break;
                case GameStates.Game:
                    //GAME IS RUNNING
                    if(!running)
                    {
                        running = true;
                        bluePlayer.setLives(st.lives);
                        redPlayer.setLives(st.lives);
                    }
                    RunGame();
                    break;
                case GameStates.Settings:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentState = GameStates.Menu;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Left) && !hasPressedLeft)
                    {
                        hasPressedLeft = true;
                        st.changeSetting(false, selectedSetting);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) && !hasPressedRight)
                    {
                        hasPressedRight = true;
                        st.changeSetting(true, selectedSetting);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) && !hasPressedUp)
                    {
                        hasPressedUp = true;
                        if (selectedSetting > 0)
                        {
                            selectedSetting--;
                        }

                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) && !hasPressedDown)
                    {
                        hasPressedDown = true;
                        if ((int) selectedSetting < (int) Enum.GetValues(typeof(allSettings)).Length-1)
                        {
                            selectedSetting++;
                        }
                    }


                    if (Keyboard.GetState().IsKeyUp(Keys.Left))
                    {
                        hasPressedLeft = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Right))
                    {
                        hasPressedRight = false;                    
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Up))
                    {
                        hasPressedUp = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Keys.Down))
                    {
                        hasPressedDown = false;
                    }

                    break;

            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // change background color accordingly with new pngs
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (currentState)
            {
                case GameStates.Menu:
                    //MAIN MENU
                    DrawCenteredString(cms, "PONG", new Vector2(450, 200), Color.White);

                    //Main menu text
                    DrawCenteredString(arial, "Press [Space] to start", new Vector2(450,340),Color.White);
                    DrawCenteredString(arial, "or press [S] to go to settings", new Vector2(450, 370), Color.White);
                    DrawCenteredString(arial, "Controls are {W,S} and {ArrowUp, ArrowDown} for the players", new Vector2(450,400), Color.Gray);

                    break;
                case GameStates.GameOver:
                    //GAME OVER
                    DrawCenteredString(cms, "PONG", new Vector2(450,200), Color.White);

                    //resulting text
                    DrawCenteredString(arial, "Game over, " + (player_turn ? "Red" : "Blue") + " wins!", new Vector2(450,340), Color.White);

                    //Help text
                    DrawCenteredString(arial, "Press [Enter] to return to the menu", new Vector2(450, 400), Color.Gray);
                    DrawCenteredString(arial, "or press [Space] to play again", new Vector2(450, 430), Color.Gray);
                    break;

                case GameStates.Settings:
                    //SETTINGS
                    DrawCenteredString(arial, "Settings", new Vector2(450, 60), Color.Gray);

                    //SELECTOR
                    DrawCenteredString(arial, "______________", new Vector2(450, 100 + 40 * (int)selectedSetting), Color.Gray);

                    //The settings
                    DrawCenteredString(arial, "Paddle speed <" + st.paddle_speed + ">", new Vector2(450, 100), Color.White);
                    DrawCenteredString(arial, "Speed multiplier on bounce <" + st.bounce_increase + ">", new Vector2(450, 140), Color.White);
                    DrawCenteredString(arial, "Starting speed of ball <" + st.ball_defaultspeed + ">", new Vector2(450, 180), Color.White);
                    DrawCenteredString(arial, "Amount of lives <" + st.lives + ">", new Vector2(450, 220), Color.White);

                    //Help text
                    DrawCenteredString(arial, "Use [Arrow keys] to change the values", new Vector2(450, 460), Color.Gray);
                    DrawCenteredString(arial, "Press [Enter] to return to the menu", new Vector2(450, 500), Color.Gray);

                    break;
                
                case GameStates.Game:
                    //GAME IS RUNNING
                    //draw middle_line
                    spriteBatch.Draw(single_pixel, new Rectangle(450, 0, 1, 600), Color.DarkGray);

                    //draw ball and players
                    spriteBatch.Draw(ball.getImage(), new Rectangle((int)ball.getPosition().X, (int)ball.getPosition().Y, ball.Width, ball.Height), ball.GetColor());
                    spriteBatch.Draw(redPlayer.getImage(), new Rectangle((int)redPlayer.getPosition().X, (int)redPlayer.getPosition().Y, redPlayer.Width, redPlayer.Height), redPlayer.getColor());
                    spriteBatch.Draw(bluePlayer.getImage(), new Rectangle((int)bluePlayer.getPosition().X, (int)bluePlayer.getPosition().Y, bluePlayer.Width, bluePlayer.Height), bluePlayer.getColor());

                    //drawing red player's lives
                    for (int i = 0; i < redPlayer.getLives(); i++)
                    {
                        spriteBatch.Draw(single_pixel, new Rectangle(4, (int)redPlayer.getPosition().Y + 31 + i * 12, 8, 8), redPlayer.getColor());
                    }

                    //drawing blue player's lives
                    for (int i = 0; i < bluePlayer.getLives(); i++)
                    {
                        spriteBatch.Draw(single_pixel, new Rectangle((int)900 - bluePlayer.Width + 4, (int)bluePlayer.getPosition().Y + 31 + i * 12, 8, 8), bluePlayer.getColor());
                    }

                    if (!has_moved)
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            Vector2 nextpos = Vector2.Add(ball.getPosition(), Vector2.Multiply(ball.getSpeed(), (float)st.ball_defaultspeed * st.bounce_speed * i * 5));
                            spriteBatch.Draw(ball.getImage(), new Rectangle((int)nextpos.X + ball.Width / 2, (int)nextpos.Y + ball.Height / 2, ball.Width / 3, ball.Height / 3), ball.GetColor());
                        }
                    }
                    break;
            }
                      

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ResetBall(bool isRed = false)
        {
            has_moved = false;
            st.bounce_speed = 1;
            if (isRed)
            {
                player_turn = false;
                ball.setSpeed(new Vector2((float)-rng.NextDouble() * 2, (float)rng.NextDouble() * 2 - 1));
                if(ball.getSpeed().X < 0.2)
                {
                    ball.setSpeed(new Vector2(-0.2f, 0.8f));
                }
            }
            else
            {
                player_turn = true;
                ball.setSpeed(new Vector2((float)rng.NextDouble() * 2, (float)rng.NextDouble() * 2 - 1));
                if (ball.getSpeed().X < 0.2)
                {
                    ball.setSpeed(new Vector2(0.2f, 0.8f));
                }
            }
            ball.setPosition(new Vector2(450-ball.Width/2, 300-ball.Height/2));
            
        }

        private Vector2 CenterString(SpriteFont font, String text)
        {
            return new Vector2((font.MeasureString(text).X / 2), (font.MeasureString(text).Y / 2));
        }

        private void DrawCenteredString(SpriteFont font, String text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, Vector2.Subtract(position, CenterString(font, text)), color);
        }

        private void RunGame()
        {
            //reset ball and remove a life when ball passes paddle
            if (ball.getPosition().X < -20)
            {
                redPlayer.setLives(redPlayer.getLives() - 1);
                ResetBall(true);
                if(redPlayer.getLives() < 1)
                {
                    currentState = GameStates.GameOver;
                    redPlayer.setLives(3);
                    bluePlayer.setLives(3);
                }
            }

            if (ball.getPosition().X > 920)
            {
                bluePlayer.setLives(bluePlayer.getLives() - 1);
                ResetBall(false);
                if (bluePlayer.getLives() < 1)
                {
                    currentState = GameStates.GameOver;
                    redPlayer.setLives(3);
                    bluePlayer.setLives(3);
                }
            }


            //RED PLAYER UP
            if (Keyboard.GetState().IsKeyDown(Keys.W) && redPlayer.getPosition().Y > 0)
            {
                redPlayer.setPosition(new Vector2(0, (float)(redPlayer.getPosition().Y - st.paddle_speed)));
                has_moved = true;
            }
            //RED PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.S) && redPlayer.getPosition().Y < 600 - redPlayer.Height)
            {
                redPlayer.setPosition(new Vector2(0, (float)(redPlayer.getPosition().Y + st.paddle_speed)));
                has_moved = true;
            }
            //BLUE PLAYER UP
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && bluePlayer.getPosition().Y > 0)
            {
                bluePlayer.setPosition(new Vector2(900 - bluePlayer.Width, (float)(bluePlayer.getPosition().Y - st.paddle_speed)));
                has_moved = true;
            }
            //BLUE PLAYER DOWN
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && bluePlayer.getPosition().Y < 600 - bluePlayer.Height)
            {
                bluePlayer.setPosition(new Vector2(900 - bluePlayer.Width, (float)(bluePlayer.getPosition().Y + st.paddle_speed)));
                has_moved = true;
            }

            //change ball position
            if (has_moved)
            {
                ball.setPosition(Vector2.Add(ball.getPosition(), Vector2.Multiply(ball.getSpeed(), (float)st.ball_defaultspeed * st.bounce_speed)));
            }

            //check boundaries of ball position
            if (ball.getPosition().Y >= 600 - ball.Height || ball.getPosition().Y <= 0)
            {
                ball.invertY();
            }

            if (player_turn == false && ball.getPosition().X < redPlayer.Width && ball.getPosition().Y > redPlayer.getPosition().Y - ball.Height && ball.getPosition().Y < redPlayer.getPosition().Y + redPlayer.Height)
            {
                double DistanceToMiddle = (redPlayer.getPosition().Y + redPlayer.Height / 2) - (ball.getPosition().Y + ball.Height / 2);
                double normalizedDistance = DistanceToMiddle / (redPlayer.Height / 2);


                ball.setSpeed(new Vector2((float)Math.Cos(normalizedDistance), (float)-Math.Sin(normalizedDistance)));


                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                st.bounce_speed *= st.bounce_increase;



            }

            if (player_turn == true && ball.getPosition().X > 900 - bluePlayer.Width - ball.Width && ball.getPosition().X < 900 && ball.getPosition().Y > bluePlayer.getPosition().Y - ball.Height && ball.getPosition().Y < bluePlayer.getPosition().Y + bluePlayer.Height)
            {

                double DistanceToMiddle = (bluePlayer.getPosition().Y + bluePlayer.Height / 2) - (ball.getPosition().Y + ball.Height / 2);
                double normalizedDistance = DistanceToMiddle / (bluePlayer.Height / 2);


                ball.setSpeed(new Vector2((float)-Math.Cos(normalizedDistance), (float)-Math.Sin(normalizedDistance)));

                //switch player turn
                player_turn = !player_turn;

                //speed up the ball on bounce
                st.bounce_speed *= st.bounce_increase;
            }
        }
    }
}
