using System;
using SplashKitSDK;
using System.Collections.Generic;
using System.IO;


class SurfGame
{
    private List<Obstacle> _Obstacles = new List<Obstacle>();
    private List<Obstacle> _LostObstacles = new List<Obstacle>();

    private Player _Player;
    private Window _GameWindow;
    private Wave _Wave;

    int wavepic;
    bool flag = false;

    double _speed;
    double accrotation;

    Timer gameTimer = new Timer("Game Timer");


    public bool Quit
    {
        get
        {
            return _Player.quit;
        }
    }

    public SurfGame(Window gameWindow)
    {
        _GameWindow = gameWindow;

        _Player = new Player(_GameWindow);
        _Wave = new Wave(_GameWindow);
        Timer gameTimer = new Timer("Game Timer");
        wavepic = 1;
        _speed = 5;
        
        
        gameTimer.Start();

    }

    
    public void RandomObstacle(Window _GameWindow)
    {
        double rand = SplashKit.Rnd();

        if (rand < 0.2)
        {
            Obstacle newObstacle = new Shark(_GameWindow, _Player);
            _Obstacles.Add(newObstacle);
        }
        else if (rand < 0.4)
        {
            Obstacle newObstacle = new Turtle(_GameWindow, _Player);
            _Obstacles.Add(newObstacle);
        }
        else if (rand < 0.6)
        {
            Obstacle newObstacle = new BodyBoard(_GameWindow, _Player);

            _Obstacles.Add(newObstacle);
        }
        else
        {
            Obstacle newObstacle = new Waste(_GameWindow, _Player);

            _Obstacles.Add(newObstacle);
        }

    }

    public void Draw()
    {
        _GameWindow.Clear(Color.White);
        // set wave animation picture and slow down player
        if (gameTimer.Ticks / 1000 % 2 == 0 && !flag)
        {
            
            wavepic ++;
            flag = true;
            if (wavepic == 3)
            {
                wavepic = 1;
            }

            // _speed -= 2;

        } else if (gameTimer.Ticks / 1000 % 2 != 0)
        {
            flag = false;
        }

        _Wave.Draw(_GameWindow, wavepic);
        DrawStuff();
        

        foreach (Obstacle obst in _Obstacles)
        {
            obst.Draw(_GameWindow);
        }

        _Player.Move(_speed);

        _Player.Draw(_GameWindow);

        _GameWindow.DrawText($"{gameTimer.Ticks / 1000}", Color.DarkGreen, 200, 40);

        _GameWindow.Refresh(60);


    }

    public void HandleInput()
    {
        
        if (SplashKit.KeyDown(KeyCode.EscapeKey)) _Player.quit = true;

        if (SplashKit.KeyDown(KeyCode.SpaceKey) && accrotation >= 360)
        {
            _speed = 20;
            accrotation = -100;
        }

        if (SplashKit.KeyDown(KeyCode.UpKey) && _speed < 9.9)
        {           
           _speed += 0.1;        

        }else if (_speed > 10)
        {
            _speed -=0.15;   
        }else _speed -=0.05;

        if (SplashKit.KeyDown(KeyCode.DownKey))
        {
           if (_speed > 0)
            {_speed -= 0.1;} 
        }

        if (SplashKit.KeyDown(KeyCode.LeftKey))
        {
            _Player.Rotate(-(_speed/3));
            if (_speed > 9) accrotation += _speed/3;
        }
        else if (SplashKit.KeyDown(KeyCode.RightKey))
        {
            _Player.Rotate(_speed/3);
            if (_speed > 9) accrotation += _speed/3;
        } else if (accrotation > 0) accrotation -= 1;

        if (accrotation >= 360)
        {
            accrotation = 365;
        }
        _Player.StayOnWindow(_GameWindow, _Wave);
        SplashKit.ProcessEvents();
    }

    public void Update()
    {

        _Player.WaveMotion(_Wave);


        if (SplashKit.Rnd() < 0.02)
        {
            RandomObstacle(_GameWindow);
        }

        foreach (Obstacle obst in _Obstacles)
        {
            obst.WaveMotion(_Wave);
            obst.Update();

        }



        CheckCollisions();

    }

    public void FinalScore()
    {
        int readHS;
        
        readHS = Convert.ToInt32(File.ReadAllText("hs.txt"));
        string hsText = ($"High Score: {readHS} seconds");        

        if (gameTimer.Ticks / 1000 > readHS )
        {
            hsText = ($"New High Score!");
        using (StreamWriter highscore = new StreamWriter("hs.txt"))
        {
            highscore.WriteLine($"{gameTimer.Ticks / 1000}");
        }
        }
        readHS = Convert.ToInt32(File.ReadAllText("hs.txt"));

        SplashKit.FillRectangleOnWindow(_GameWindow, Color.LightGoldenrodYellow, 100, 100, 300, 300);
        _GameWindow.DrawText($"You survived for {gameTimer.Ticks / 1000} seconds", Color.Red, "FontStyle.NormalFont", 200, 160, 250);
        _GameWindow.DrawText($"{hsText}", Color.Red, "FontStyle.NormalFont", 200, 160, 300);

        gameTimer.Stop();
        _GameWindow.Refresh();
    }

    private void DrawStuff()
    {
        // draw life circles
         for (int i = 0; i < _Player.lives; i++)
        {
            int x = 20 + 30 * i;
            _GameWindow.FillCircle(Color.Red, x, 20, 10);
        }

        // draw accelerator bar
        
        if (_speed > 0 && _speed <= 10)
        {
            _GameWindow.FillRectangle(Color.Yellow, 750, 20, (_speed * 45), 40);
        } 
        if (_speed > 10)
        {            
            _GameWindow.FillRectangle(Color.Red, 750, 20, (450), 40);
        } 
        _GameWindow.DrawRectangle(Color.Black, 750, 20, 450, 40);
        

        // draw accumulated rotation bar        
        if (accrotation > 0 && accrotation < 365)
        {
            _GameWindow.FillRectangle(Color.Green, 750, 70, ((accrotation/360) * 450), 40);
        } 
        if (accrotation >= 365)
        {            
            _GameWindow.FillRectangle(Color.Purple, 750, 70, 450, 40);
        } 
        _GameWindow.DrawRectangle(Color.Black, 750, 70, 450, 40);
    }
    private void CheckCollisions()
    {

        foreach (Obstacle obst in _Obstacles)
        {
            if (_Player.CollidedWith(obst) || (obst.IsOffScreen(_GameWindow)))
            {
                _LostObstacles.Add(obst);

            }


            if (_Player.CollidedWith(obst))
            {
                // _Player.lives--;
                obst.isHit(_GameWindow);
            }

        }

        foreach (Obstacle lobst in _LostObstacles)
        {
            _Obstacles.Remove(lobst);
        }

    }

}