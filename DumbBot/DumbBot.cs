using Robocode.TankRoyale.BotApi;
using Robocode.TankRoyale.BotApi.Events;

// ------------------------------------------------------------------
// TrackFire
// ------------------------------------------------------------------
// A sample bot original made for Robocode by Mathew Nelson.
// Ported to Robocode Tank Royale by Flemming N. Larsen.
//
// Sits still. Tracks and fires at the nearest bot it sees.
// ------------------------------------------------------------------
public class DumbBot : Bot
{
    // Constructor, which loads the bot config file
    public DumbBot() : base(BotInfo.FromFile("DumbBot.json"), new System.Uri("ws://localhost:7654/"), "ulhmEmwWmsVdfm5LIsKcRA") { }

    // Called when a new round is started -> initialize and do some movement
    public override void Run()
    {
        // Set colors
        BodyColor = Color.Red;
        TurretColor = Color.Red;
        RadarColor = Color.Red;
        ScanColor = Color.Red;
        BulletColor = Color.Red;

        // Loop while running
        while (IsRunning)
        {
            Forward(2);
            TurnLeft(30);
            Forward(10);
            TurnRight(60);
        }
    }

    // We scanned another bot -> we have a target, so go get it
    public override void OnScannedBot(ScannedBotEvent e)
    {
        Fire(1);
    }

    public override void OnHitWall(HitWallEvent e)
    {
        TurnRight(180);
    }

    // We won the round -> do a victory dance!
    public override void OnWonRound(WonRoundEvent e)
    {
        // Victory dance turning right 360 degrees 100 times
        TurnLeft(36_000);
    }
}