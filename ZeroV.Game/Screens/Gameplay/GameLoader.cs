using System;

using osu.Framework.Allocation;
using osu.Framework.Screens;

namespace ZeroV.Game.Screens.Gameplay;

[Cached]
public partial class GameLoader : Screen {
    private readonly Func<Screen> createTargetScreen;
    public Screen? CurrentScreen { get; private set; }

    public Boolean ExitRequested { get; set; }

    public GameLoader(Func<Screen> createScreen) {
        this.createTargetScreen = createScreen;
    }

    public override void OnEntering(ScreenTransitionEvent e) {
        base.OnEntering(e);
        this.contentIn();
        // TODO: We temporarily wait it loaded and push it here simply.
        // Scheduler.Add(new ScheduledDelegate(pushWhenLoaded, Clock.CurrentTime + PlayerPushDelay, 0));
        //if (this.CurrentScreen is not null) {
        //    this.Push(this.CurrentScreen);
        //} else {
        //    throw new InvalidOperationException("CurrentScreen is null, this should not happen.");
        //    //this.Exit();
        //}
    }

    public override void OnResuming(ScreenTransitionEvent e) {
        base.OnResuming(e);
        //this.CurrentScreen?.Dispose();
        if (this.ExitRequested) {
            this.Exit();
        } else {
            this.contentIn();
        }
        //this.contentIn();
        //throw new NotImplementedException();
        //if (this.CurrentScreen is not null) {
        //    this.Push(this.CurrentScreen);
        //} else {
        //    throw new InvalidOperationException("CurrentScreen is null, this should not happen.");
        //    //this.Exit();
        //}
    }

    private void contentIn() {
        //
        // content.ScaleTo(1, 650, Easing.OutQuint).Then().Schedule(prepareNewPlayer);
        // prepareNewPlayer here
        //this.CurrentScreen = this.createTargetScreen();
        //this.Schedule(() => {
            this.CurrentScreen = this.createTargetScreen();
            this.LoadComponentAsync(this.CurrentScreen, _ => {
                this.OnPlayerLoaded();
            });
        //});

        // TODO: We temporarily wait it loaded and push it here simply.
        // Scheduler.Add(new ScheduledDelegate(pushWhenLoaded, Clock.CurrentTime + PlayerPushDelay, 0));
        if (this.CurrentScreen is not null) {
            this.Push(this.CurrentScreen);
        } else {
            throw new InvalidOperationException("CurrentScreen is null, this should not happen.");
            //this.Exit();
        }
    }

    protected virtual void OnPlayerLoaded() {
    }

    protected override void Dispose(Boolean isDisposing) {
        base.Dispose(isDisposing);
        if (isDisposing) {
            // if the player never got pushed, we should explicitly dispose it.
            this.CurrentScreen?.Dispose();
        }
    }
}
