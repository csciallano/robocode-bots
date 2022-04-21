using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;
using System;

/// <summary>
/// The wonderful Cleminem bot
/// </summary>
public class CleminemBot : Bot
{
    /// <summary>
    /// The tank body color
    /// </summary>
    private Color bodyColor = Color.FromString("#FF69B4");

    /// <summary>
    /// The tank turret color
    /// </summary>
    private Color turretColor = Color.Black;

    /// <summary>
    /// The tank radar color
    /// </summary>
    private Color radarColor = Color.Blue;

    /// <summary>
    /// The tank scan color
    /// </summary>
    private Color scanColor = Color.White;

    /// <summary>
    /// The tank bullet color
    /// </summary>
    private Color bulletColor = Color.Red;

    /// <summary>
    /// Gets a value indicating if we are scanning
    /// </summary>
    private bool isScanning; // flag set when scanning

    /// <summary>
    /// Number of iterations without scans
    /// </summary>
    private int iterationsWithoutScannedBots = 0;

    // Constructor, which loads the bot config file
    public CleminemBot() : base(BotInfo.FromFile("CleminemBot.json"), new System.Uri("ws://localhost:7654/"), "ulhmEmwWmsVdfm5LIsKcRA") 
    { 
    }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = bodyColor;
        TurretColor = turretColor;
        RadarColor = radarColor;
        ScanColor = scanColor;
        BulletColor = bulletColor;

        // Repeat while the bot is running
        while (IsRunning)
        {
            iterationsWithoutScannedBots++;
            if (iterationsWithoutScannedBots > 5)
            {
                iterationsWithoutScannedBots = 0;
                TurnRight(25);
                Forward(200);
            }

            Forward(100);
            TurnGunRight(360);
            TurnRadarRight(360);
            Back(100);
            TurnGunRight(360);
        }
    }

    // We saw another bot -> fire!
    public override void OnScannedBot(ScannedBotEvent e)
    {
        iterationsWithoutScannedBots = 0;
        isScanning = true; // we started scanning

        // Calculate direction of the scanned bot and bearing to it for the gun
        double bearingFromGun = GunBearingTo(e.X, e.Y);

        // Turn the gun toward the scanned bot
        TurnGunLeft(bearingFromGun);

        // If it is close enough, fire!
        if (Math.Abs(bearingFromGun) <= 3 && GunHeat == 0)
        {
            Fire(Math.Min(3 - Math.Abs(bearingFromGun), Energy - .1));
            Console.WriteLine("Fire!");
        }

        isScanning = false; // we stopped scanning
    }

    /// <summary>
    /// We hit a wall.
    /// </summary>
    /// <param name="e"></param>
    public override void OnHitWall(HitWallEvent e)
    {
        Back(20);
        TurnRight(180);
    }

    /// <summary>
    /// We hit a bot : FIRE!
    /// </summary>
    /// <param name="botHitBotEvent">The event</param>
    public override void OnHitBot(HitBotEvent botHitBotEvent)
    {
        Fire(3);
    }

    /// <summary>
    /// When a bullet hits a bullet
    /// </summary>
    /// <param name="bulletHitBulletEvent"></param>
    public override void OnBulletHitBullet(BulletHitBulletEvent bulletHitBulletEvent)
    {
        TurnLeft(90);
        Forward(150);
    }

    /// <summary>
    // We were hit by a bullet -> turn perpendicular to the bullet
    /// </summary>
    /// <param name="evt">The event</param>
    public override void OnHitByBullet(BulletHitBotEvent evt)
    {
        // Calculate the bearing to the direction of the bullet
        double bearing = CalcBearing(evt.Bullet.Direction);

        // Turn 90 degrees to the bullet direction based on the bearing
        TurnLeft(90 - bearing);
        Back(100);
    }
}