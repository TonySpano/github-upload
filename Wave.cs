using System;
using SplashKitSDK;
class Wave
{
    public double X { get; private set; }
    public double Y { get; private set; }
    public double lipX { get; private set; }
    public double lipY { get; private set; }
    public double screenX;
    private Bitmap _waveBitmap;
    int dir = 0;

    Timer waveTimer = new Timer("wave timer");

    public Wave(Window gameWindow)
    {
        _waveBitmap = new Bitmap("wave1", "wave1.png");

        X = -350;
        Y = 0;
        lipX = X + 900;
        lipY = 120;
        screenX = gameWindow.Width;
        waveTimer.Start();

    }
    public void Draw(Window gameWindow, int wavepic)
    {
        Move();

        _waveBitmap = new Bitmap($"wave{wavepic}", $"wave{wavepic}.png");
                

        gameWindow.DrawBitmap(_waveBitmap, X, Y);

    }

    private void Move()
    {


        // determine if the wave moves left or right
        if (SplashKit.Rnd() < 0.01)
        {
            dir = -dir + 1;
        }

        if (dir == 1)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }


    }

    private void MoveLeft()
    {
        X = X - 1;

        if ((X - 400) < -(_waveBitmap.Width - screenX))
        {
            X = -(_waveBitmap.Width - screenX) + 400;
        }

        lipX = X + 900;
        

    }

    private void MoveRight()
    {
        X = X + 1;

        if (X > 0)
        {
            X = 0;
        }

        lipX = X + 900;
        

    }
}
