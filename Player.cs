using System;
using SplashKitSDK;


class Player
{
    private Bitmap _playerBitmap;
    public double X { get; private set; }
    public double Y { get; private set; }
    private Vector2D WaveVelocity { get; set; }
    private double _angle;
    
    private bool flyaway = false;
    public bool quit { get; set; }

    public int lives { get; set; }

    public double mX { get; set; }
    public double mY { get; set; }

    public bool shoot = false;

    public bool isAlive()
    {
        return (lives > 0);
    }


    public int Width
    {
        get
        {
            return _playerBitmap.Width;
        }
    }

    public int Height
    {
        get
        {
            return _playerBitmap.Height;
        }
    }



    public Player(Window gameWindow)
    {
        _playerBitmap = new Bitmap("player", "player.png");


        X = ((gameWindow.Width - Width) / 4);
        Y = ((gameWindow.Height - Height) / 2);
        _angle = 0;
        
        lives = 5;
        quit = false;


    }



    public void Draw(Window gameWindow)
    {
        _playerBitmap.Draw(X, Y, SplashKit.OptionRotateBmp(_angle));
    
       
        if (!isAlive())
        {
            quit = true;
        }

    }

    

    public void StayOnWindow(Window gameWindow, Wave wave)
    {
        const int GAP = 10;

        if (flyaway == false)
        {

            if (X < GAP) X = GAP;

            if (gameWindow.Width - GAP < (X + Width)) X = gameWindow.Width - GAP - Width;

            if ((Y - 30) < GAP) Y = GAP + 30;

            if (gameWindow.Height - GAP < (Y + Height)) Y = gameWindow.Height - GAP - Height;
        }
        else if (X - 1000 > gameWindow.Width)
        {
            lives = 0;
        }
    }

    public bool CollidedWith(Obstacle _TestObstacle)
    {
        return _playerBitmap.CircleCollision(X, Y, _TestObstacle.CollisionCircle);
    }

    public void Rotate(double amount)
    {
        _angle = (_angle + amount) % 360;
    }

    public void Move(double amountForward)
    {
        Vector2D movement = new Vector2D();
        Matrix2D rotation = SplashKit.RotationMatrix(_angle);

        movement.X += amountForward;

        movement = SplashKit.MatrixMultiply(rotation, movement);

        X -= movement.X;
        Y -= movement.Y;
    }

    
    public void WaveMotion(Wave wave)
    {

        int proximity;

        if (wave.lipX - X < 50)
        {
            proximity = 800;
        }
        else if (wave.lipX - X < 300)
        {
            proximity = 500;
        }
        else { proximity = 400; }


        if ((wave.lipX + (Y - wave.lipY)) < X)
        {
            flyaway = true;

        }

        if (flyaway == false)
        {
            Point2D fromPt = new Point2D()
            {
                X = X,
                Y = Y
            };

            Point2D toPt = new Point2D()
            {
                X = wave.lipX,
                Y = wave.lipY
            };

            Vector2D dir;


            dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));
            WaveVelocity = SplashKit.VectorMultiply(dir, (proximity / ((wave.lipX - X) + (Y - wave.lipY)) + 2.5));

            X = X + WaveVelocity.X;
            Y = Y + WaveVelocity.Y;
        }
        else if (flyaway == true)
        {
            X += 10;
            Y += 30;

        }

    }
}