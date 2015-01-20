/// <summary>
/// RYAN DAVIS
/// ICS 4U
/// 17/06/2012
/// Game features: full platformer engine, console service, score/lives/status report, good collision checking, different sprites for different movements, 
/// smooth controls, respawning, lives are shown numerically and visually, play again feature
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        HUD hud;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ConsoleKeyInfo response;
        List<Vector2> blockPositions = new List<Vector2>();
        List<Vector2> blueGemPositions = new List<Vector2>();
        List<Rectangle> blueGemRectangles = new List<Rectangle>();
        List<Vector2> firePositions = new List<Vector2>();
        Vector2 skullVector = new Vector2(270, 150-32);
        Vector2 stopVector = new Vector2(32*17, 250);
        Vector2 stopVector2 = new Vector2(32 * 20, 220);
        Vector2 stopVector3 = new Vector2(32 * 23, 280);

        Vector2 playerSpawnPoint = new Vector2(20f, 50f);
        int numberOfBlueGems = 4;
        int numberOfFires = 4;
        int numberOfSkulls = 4;
        int numberOfBlocksR1 = 35;
        int numberOfBlocksR2 = 30;
        int numberOfBlocksR3 = 7;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        
        protected override void Initialize()
        {
            base.Initialize();
            Console.Title = "Game Console";
            this.Window.Title = "Platformer";
            Console.WriteLine("Welcome Adventurer!\n\nThe controls are fairly standard, figure them out!\nScary things = bad, gems = good!\nThis is the console!");
            hud.lives = 3;
            hud.info = "Good luck white hat!";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            //fire
            for (int i = 0; i < numberOfFires-1; i++)
            {
                firePositions.Add(new Vector2(i*32+32*3,150-32));
                firePositions.Add(new Vector2(i * 32 + 320, 310 - 32));
            }
            firePositions.Add(new Vector2(32 + 32*10, 150 - 32));
            for (int i = 0; i < 40; i++)
            {
                firePositions.Add(new Vector2(i*32,32*22));//bottom row
            }
           
            
            //blocks
            for (int i = 0; i < numberOfBlocksR1; i++)
            {
                blockPositions.Add(new Vector2(i * 32, 150));//all the floor blocks
            }
            blockPositions.Add(new Vector2(240, 150 - 32)); blockPositions.Add(new Vector2(700, 150-32));//skull collision blocks
            for (int i = 0; i < numberOfBlocksR2; i++)
            {
                blockPositions.Add(new Vector2(i * 32+320, 310));
            }

            for (int i = 0; i < numberOfBlocksR3; i++)
            {
                blockPositions.Add(new Vector2(i * 32 +400, 50));
            }
            //the bottom row of the blocks
            for (int i = 0; i < 6; i++)
            {
                blockPositions.Add(new Vector2(i * 32 + 32*3, 500));
            }
            for (int i = 0; i < 3; i++)
            {
                blockPositions.Add(new Vector2(i * 32 + 32 * 12, 564));
            }
            blockPositions.Add(new Vector2(32 * 19, 32*15));
            blockPositions.Add(new Vector2(32 * 22, 32 * 18));
            for (int i = 0; i < 6; i++)
            {
                blockPositions.Add(new Vector2(i * 32 + 32 * 25, 32*20));
            }
            for (int i = 0; i < 3; i++)
            {
                blockPositions.Add(new Vector2(i * 32 + 32 * 33, 32 * 17));
            }
            
            //blue gems
            for (int i = 0; i < numberOfBlueGems-1; i++)
            {
                blueGemPositions.Add(new Vector2(i * 32 + 400, 150-32));
            }
            blueGemPositions.Add(new Vector2(32 * 19, 32 * 14));
            for (int i = 0; i < numberOfBlueGems-1; i++)
            {
                blueGemPositions.Add(new Vector2(i * 32 + 832, 310 - 32));//////////////////////////////////////////////////////////////////////////
            }
            //blue gem rectangles
            for (int i = 0; i < numberOfBlueGems+3; i++)
            {
                blueGemRectangles.Add(new Rectangle((int)blueGemPositions[i].X, (int)blueGemPositions[i].Y, gem_blue.Width, gem_blue.Height));
            }
            blueGemRectangles.Add(new Rectangle((int)blueGemPositions[3].X, (int)blueGemPositions[3].Y, gem_blue.Width, gem_blue.Height));

        }
        /// <summary>
        /// Load textures/////////////////////////////////////////////////
        /// </summary>

        int maxX;
        int maxY;
        int continues;
        int difficulty;
        int fCount;
        float gravity = 0.3F;
        float ySpeed = 20F;
        float fallSpeed = 0F;
        int skullXSpeed = 3;
        int stopYSpeed = 2;
        int stopYSpeed2 = 4;
        int stopYSpeed3 = 3;
        Boolean jump;
        Boolean hitBlock;
        Boolean falling;

        Texture2D player;
        Texture2D livesIcon;
        Vector2 playerPosition = new Vector2(20,50);
        Texture2D block;
        Texture2D gem_blue;
        Texture2D gem_red;
        Texture2D fire;
        Texture2D skull;
        Texture2D stop;
        Texture2D exit;
        int removeGem=9000;
        Random random = new Random();
        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        protected override void LoadContent()
        {
            hud = new HUD();
            hud.font = Content.Load<SpriteFont>("Arial");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = Content.Load<Texture2D>("explorer_jumpleft");
            block = Content.Load<Texture2D>("wall_block");
            gem_blue = Content.Load<Texture2D>("gem_blue");
            gem_red = Content.Load<Texture2D>("gem_red");
            fire = Content.Load<Texture2D>("fire");
            skull = Content.Load<Texture2D>("skull");
            stop = Content.Load<Texture2D>("stop");
            livesIcon = Content.Load <Texture2D>("npc_boy");
            exit = Content.Load<Texture2D>("exitsign");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            //Rectangle exitRectangle = new Rectangle((int)exitVector.X, (int)exitVector.Y, exit.Width, exit.Height);
            Rectangle playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, player.Width, player.Height);
            if (playerPosition.X > (32*34) && playerPosition.Y > (32*15+4))
            {
                hud.info="won";
                Console.Clear();
                Console.WriteLine("Good job adventurer! You saved the princess and such. Play again? (Y/N)");
                response = Console.ReadKey();
                if (response.Key.ToString() == "Y")
                {
                    Console.Clear();
                    Console.WriteLine("Continues = " + continues + "\n\n");
                    hud.lives = 3;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                }
                else if (response.Key.ToString() == "N")
                {
                    Console.WriteLine("LOSER!!!");
                    this.Exit();
                }
            }
            maxX = graphics.GraphicsDevice.Viewport.Width - player.Width;
            maxY = graphics.GraphicsDevice.Viewport.Height - player.Height;
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if (newState.IsKeyDown(Keys.LeftShift)){
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }
            if (newState.IsKeyDown(Keys.RightShift)){
                graphics.IsFullScreen = false;
                graphics.ApplyChanges();
            }
            if (newState.IsKeyDown(Keys.W))
            {
                jump = true;
            }
            if (newState.IsKeyDown(Keys.A))
            {
                player = Content.Load<Texture2D>("explorer_jumpleft");
                playerPosition.X -= 4;
            }
            if (newState.IsKeyDown(Keys.S))
            {
                playerPosition.Y += 4;
            }
            if (newState.IsKeyDown(Keys.D))
            {
                player = Content.Load<Texture2D>("explorer_jumpright");
                playerPosition.X += 4;
            }

            if (playerPosition.Y > maxY||playerPosition.X<-20||playerPosition.X>maxX+20)
            {
                playerPosition.X = playerSpawnPoint.X;
                playerPosition.Y = playerSpawnPoint.Y;
                hud.lives -= 1;
            }

            if (jump == true)
            {
                falling = false;
                playerPosition.Y -= 1;
                playerPosition.Y -= ySpeed * gravity;
                ySpeed -= 1F;
            }
            if (falling == true)
            {
                playerPosition.Y -= fallSpeed * gravity;
                fallSpeed -= 1F;
                
            }

            //COLLISION CHECK WITH FIRE
            for (int i = 0; i < firePositions.Count; i++)
            {
                Rectangle fireRectangle = new Rectangle((int)firePositions[i].X, (int)firePositions[i].Y, fire.Width, fire.Height);

                if (playerRectangle.Intersects(fireRectangle))
                {
                    hud.score -= 5;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                    hud.lives -= 1;
                }
            }
            
            //COLLISION CHECKING WITH BLOCK
            

            for (int i = 0; i < blockPositions.Count; i++)
            {
                Rectangle blockRectangle = new Rectangle((int)blockPositions[i].X, (int)blockPositions[i].Y, block.Width, block.Height);
                Rectangle skullRectangle = new Rectangle((int)skullVector.X, (int)skullVector.Y, skull.Width, skull.Height);
                
                Rectangle stopRectangle = new Rectangle((int)stopVector.X, (int)stopVector.Y, stop.Width - 5, stop.Height - 5);
                Rectangle stopRectangle2 = new Rectangle((int)stopVector2.X, (int)stopVector2.Y, stop.Width - 5, stop.Height - 5);
                Rectangle stopRectangle3 = new Rectangle((int)stopVector3.X, (int)stopVector3.Y, stop.Width - 5, stop.Height - 5);

                if (skullRectangle.Intersects(blockRectangle))
                {
                    if (skullVector.X < 300)
                    {
                        skullVector.X += 2;
                    }
                    if (skullVector.X > 300)
                    {
                        skullVector.X -= 2;
                    }
                    skullXSpeed *= -1;
                }
                if (stopVector.Y > 300)
                {
                    stopYSpeed = -2;
                }
                if (stopVector.Y < 170)
                {
                    stopYSpeed = 2;
                }
                if (stopVector2.Y > 300)
                {
                    stopYSpeed2 = -4;
                }
                if (stopVector2.Y < 170)
                {
                    stopYSpeed2 = 4;
                }
                if (stopVector3.Y > 300)
                {
                    stopYSpeed3 = -3;
                }
                if (stopVector2.Y < 170)
                {
                    stopYSpeed3 = 3;
                }
                

                if (playerRectangle.Intersects(blockRectangle))
                {
                    fallSpeed = 0;
                    hitBlock = true;
                    if (playerPosition.Y > blockRectangle.Y - 30)
                    {
                        playerPosition.Y = blockRectangle.Y - 30;
                        ySpeed = 0;
                       /* if (playerPosition.X < blockRectangle.X)
                        {
                            playerPosition.X -= playerPosition.X * 0.11F;
                        }
                        if (playerPosition.X > blockRectangle.X)
                        {
                            playerPosition.X += playerPosition.X * 0.11F;
                        }*/
                    }
                    else
                    if (playerPosition.Y < blockRectangle.Y-30)
                    {
                        //playerPosition.Y = blockRectangle.Y - 30;
                        //hud.info = "above";
                        falling = false;
                        jump = false;
                        ySpeed =20;
                    }
                }
                else
                {
                    falling = true;
                }
                if(playerRectangle.Intersects(skullRectangle)){
                    if (skullVector.X > playerPosition.X)
                    {
                        skullVector.X += 5;
                    }
                    else skullVector.X -= 5;
                    playerPosition.X=playerSpawnPoint.X;
                    playerPosition.Y=playerSpawnPoint.Y;
                    hud.score-=5;
                    hud.lives-=1;
                }
                if (playerRectangle.Intersects(stopRectangle))
                {
                    if (stopVector.Y > playerPosition.X)
                    {
                        stopVector.Y += 15;
                    }
                    else stopVector.Y -= 15;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                    hud.score -= 5;
                    hud.lives -= 1;
                }
                if (playerRectangle.Intersects(stopRectangle2))
                {
                    if (stopVector2.Y > playerPosition.X)
                    {
                        stopVector2.Y += 15;
                    }
                    else stopVector2.Y -= 15;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                    hud.score -= 5;
                    hud.lives -= 1;
                }
                if (playerRectangle.Intersects(stopRectangle3))
                {
                    if (stopVector3.Y > playerPosition.X)
                    {
                        stopVector3.Y += 15;
                    }
                    else stopVector3.Y -= 15;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                    hud.score -= 5;
                    hud.lives -= 1;
                }
                fCount++;
                if (fCount == 60)
                {
                    skullVector.X += skullXSpeed;
                    stopVector.Y += stopYSpeed;
                    stopVector2.Y += stopYSpeed2;
                    stopVector3.Y += stopYSpeed3;
                    fCount = 0;
                }
            }
           //COLLISION CHECKING WITH GEMS
            for (int i = 0; i < numberOfBlueGems+3; i++)
            {
                if (playerRectangle.Intersects(blueGemRectangles[i]))
                {
                    hud.info = Convert.ToString(i);
                    hud.score += 10;
                    removeGem = i;                
                }
            }
            if (removeGem != 9000)
            {
                if (removeGem < blueGemPositions.Count)
                {
                    /*for (int i = 0; i < removeGem; i++)
                    {
                        blueGemPositions.Add(new Vector2(i * 32 + 400, 400));
                        blueGemRectangles.Add(new Rectangle((int)blueGemPositions[i].X, (int)blueGemPositions[i].Y, gem_blue.Width, gem_blue.Height));
                    }*/
                }
                blueGemPositions.Remove(blueGemPositions[removeGem]);
                blueGemRectangles.Remove(blueGemRectangles[removeGem]);
                numberOfBlueGems -= 1;
                removeGem = 9000;
            }
            //console
            
            if (hud.lives < 0)
            {                
                Console.WriteLine("You died!\nContinue? Y/N");
                response = Console.ReadKey();
                if (response.Key.ToString() == "Y")
                {
                    continues += 1;
                    Console.Clear();
                    Console.WriteLine("Continues = "+continues+"\n\n");
                    hud.lives = 3;
                    playerPosition.X = playerSpawnPoint.X;
                    playerPosition.Y = playerSpawnPoint.Y;
                    hud.score = 0;
                }
                else if (response.Key.ToString() == "N")
                {
                    Console.WriteLine("LOSER!!!");
                    this.Exit();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 exitVector = new Vector2(32*35,32*16+4);

            Rectangle exitRectangle = new Rectangle((int)exitVector.X, (int)exitVector.Y, exit.Width, exit.Height);
            Vector2 livesVector = new Vector2(100, 20);

            // draw the sprite
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(exit, exitVector, Color.White);
            spriteBatch.Draw(player, playerPosition, Color.White);
            for (int i = 0; i < hud.lives;i++)
            {
                spriteBatch.Draw(livesIcon, livesVector, Color.White);
                livesVector.X += 40;
            }
            //gems
            foreach (Vector2 gemBluePosition in blueGemPositions)
            {
                spriteBatch.Draw(gem_blue, gemBluePosition, Color.White);
            }
            //blocks
            foreach (Vector2 blockPosition in blockPositions)
            {
                spriteBatch.Draw(block, blockPosition, Color.White);
                Rectangle blockRectangle = new Rectangle((int)blockPosition.X, (int)blockPosition.Y, block.Width, block.Height);
            }
            //fire
            foreach (Vector2 firePosition in firePositions)
            {
                spriteBatch.Draw(fire, firePosition, Color.White);
                Rectangle blockRectangle = new Rectangle((int)firePosition.X, (int)firePosition.Y, fire.Width, fire.Height);
            }
            //skulls
            spriteBatch.Draw(skull, skullVector, Color.White);
            
            //stop signs
            spriteBatch.Draw(stop, stopVector, Color.White);
            spriteBatch.Draw(stop, stopVector2, Color.White);
            spriteBatch.Draw(stop, stopVector3, Color.White);
            //Rectangle stopRectangle = new Rectangle((int)stopVector.X, (int)stopVector.Y, stop.Width, stop.Height);
            

            hud.DrawScore(spriteBatch);
            hud.DrawLives(spriteBatch);
            hud.DrawInfo(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
