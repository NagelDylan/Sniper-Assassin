using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using Animation2D;
using Helper;

namespace SniperAssassin
{
    public class Game1 : Game
    {
        const int MENU = 0;
        const int PRE_GAME = 1;
        const int INSTRUCTIONS = 2;
        const int GAMEPLAY = 3;
        const int PAUSE = 4;
        const int EXIT = 5;
        const int GAME_OVER = 6;

        const int HUD_HEIGHT = 84;

        const int ROOF_RAND_ARRAY_LOC = 36;
        const int FLOOR_RAND_ARRAY_LOC_LEFT = 37;
        const int FLOOR_RAND_ARRAY_LOC_RIGHT = 38;

        const int HEART_WIDTH = 23;
        const int HEART_HEIGHT = 21;
        const float BULLET_MULTIPLIER = 0.0526f;
        const int BULLET_WIDTH = (int)(120 * BULLET_MULTIPLIER);
        const int BULLET_HEIGHT = (int)(400 * BULLET_MULTIPLIER);

        const int AMMO_SPEED = 2;
        const int MAX_SWIRVLE = 5;

        const int ASSASSIN_1 = 0;
        const int ASSASSIN_2 = 1;
        const int ASSASSIN_3 = 2;

        const int MAX_HEALTH = 10;
        const int MAX_AMMO = 10;
        const int MIN_AMMO = 0;
        const int MIN_HEALTH = 0;
        const int MIN_SCORE = 0;

        const int INSTRUCTIONS_FIRST_LINE_Y_LOC = 100;
        const int INSTRUCTIONS_SPACING = 70;

        const int OFF_SCREEN_X = 0;
        const int OFF_SCREEN_Y = -200;

        const int BULLET_INITIAL_X_VALUE = 23;
        const int BULLET_SPACING = 47;
        const int BULLET_Y_VALUE = 695;
        const int HEART_Y_VALUE = 723;

        const int CLOUD_SPEED = -1;

        const int SECOND_ASSASSIN_SCORE = 1000;
        const int THIRD_ASSASSIN_SCORE = 2000;

        const float SCREEN_TRANS_INCREASE = 0.015f;

        const int MAX_OPACITY = 1;
        const int MIN_OPACITY = 0;

        int gameState = MENU;
        int nextGameState;

        Random rng = new Random();

        SpriteFont userStatsFonts;
        SpriteFont highScoreFont;

        MouseState mouse;
        MouseState prevMouse;

        Texture2D sniperTowerImg;
        Rectangle sniperTowerRec;
        Texture2D cloudImg;
        Rectangle[] cloudRec;

        Texture2D uavMapImg;
        Rectangle uavMapRec;

        Texture2D scopeImg;
        Rectangle scopeRec;
        Texture2D mouseClickImg;
        Rectangle mouseClickRec;

        Texture2D ammoDropImg;
        Rectangle ammoDropRec;
        int ammoDropXLoc;
        Texture2D ammoDropUavDotImg;
        Rectangle ammoDropUavDotRec;
        int ammoDropChance;
        bool isAmmoDropped = false;
        double ammoDropAngle = 0;

        Texture2D playerHpImg;
        float[] playerHpRecTrans = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        Texture2D bulletImg;
        float[] bulletRecTrans = new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        Texture2D playButtonImg;
        Rectangle playButtonRec;
        Texture2D instructionsButtonImg;
        Rectangle instructionsButtonRec;
        Texture2D exitButtonImg;
        Rectangle exitButtonRec;
        Texture2D backButtonImg;
        Rectangle backButtonInstrucRec;
        Texture2D goButtonImg;
        Rectangle goButtonRec;
        Texture2D menuButtonImg;
        Rectangle menuButtonRec;

        Texture2D nonGameBorderImg;
        Rectangle nonGameBorderRec;
        Texture2D bloodScreenImg;
        Rectangle bloodScreenRec;
        float bloodTrans = 0;

        Texture2D assassinImg;
        Vector2[] assassinLocs = new Vector2[] { new Vector2(123, 190), new Vector2(170, 190), new Vector2(223, 174), new Vector2(275, 190), new Vector2(320, 190), new Vector2(124, 290), new Vector2(170, 290), new Vector2(223, 289), new Vector2(275, 289), new Vector2(320, 289), new Vector2(124, 377), new Vector2(170, 377), new Vector2(222, 377), new Vector2(275, 377), new Vector2(320, 377), new Vector2(124, 447), new Vector2(170, 447), new Vector2(222, 447), new Vector2(275, 447), new Vector2(320, 447), new Vector2(125, 532), new Vector2(170, 532), new Vector2(275, 532), new Vector2(320, 532), new Vector2(124, 602), new Vector2(170, 602), new Vector2(275, 602), new Vector2(320, 602), new Vector2(77, 208), new Vector2(366, 207), new Vector2(77, 285), new Vector2(366, 284), new Vector2(77, 365), new Vector2(366, 366), new Vector2(77, 442), new Vector2(366, 441), new Vector2(0, 25), new Vector2(0, 618), new Vector2(0, 618) };
        Timer[] assassinTimer = new Timer[3];
        int assassinNum = 0;
        Texture2D assassinSprite;
        Vector2[] assassinAnimLoc = new Vector2[3];
        Animation[] assassinAnim = new Animation[3];
        Texture2D assassinUavDotImg;
        Rectangle[] assassinUavDotRecs = new Rectangle[3];

        string gameTitle = "Sniper Assassin";
        Vector2 gameTitleLoc = new Vector2(50, 50);
        Vector2 gameTitleShadowLoc = new Vector2(52, 52);
        SpriteFont titleFont;

        float playButtonTrans = 1;
        float instructionsButtonTrans = 1;
        float exitButtonTrans = 1;
        float backButtonTrans = 1;
        float goButtonTrans = 1;
        float menuButtonTrans = 1;

        string highScoreLabel = "High Score";
        Vector2 highScoreLabelPos = new Vector2(215, 686);
        Vector2 highScorePos = new Vector2(215, 664);
        Vector2 scorePos = new Vector2(15, 664);
        int userScore;
        int curGameHighScore;

        int userAmmo = MAX_AMMO;
        int userHealth = MAX_HEALTH;
        Rectangle[] playerHpRec;
        Rectangle[] bulletRec = new Rectangle[10];

        Timer gameTimer;
        Vector2 gameTimerPos = new Vector2(385, 664);
        Timer hpRegenTimer;

        int highScore = 0;

        int screenWidth;
        int screenHeight;

        int[] randomLoc = new int[] { -1, -1, -1 };
        int[] prevRandomLoc = new int[] { -1, -1, -1 };
        bool[] isAssassinRandom = new bool[] { false, false, false };

        string instructionsTitle = "Instructions";
        Vector2 instructionsTitleLoc = new Vector2(100, 15);
        Vector2 instructionsTitleShadLoc = new Vector2(102, 17);
        string[] instructionsLines = new string[] { "Assassins randomly change location", "Shoot them in time or lose health", "Player begins with 10 HP and ammo", "5 health lost if assassin timer ends", "Ammo drops supply 2 bullets", "At 1000 score, new assassin is added", "At 2000 score, new assassin is added", "TIP: use UAV in corner for tracking" };
        Vector2[] instructionLinesLocs = new Vector2[8];

        Rectangle assassinInstructRec;
        Rectangle ammoInstructRec;
        Rectangle healthInstructRec;
        Rectangle ammoDropInstructRec;
        Rectangle uavDotInstructRec;

        Texture2D blackScreenImg;
        Rectangle blackScreenRec;
        float blackScreenPauseTrans = 0.6f;
        Rectangle windowBlackOutRec;
        Rectangle hudBlackOutRec;

        KeyboardState kb;
        KeyboardState prevKb;

        string pauseText = "Paused";
        Vector2 pauseTextLoc;
        string backToGameText = "Press ESC to return to game play";
        Vector2 backtoGameTextLoc;
        string exitGameText = "Press ENTER for menu (restart data)";
        Vector2 exitGameTextLoc;
        string highScoreUserText = "New high score: ";
        Vector2 highScoreUserTextLoc;

        bool isHighScoreAcheived = false;

        string endOfGameLossMessage = "You played trash!";
        Vector2 endOfGameLossMessageLoc;
        Vector2 endOfGameLossMessageShadLoc;

        string endOfGameWinMessage = "You got the high score!";
        Vector2 endOfGameWinMessageLoc;
        Vector2 endOfGameWinShadMessageLoc;

        string endOfGameScore = "Your final score: ";
        Vector2 endOfGameScoreLoc;
        Rectangle userInfoSquareRec;

        Vector2 endOfGameHighScoreLoc;

        string highScoreTitle = "HIGH SCORE";
        Vector2 highScoreTitleLoc;
        Vector2 highScoreTitleShadLoc;

        string loseTitle = "YOU LOSE";
        Vector2 loseTitleLoc;
        Vector2 loseTitleShadLoc;

        float screenTrans = 0;
        bool isFadingIn = false;
        bool isFadingOut = false;

        SoundEffect gunShotSnd;
        SoundEffect missShotSnd;
        SoundEffect assassinShotSnd;
        SoundEffect buttonClickSnd;
        SoundEffect lossSnd;
        SoundEffect noAmmoSnd;
        SoundEffect ammoPickup;

        Song menuBkgMsk;
        Song gameBkgMsk;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.graphics.PreferredBackBufferWidth = 469;
            this.graphics.PreferredBackBufferHeight = 664 + HUD_HEIGHT;

            graphics.ApplyChanges();

            SoundEffect.MasterVolume = 1f;
            MediaPlayer.Volume = 0.6f;
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            screenWidth = this.graphics.GraphicsDevice.Viewport.Width;
            screenHeight = this.graphics.GraphicsDevice.Viewport.Height;

            userStatsFonts = Content.Load<SpriteFont>("Fonts/GameStats");
            highScoreFont = Content.Load<SpriteFont>("Fonts/HighScoreFont");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            bulletImg = Content.Load<Texture2D>("Sprites/Bullet2.0");
            for (int i = 0; i < 10; i++)
            {
                bulletRec[i] = new Rectangle(BULLET_INITIAL_X_VALUE + BULLET_SPACING * i, BULLET_Y_VALUE, BULLET_WIDTH, BULLET_HEIGHT);
            }

            playerHpImg = Content.Load<Texture2D>("Sprites/PlayerHp");
            playerHpRec = new Rectangle[] { new Rectangle(15, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(62, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(109, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(156, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(202, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(249, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(296, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(344, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(390, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT), new Rectangle(437, HEART_Y_VALUE, HEART_WIDTH, HEART_HEIGHT) };

            assassinImg = Content.Load<Texture2D>("Sprites/AssasinImg");
            assassinUavDotImg = Content.Load<Texture2D>("Sprites/OrangeCircle");
            for (int i = 0; i < 3; i++)
            {
                assassinUavDotRecs[i] = new Rectangle(OFF_SCREEN_X, OFF_SCREEN_Y, (int)(assassinUavDotImg.Width * 0.11), (int)(assassinUavDotImg.Height * 0.11));
            }

            scopeImg = Content.Load<Texture2D>("Sprites/Scope");
            scopeRec = new Rectangle((int)(mouse.X - scopeImg.Width * 0.5), (int)(mouse.Y - scopeImg.Height * 0.5), scopeImg.Width, scopeImg.Height);

            sniperTowerImg = Content.Load<Texture2D>("Sprites/ShootingTower");
            sniperTowerRec = new Rectangle(0, 0, screenWidth, screenHeight - HUD_HEIGHT);
            cloudImg = Content.Load<Texture2D>("Sprites/CloudImage");
            cloudRec = new Rectangle[] { new Rectangle(200, 300, cloudImg.Width, cloudImg.Height), new Rectangle(650, 100, cloudImg.Width, cloudImg.Height), new Rectangle(400, 400, cloudImg.Width, cloudImg.Height), new Rectangle(300, 800, cloudImg.Width, cloudImg.Height) };

            uavMapImg = Content.Load<Texture2D>("Sprites/UavMap");
            uavMapRec = new Rectangle(0, 0, (int)(uavMapImg.Width * 0.0165), (int)(uavMapImg.Height * 0.0175));

            playButtonImg = Content.Load<Texture2D>("Sprites/ButtonPlay");
            playButtonRec = new Rectangle((int)(screenWidth * 0.5 - playButtonImg.Width * 0.2), 140, (int)(playButtonImg.Width * 0.4), (int)(playButtonImg.Height * 0.4));
            instructionsButtonImg = Content.Load<Texture2D>("Sprites/ButtonInstructions");
            instructionsButtonRec = new Rectangle((int)(screenWidth * 0.5 - instructionsButtonImg.Width * 0.2), 320, (int)(instructionsButtonImg.Width * 0.4), (int)(instructionsButtonImg.Height * 0.4));
            exitButtonImg = Content.Load<Texture2D>("Sprites/ButtonExit");
            exitButtonRec = new Rectangle((int)(screenWidth * 0.5 - exitButtonImg.Width * 0.2), 510, (int)(exitButtonImg.Width * 0.4), (int)(exitButtonImg.Height * 0.4));
            goButtonImg = Content.Load<Texture2D>("Sprites/GoButton");
            goButtonRec = new Rectangle((int)(screenWidth * 0.5 - goButtonImg.Width * 0.5), (int)(screenHeight * 0.5 - goButtonImg.Height * 0.5), goButtonImg.Width, goButtonImg.Height);
            backButtonImg = Content.Load<Texture2D>("Sprites/button_back");
            backButtonInstrucRec = new Rectangle((int)(screenWidth * 0.5 - backButtonImg.Width * 0.4), screenHeight - 80, (int)(backButtonImg.Width * 0.8), (int)(backButtonImg.Height * 0.8));
            menuButtonImg = Content.Load<Texture2D>("Sprites/MenuButton");
            menuButtonRec = new Rectangle(backButtonInstrucRec.X, backButtonInstrucRec.Y - 50, (int)(backButtonImg.Width * 0.8), (int)(backButtonImg.Height * 0.8));

            bloodScreenImg = Content.Load<Texture2D>("Sprites/BloodyScreen");
            bloodScreenRec = new Rectangle(0, 0, screenWidth, screenHeight - HUD_HEIGHT);
            nonGameBorderImg = Content.Load<Texture2D>("Sprites/NonGameBorderThing");
            nonGameBorderRec = new Rectangle(0, 0, screenWidth, screenHeight);
            blackScreenImg = Content.Load<Texture2D>("Sprites/BlackScreen");
            blackScreenRec = new Rectangle(0, 0, screenWidth, screenHeight);
            windowBlackOutRec = new Rectangle(100, 100, 250, 550);
            hudBlackOutRec = new Rectangle(0, screenHeight - HUD_HEIGHT, screenWidth, HUD_HEIGHT);

            ammoDropImg = Content.Load<Texture2D>("Sprites/AmmoDrop");
            ammoDropRec = new Rectangle(OFF_SCREEN_X, OFF_SCREEN_Y, (int)(ammoDropImg.Width * 0.1), (int)(ammoDropImg.Height * 0.1));
            ammoDropUavDotImg = Content.Load<Texture2D>("Sprites/YellowCircle");
            ammoDropUavDotRec = new Rectangle(OFF_SCREEN_X, OFF_SCREEN_Y, (int)(ammoDropUavDotImg.Width * 0.11), (int)(ammoDropUavDotImg.Height * 0.11));

            assassinSprite = Content.Load<Texture2D>("Sprites/AssasinSprite");
            for (int i = 0; i < 3; i++)
            {
                assassinAnimLoc[i] = new Vector2(OFF_SCREEN_X, OFF_SCREEN_Y);
                assassinAnim[i] = new Animation(assassinSprite, 6, 1, 6, 0, 0, 0, 16, assassinAnimLoc[i], 0.35f, false);
            }

            mouseClickImg = Content.Load<Texture2D>("Sprites/MouseClicker");
            mouseClickRec = new Rectangle(OFF_SCREEN_X, OFF_SCREEN_Y, (int)(mouseClickImg.Width * 0.09), (int)(mouseClickImg.Height * 0.09));

            pauseTextLoc = new Vector2((int)(screenWidth * 0.5 - 85), (int)(screenHeight * 0.5 - 30));
            backtoGameTextLoc = new Vector2(45, pauseTextLoc.Y + 55);
            exitGameTextLoc = new Vector2(25, backtoGameTextLoc.Y + 30);

            endOfGameLossMessageLoc = new Vector2(135, 130);
            endOfGameLossMessageShadLoc = new Vector2(endOfGameLossMessageLoc.X + 1, endOfGameLossMessageLoc.Y + 1);
            endOfGameWinMessageLoc = new Vector2(100, 130);
            endOfGameWinShadMessageLoc = new Vector2(endOfGameWinMessageLoc.X + 1, endOfGameWinMessageLoc.Y + 1);

            endOfGameScoreLoc = new Vector2(115, 320);
            endOfGameHighScoreLoc = new Vector2(125, 390);
            userInfoSquareRec = new Rectangle(40, 270, 388, 200);


            highScoreUserTextLoc = new Vector2(130, 650);
            highScoreTitleLoc = new Vector2(85, 60);
            highScoreTitleShadLoc = new Vector2(highScoreTitleLoc.X + 2, highScoreTitleLoc.Y + 2);

            loseTitleLoc = new Vector2(110, 60);
            loseTitleShadLoc = new Vector2(loseTitleLoc.X + 2, loseTitleLoc.Y + 2);


            for (int i = 0; i < instructionLinesLocs.Length; i++)
            {
                instructionLinesLocs[i] = new Vector2(5, INSTRUCTIONS_FIRST_LINE_Y_LOC + INSTRUCTIONS_SPACING * i);
            }

            assassinTimer = new Timer[] { new Timer(3000, false), new Timer(3000, false), new Timer(3000, false) };
            gameTimer = new Timer(30000, true);
            hpRegenTimer = new Timer(4000, true);

            assassinInstructRec = new Rectangle((int)(screenWidth - assassinImg.Width * 0.18 - 5), (int)(instructionLinesLocs[0].Y - assassinImg.Height * 0.09), (int)(assassinImg.Width * 0.18), (int)(assassinImg.Height * 0.18));
            ammoInstructRec = new Rectangle((int)(screenWidth - bulletImg.Width * 0.1 - 10), (int)(instructionLinesLocs[1].Y - bulletImg.Height * 0.05), (int)(bulletImg.Width * 0.1), (int)(bulletImg.Height * 0.1));
            healthInstructRec = new Rectangle((int)(screenWidth - playerHpImg.Width * 0.2 - 5), (int)(instructionLinesLocs[2].Y - playerHpImg.Height * 0.05), (int)(playerHpImg.Width * 0.2), (int)(playerHpImg.Height * 0.2));
            ammoDropInstructRec = new Rectangle((int)(screenWidth - ammoDropImg.Width * 0.1 - 5), (int)(instructionLinesLocs[4].Y - ammoDropImg.Height * 0.08), (int)(ammoDropImg.Width * 0.1), (int)(ammoDropImg.Height * 0.1));
            uavDotInstructRec = new Rectangle((int)(screenWidth - assassinUavDotImg.Width * 0.3 - 5), (int)(instructionLinesLocs[7].Y + assassinUavDotImg.Height * 0.15), (int)(assassinUavDotImg.Width * 0.3), (int)(assassinUavDotImg.Height * 0.3));

            gunShotSnd = Content.Load<SoundEffect>("Audio/Sounds/GunShot");
            missShotSnd = Content.Load<SoundEffect>("Audio/Sounds/HitMarker");
            assassinShotSnd = Content.Load<SoundEffect>("Audio/Sounds/GettingShot");

            buttonClickSnd = Content.Load<SoundEffect>("Audio/Sounds/ButtonClick");
            lossSnd = Content.Load<SoundEffect>("Audio/Sounds/LossSound");

            noAmmoSnd = Content.Load<SoundEffect>("Audio/Sounds/OutOfAmmo");
            ammoPickup = Content.Load<SoundEffect>("Audio/Sounds/AmmoPickup");

            menuBkgMsk = Content.Load<Song>("Audio/Music/MenuBackground");
            gameBkgMsk = Content.Load<Song>("Audio/Music/GameBackground");
        }

        protected override void Update(GameTime gameTime)
        {
            UserInputReader();

            switch (gameState)
            {
                case MENU:
                    MenuScreenButClicker();

                    MoveMouseCursor();
                    
                    if(isFadingIn)
                    {
                        FadeIn();
                    }

                    if(isFadingOut)
                    {
                        FadeOut();
                    }

                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(menuBkgMsk);
                    }

                    break;

                case PRE_GAME:
                    if (!isFadingIn && !isFadingOut)
                    {
                        PreGameButtonsClicker();
                    }
                    else if (isFadingOut)
                    {
                        FadeOut();
                    }
                    else
                    {
                        FadeIn();
                    }

                    MoveMouseCursor();

                    break;

                case INSTRUCTIONS:
                    if (!isFadingIn && !isFadingOut)
                    {
                        InstructButtonClicker();
                    }
                    else if (isFadingOut)
                    {
                        FadeOut();
                    }
                    else
                    {
                        FadeIn();
                    }

                    MoveMouseCursor();

                    break;

                case GAMEPLAY:
                    if (isFadingOut)
                    {
                        FadeOut();
                    }
                    else if (isFadingIn)
                    {
                        FadeIn();
                    }

                    if (!isFadingIn && !isFadingOut)
                    {
                        UpdateGameItems(gameTime);

                        ExitWithEsc(PAUSE);
                    }

                    MouseCollisionChecker();

                    NewAssassinChecker();

                    if (assassinNum >= ASSASSIN_1)
                    {
                        AssassinController(ASSASSIN_1, ASSASSIN_3, ASSASSIN_2);
                    }
                    if (assassinNum >= ASSASSIN_2)
                    {
                        AssassinController(ASSASSIN_2, ASSASSIN_1, ASSASSIN_3);
                    }
                    if (assassinNum >= ASSASSIN_3)
                    {
                        AssassinController(ASSASSIN_3, ASSASSIN_2, ASSASSIN_1);
                    }

                    ScopeLocater();

                    AmmoDropMover();

                    if(hpRegenTimer.IsFinished())
                    {
                        userHealth++;

                        hpRegenTimer.ResetTimer(true);
                    }

                    StatTransChanger();

                    StatBoundaryImplementer();

                    MoveClouds();

                    if (userHealth <= MIN_HEALTH || gameTimer.IsFinished())
                    {
                        EndGame();
                    }
                    else if (userAmmo == MIN_AMMO)
                    {
                        if(!isAmmoDropped)
                        {
                            EndGame();
                        }
                    }

                    break;
                case PAUSE:
                    if (!isFadingIn)
                    {
                        ExitPauseKeys();
                    }
                    else
                    {
                        FadeIn();
                    }

                    MoveMouseCursor();

                    break;

                case EXIT:
                    Exit();

                    break;

                case GAME_OVER:
                    if(!isFadingIn)
                    {
                        MenuButtonClicker();
                    }
                    else
                    {
                        FadeIn();

                        if (screenTrans >= MAX_OPACITY)
                        {
                            userScore = MIN_SCORE;
                            isHighScoreAcheived = false;
                        }
                    }
                    
                    MoveMouseCursor();

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(35, 31, 32));

            spriteBatch.Begin();

            switch (gameState)
            {
                case MENU:
                    spriteBatch.Draw(nonGameBorderImg, nonGameBorderRec, Color.White);

                    spriteBatch.DrawString(titleFont, gameTitle, gameTitleShadowLoc, Color.Red);
                    spriteBatch.DrawString(titleFont, gameTitle, gameTitleLoc, Color.White);

                    spriteBatch.Draw(playButtonImg, playButtonRec, Color.White * playButtonTrans);
                    spriteBatch.Draw(instructionsButtonImg, instructionsButtonRec, Color.White * instructionsButtonTrans);
                    spriteBatch.Draw(exitButtonImg, exitButtonRec, Color.White * exitButtonTrans);

                    spriteBatch.DrawString(userStatsFonts, highScoreUserText + highScore, highScoreUserTextLoc, Color.White);

                    spriteBatch.Draw(mouseClickImg, mouseClickRec, Color.White);
                    
                    break;

                case PRE_GAME:
                    spriteBatch.Draw(nonGameBorderImg, nonGameBorderRec, Color.White);

                    spriteBatch.Draw(goButtonImg, goButtonRec, Color.White * goButtonTrans);

                    spriteBatch.Draw(mouseClickImg, mouseClickRec, Color.White);

                    break;

                case INSTRUCTIONS:
                    spriteBatch.DrawString(titleFont, instructionsTitle, instructionsTitleShadLoc, Color.DarkRed);
                    spriteBatch.DrawString(titleFont, instructionsTitle, instructionsTitleLoc, Color.White);

                    for (int i = 0; i < instructionLinesLocs.Length; i++)
                    {
                        spriteBatch.DrawString(userStatsFonts, instructionsLines[i], instructionLinesLocs[i], Color.White);
                    }

                    spriteBatch.Draw(backButtonImg, backButtonInstrucRec, Color.White * backButtonTrans);
                    spriteBatch.Draw(assassinImg, assassinInstructRec, Color.White);
                    spriteBatch.Draw(bulletImg, ammoInstructRec, Color.White);
                    spriteBatch.Draw(playerHpImg, healthInstructRec, Color.White);
                    spriteBatch.Draw(ammoDropImg, ammoDropInstructRec, Color.White);
                    spriteBatch.Draw(assassinUavDotImg, uavDotInstructRec, Color.White);

                    spriteBatch.Draw(mouseClickImg, mouseClickRec, Color.White);

                    break;

                case GAMEPLAY:
                    DrawGameplay();

                    break;

                case PAUSE:
                    DrawGameplay();
                    spriteBatch.Draw(blackScreenImg, blackScreenRec, Color.White * blackScreenPauseTrans);

                    spriteBatch.DrawString(titleFont, pauseText, pauseTextLoc, Color.White);

                    spriteBatch.DrawString(userStatsFonts, backToGameText, backtoGameTextLoc, Color.White);
                    spriteBatch.DrawString(userStatsFonts, exitGameText, exitGameTextLoc, Color.White);

                    spriteBatch.Draw(mouseClickImg, mouseClickRec, Color.White);

                    break;

                case GAME_OVER:
                    spriteBatch.Draw(nonGameBorderImg, nonGameBorderRec, Color.White);

                    if (isHighScoreAcheived)
                    {
                        spriteBatch.DrawString(titleFont, highScoreTitle, highScoreTitleLoc, Color.Red);
                        spriteBatch.DrawString(titleFont, highScoreTitle, highScoreTitleShadLoc, Color.White);

                        spriteBatch.DrawString(userStatsFonts, endOfGameWinMessage, endOfGameWinMessageLoc, Color.Red);
                        spriteBatch.DrawString(userStatsFonts, endOfGameWinMessage, endOfGameWinShadMessageLoc, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(titleFont, loseTitle, loseTitleLoc, Color.Red);
                        spriteBatch.DrawString(titleFont, loseTitle, loseTitleShadLoc, Color.White);

                        spriteBatch.DrawString(userStatsFonts, endOfGameLossMessage, endOfGameLossMessageLoc, Color.Red);
                        spriteBatch.DrawString(userStatsFonts, endOfGameLossMessage, endOfGameLossMessageShadLoc, Color.White);
                    }

                    spriteBatch.Draw(blackScreenImg, userInfoSquareRec, Color.White * 0.5f);
                    spriteBatch.DrawString(userStatsFonts, endOfGameScore + userScore, endOfGameScoreLoc, Color.White);
                    spriteBatch.DrawString(userStatsFonts, highScoreUserText + highScore, endOfGameHighScoreLoc, Color.White);

                    spriteBatch.Draw(menuButtonImg, menuButtonRec, Color.White * menuButtonTrans);

                    spriteBatch.Draw(mouseClickImg, mouseClickRec, Color.White);

                    break;
            }

            spriteBatch.Draw(blackScreenImg, blackScreenRec, Color.White * screenTrans);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RandomizeAssassin(int assassinNum, int prevAssassinNum, int nextAssassinNum)
        {
            if (!isAssassinRandom[assassinNum])
            {
                prevRandomLoc[assassinNum] = randomLoc[assassinNum];

                while (randomLoc[assassinNum] == prevRandomLoc[assassinNum] || randomLoc[assassinNum] == randomLoc[prevAssassinNum] || randomLoc[assassinNum] == randomLoc[nextAssassinNum])
                {
                    randomLoc[assassinNum] = rng.Next(0, assassinLocs.Length);
                }

                if (randomLoc[assassinNum] == ROOF_RAND_ARRAY_LOC)
                {
                    assassinLocs[ROOF_RAND_ARRAY_LOC].X = rng.Next(82, 359);
                }
                else if (randomLoc[assassinNum] == FLOOR_RAND_ARRAY_LOC_LEFT)
                {
                    assassinLocs[FLOOR_RAND_ARRAY_LOC_LEFT].X = rng.Next(0, 78);
                }
                else if (randomLoc[assassinNum] == FLOOR_RAND_ARRAY_LOC_RIGHT)
                {
                    assassinLocs[FLOOR_RAND_ARRAY_LOC_RIGHT].X = rng.Next(366, screenWidth - assassinAnim[assassinNum].destRec.Width);
                }

                assassinAnim[assassinNum] = new Animation(assassinSprite, 6, 1, 6, 0, 0, 0, 16, assassinLocs[randomLoc[assassinNum]], 0.35f, false);

                assassinUavDotRecs[assassinNum].X = (int)(assassinAnim[assassinNum].destRec.X * 0.135);
                assassinUavDotRecs[assassinNum].Y = (int)(assassinAnim[assassinNum].destRec.Y * 0.136);

                isAssassinRandom[assassinNum] = true;
            }
        }

        private void AssassinMouseCollision(int assassinNum)
        {
            userAmmo--;
            userScore += 100;

            isAssassinRandom[assassinNum] = false;
            assassinTimer[assassinNum].ResetTimer(true);

            if (!isAmmoDropped)
            {
                ammoDropChance = rng.Next(1, 101);

                if (ammoDropChance <= 50)
                {
                    ammoDropRec.X = rng.Next(0 + MAX_SWIRVLE, screenWidth - ammoDropRec.Width - MAX_SWIRVLE);
                    ammoDropXLoc = ammoDropRec.X;

                    isAmmoDropped = true;
                }
            }
        }

        private void AssassinTimerRessetter(int assassinNum)
        {
            if (assassinTimer[assassinNum].IsFinished())
            {
                isAssassinRandom[assassinNum] = false;

                userHealth -= 5;
                userScore -= 100;

                assassinShotSnd.CreateInstance().Play();

                assassinTimer[assassinNum].ResetTimer(true);
            }
        }

        private void ScopeLocater()
        {
            scopeRec.X = (int)(mouse.X - scopeImg.Width * 0.5);
            scopeRec.Y = (int)(mouse.Y - scopeImg.Height * 0.5);

            if (mouse.X <= 0)
            {
                scopeRec.X = (int)(-scopeRec.Width * 0.5);
            }
            else if (mouse.X >= screenWidth)
            {
                scopeRec.X = (int)(screenWidth - scopeRec.Width * 0.5);
            }

            if (mouse.Y <= 0)
            {
                scopeRec.Y = (int)(-scopeRec.Height * 0.5);
            }
            else if (mouse.Y >= screenHeight - HUD_HEIGHT)
            {
                scopeRec.Y = (int)(screenHeight - HUD_HEIGHT - scopeRec.Height * 0.5);
            }
        }

        private void StatBoundaryImplementer()
        {
            if (userHealth > MAX_HEALTH)
            {
                userHealth = MAX_HEALTH;
            }
            if (userAmmo > MAX_AMMO)
            {
                userAmmo = MAX_AMMO;
            }
            else if(userAmmo < MIN_AMMO)
            {
                userAmmo = MIN_AMMO;
            }
        }

        private void StatTransChanger()
        {
            for (int i = 0; i <= userAmmo; i++)
            {
                if (i <= bulletRecTrans.Length - 1)
                {
                    bulletRecTrans[i] = 1;
                }
            }

            for (int i = bulletRecTrans.Length - 1; i >= userAmmo; i--)
            {
                if (i >= 0)
                {
                    bulletRecTrans[i] = 0.3f;
                }
            }

            for (int i = 0; i <= userHealth; i++)
            {
                if (i <= playerHpRec.Length - 1)
                {
                    playerHpRecTrans[i] = 1;
                }
            }

            for (int i = playerHpRec.Length - 1; i >= userHealth; i--)
            {
                if (i >= 0)
                {
                    playerHpRecTrans[i] = 0.2f;
                }
            }

            bloodTrans = (float)(Math.Abs(userHealth - MAX_HEALTH) * 0.1);
        }

        private void AmmoDropMover()
        {
            if (isAmmoDropped)
            {
                ammoDropRec.Y += AMMO_SPEED;

                ammoDropAngle = ammoDropAngle + .1;
                ammoDropRec.X = (int)(ammoDropXLoc + (Math.Sin(ammoDropAngle) * 5));

                if (ammoDropRec.Y >= screenHeight - HUD_HEIGHT)
                {
                    isAmmoDropped = false;

                    ammoDropRec.X = 0;
                    ammoDropRec.Y = -200;
                }
            }

            ammoDropUavDotRec.X = (int)(ammoDropRec.X * 0.135);
            ammoDropUavDotRec.Y = (int)((ammoDropRec.Y + ammoDropRec.Height) * 0.136);
        }

        private void TimerActivator(int assassinNum)
        {
            if (assassinTimer[assassinNum].IsInactive())
            {
                assassinTimer[assassinNum].Activate();
            }
        }

        private void DrawGameplay()
        {
            for (int i = 0; i < cloudRec.Length; i++)
            {
                spriteBatch.Draw(cloudImg, cloudRec[i], Color.White);
            }

            spriteBatch.Draw(blackScreenImg, windowBlackOutRec, Color.White);

            for (int i = 0; i < assassinAnim.Length - 1; i++)
            {
                assassinAnim[i].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            }

            spriteBatch.Draw(sniperTowerImg, sniperTowerRec, Color.White);

            spriteBatch.Draw(ammoDropImg, ammoDropRec, Color.White);

            spriteBatch.Draw(scopeImg, scopeRec, Color.White);

            spriteBatch.Draw(bloodScreenImg, bloodScreenRec, Color.White * bloodTrans);

            spriteBatch.Draw(uavMapImg, uavMapRec, Color.White * 0.5f);

            for (int i = 0; i < assassinAnim.Length - 1; i++)
            {
                spriteBatch.Draw(assassinUavDotImg, assassinUavDotRecs[i], Color.White);
            }

            spriteBatch.Draw(blackScreenImg, hudBlackOutRec, Color.White);

            spriteBatch.Draw(playerHpImg, playerHpRec[0], Color.White * playerHpRecTrans[0]);
            spriteBatch.Draw(playerHpImg, playerHpRec[1], Color.White * playerHpRecTrans[1]);
            spriteBatch.Draw(playerHpImg, playerHpRec[2], Color.White * playerHpRecTrans[2]);
            spriteBatch.Draw(playerHpImg, playerHpRec[3], Color.White * playerHpRecTrans[3]);
            spriteBatch.Draw(playerHpImg, playerHpRec[4], Color.White * playerHpRecTrans[4]);
            spriteBatch.Draw(playerHpImg, playerHpRec[5], Color.White * playerHpRecTrans[5]);
            spriteBatch.Draw(playerHpImg, playerHpRec[6], Color.White * playerHpRecTrans[6]);
            spriteBatch.Draw(playerHpImg, playerHpRec[7], Color.White * playerHpRecTrans[7]);
            spriteBatch.Draw(playerHpImg, playerHpRec[8], Color.White * playerHpRecTrans[8]);
            spriteBatch.Draw(playerHpImg, playerHpRec[9], Color.White * playerHpRecTrans[9]);

            spriteBatch.Draw(bulletImg, bulletRec[0], Color.White * bulletRecTrans[0]);
            spriteBatch.Draw(bulletImg, bulletRec[1], Color.White * bulletRecTrans[1]);
            spriteBatch.Draw(bulletImg, bulletRec[2], Color.White * bulletRecTrans[2]);
            spriteBatch.Draw(bulletImg, bulletRec[3], Color.White * bulletRecTrans[3]);
            spriteBatch.Draw(bulletImg, bulletRec[4], Color.White * bulletRecTrans[4]);
            spriteBatch.Draw(bulletImg, bulletRec[5], Color.White * bulletRecTrans[5]);
            spriteBatch.Draw(bulletImg, bulletRec[6], Color.White * bulletRecTrans[6]);
            spriteBatch.Draw(bulletImg, bulletRec[7], Color.White * bulletRecTrans[7]);
            spriteBatch.Draw(bulletImg, bulletRec[8], Color.White * bulletRecTrans[8]);
            spriteBatch.Draw(bulletImg, bulletRec[9], Color.White * bulletRecTrans[9]);

            spriteBatch.Draw(ammoDropUavDotImg, ammoDropUavDotRec, Color.White);

            spriteBatch.DrawString(userStatsFonts, Convert.ToString(userScore), scorePos, Color.White);
            spriteBatch.DrawString(userStatsFonts, gameTimer.GetTimeRemainingAsString(Timer.FORMAT_SEC_MIL), gameTimerPos, Color.White);

            spriteBatch.DrawString(highScoreFont, highScoreLabel, highScoreLabelPos, Color.White);
            spriteBatch.DrawString(userStatsFonts, Convert.ToString(highScore), highScorePos, Color.White);
        }

        private void RESTART_GAMEPLAY()
        {
            userAmmo = MAX_AMMO;
            userHealth = MAX_HEALTH;

            assassinNum = ASSASSIN_1;

            gameTimer.ResetTimer(true);

            ammoDropRec.X = 0;
            ammoDropRec.Y = -200;

            curGameHighScore = 0;

            isAmmoDropped = false;

            for (int i = 0; i < isAssassinRandom.Length - 1; i++)
            {
                isAssassinRandom[i] = false;

                assassinTimer[i].Deactivate();

                assassinAnim[i].destRec.X = 0;
                assassinAnim[i].destRec.Y = -200;

                assassinUavDotRecs[i].X = 0;
                assassinUavDotRecs[i].Y = -200;
            }
        }

        private void AnnimateAssassins(int assassinNum)
        {
            if (assassinTimer[assassinNum].GetTimeRemaining() <= 1700)
            {
                assassinAnim[assassinNum].isAnimating = true;
            }
            else
            {
                assassinAnim[assassinNum].isAnimating = false;
                assassinAnim[assassinNum].curFrame = 0;
            }
        }

        private void MenuScreenButClicker()
        {
            if(!isFadingIn && !isFadingOut)
            {
                if (playButtonRec.Contains(mouse.Position))
                {
                    playButtonTrans = 0.5f;

                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        isFadingIn = true;
                        nextGameState = PRE_GAME;

                        buttonClickSnd.CreateInstance().Play();
                    }
                }
                else
                {
                    playButtonTrans = 1;
                }

                if (instructionsButtonRec.Contains(mouse.Position))
                {
                    instructionsButtonTrans = 0.5f;

                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        isFadingIn = true;
                        nextGameState = INSTRUCTIONS;

                        buttonClickSnd.CreateInstance().Play();
                    }
                }
                else
                {
                    instructionsButtonTrans = 1;
                }

                if (exitButtonRec.Contains(mouse.Position))
                {
                    exitButtonTrans = 0.5f;

                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                    {
                        isFadingIn = true;
                        nextGameState = EXIT;
                    }
                }
                else
                {
                    exitButtonTrans = 1;
                }
            }
        }

        private void MenuButtonClicker()
        {
            if (menuButtonRec.Contains(mouse.Position))
            {
                menuButtonTrans = 0.5f;

                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    nextGameState = MENU;
                    isFadingIn = true;

                    buttonClickSnd.CreateInstance().Play();

                    RESTART_GAMEPLAY();
                }
            }
            else
            {
                menuButtonTrans = 1;
            }
        }

        private void PreGameButtonsClicker()
        {
            if (goButtonRec.Contains(mouse.Position))
            {
                goButtonTrans = 0.5f;

                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    isFadingIn = true;
                    nextGameState = GAMEPLAY;

                    buttonClickSnd.CreateInstance().Play();

                    if (MediaPlayer.State == MediaState.Playing)
                    {
                        MediaPlayer.Pause();
                    }
                    if (MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Play(gameBkgMsk);
                    }
                }
            }
            else
            {
                goButtonTrans = 1;
            }
        }

        private void InstructButtonClicker()
        {
            if (backButtonInstrucRec.Contains(mouse.Position))
            {
                backButtonTrans = 0.5f;

                if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
                {
                    isFadingIn = true;
                    nextGameState = MENU;

                    buttonClickSnd.CreateInstance().Play();
                }
            }
            else
            {
                backButtonTrans = 1;
            }
        }

        private void UpdateGameItems(GameTime gameTime)
        {
            gameTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            hpRegenTimer.Update(gameTime.ElapsedGameTime.TotalMilliseconds);

            for (int i = 0; i < 3; i++)
            {
                assassinAnim[i].Update(gameTime);
                assassinTimer[i].Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        private void AssassinClicker()
        {
            if (assassinAnim[ASSASSIN_1].destRec.Contains(mouse.Position))
            {
                if (assassinNum >= ASSASSIN_1)
                {
                    AssassinMouseCollision(ASSASSIN_1);
                }
            }
            else if (assassinAnim[ASSASSIN_2].destRec.Contains(mouse.Position))
            {
                if (assassinNum >= ASSASSIN_2)
                {
                    AssassinMouseCollision(ASSASSIN_2);
                }
            }
            else if (assassinAnim[ASSASSIN_3].destRec.Contains(mouse.Position))
            {
                if (assassinNum >= ASSASSIN_3)
                {
                    AssassinMouseCollision(ASSASSIN_3);
                }
            }
        }

        private void AmmoDropFinishedActions()
        {
            isAmmoDropped = false;

            ammoDropRec.X = 0;
            ammoDropRec.Y = -200;

            userAmmo += 3;
        }

        private void MoveMouseCursor()
        {
            mouseClickRec.X = mouse.X;
            mouseClickRec.Y = mouse.Y;
        }

        private void UserInputReader()
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();

            prevKb = kb;
            kb = Keyboard.GetState();
        }

        private void MouseCollisionChecker()
        {
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                if (ammoDropRec.Contains(mouse.Position))
                {
                    AmmoDropFinishedActions();

                    ammoPickup.CreateInstance().Play();
                }
                else if(userAmmo > MIN_AMMO)
                {
                    if (assassinAnim[ASSASSIN_1].destRec.Contains(mouse.Position) || assassinAnim[ASSASSIN_2].destRec.Contains(mouse.Position) || assassinAnim[ASSASSIN_3].destRec.Contains(mouse.Position))
                    {
                        AssassinClicker();
                        gunShotSnd.CreateInstance().Play();
                    }
                    else
                    {
                        userAmmo--;

                        userScore -= 50;
                        missShotSnd.CreateInstance().Play();
                    }
                }
                else
                {
                    noAmmoSnd.CreateInstance().Play();
                }
            }
        }

        private void AssassinController(int assassinNum, int prevAssassinNum, int nextAssassinNum)
        {
            TimerActivator(assassinNum);
            AssassinTimerRessetter(assassinNum);
            RandomizeAssassin(assassinNum, nextAssassinNum, prevAssassinNum);
            AnnimateAssassins(assassinNum);
        }

        private void ExitWithEsc(int nextGameState)
        {
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
            {
                buttonClickSnd.CreateInstance().Play();

                gameState = nextGameState;
            }
        }

        private void ExitPauseKeys()
        {
            if (kb.IsKeyDown(Keys.Escape) && !prevKb.IsKeyDown(Keys.Escape))
            {
                buttonClickSnd.CreateInstance().Play();

                gameState = GAMEPLAY;
            }
            else if (kb.IsKeyDown(Keys.Enter) && !prevKb.IsKeyDown(Keys.Enter))
            {
                isFadingIn = true;
                nextGameState = MENU;

                buttonClickSnd.CreateInstance().Play();

                RESTART_GAMEPLAY();

                userScore = MIN_SCORE;

                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Pause();
                }
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Play(menuBkgMsk);
                }
            }
        }

        private void MoveClouds()
        {
            for (int i = 0; i < cloudRec.Length; i++)
            {
                cloudRec[i].X += CLOUD_SPEED;

                if (cloudRec[i].X <= -cloudRec[i].Width)
                {
                    cloudRec[i].X = screenWidth;
                }
            }
        }

        private void NewAssassinChecker()
        {
            if (userScore > curGameHighScore)
            {
                curGameHighScore = userScore;
            }
            if (curGameHighScore >= SECOND_ASSASSIN_SCORE)
            {
                assassinNum = ASSASSIN_2;
            }
            if (curGameHighScore >= THIRD_ASSASSIN_SCORE)
            {
                assassinNum = ASSASSIN_3;
            }
        }

        private void EndGame()
        {
            if (userScore > highScore)
            {
                highScore = userScore;

                isHighScoreAcheived = true;
            }

            RESTART_GAMEPLAY();

            gameState = GAME_OVER;

            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Play(menuBkgMsk);
            }

            lossSnd.CreateInstance().Play();
        }

        private void FadeIn()
        {
            screenTrans += SCREEN_TRANS_INCREASE;

            if(screenTrans >= MAX_OPACITY)
            {
                gameState = nextGameState;

                isFadingIn = false;
                isFadingOut = true;
            }
        }

        private void FadeOut ()
        {
            screenTrans -= SCREEN_TRANS_INCREASE;

            if (screenTrans <= MIN_OPACITY)
            {
                isFadingOut = false;

                gameState = nextGameState;
            }
        }
    }    
}