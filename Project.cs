using (var game = new BlobGame.Main())
    game.Run();
    
//git status (optional, outputs all changed files)
//git add . (to stage all files)
//git add <file1> <file2> (to stage specific files)
//git commit -m "Your commit message here" (to commit)
//git push origin main (to push, with main being the branch name)

//dotnet publish -r win-x64 (for windows)
//dotnet publish -r linux-x64 (for linux)

/*

? Hey,
? So this is probably where I'm going to be leaving notes and questioning reality.
? Man coding is kind of hard, thank God I didn't choose C++, that would have been too difficult.
? Any things I have to finish will be marked with TODO, else with ? (or if really important with !)
? I have no idea when I first started this, github says 2 months ago (writing on 28/11/2024) but that was
? when I first committed to github, what about those tests that I made?
? This stuff will probably go on some essay one day soooo...
? Hi, I'm MrBeelo! (Vaggelis) and uhhhhhhhh...
? That's about it... Okay look,
? I have way too much shit going on at the moment, I hope future me is okay. :(
? This is my first *real* project (if we exclude anything made in scratch)
? Currently in 9th grade doing this as a passion project.
? I hope one day I can make this into a full game and maybe even sell it.
? Currently I want to make it to 50 updates and come out with a finished product.
? And I'm not saying polished, unless I join a game studio or something.
? This idea wouldn't have been a thing if my friend Nick didn't make this
? horrific model of a 'blob' or whatever. Whatever it is, it was funny at the time.
? Genuinely, how did I even get here??? Like I'm making a fucking game???
? This is something I've always wanted to do, and now I'm here!
? And I've learned so much along the way too! I even learned how shit some
? programming languages are! (I'm looking at you, Java)
? As much as I hate them though, they're still languages and you can still make
? some insane shit with them.. I also hope that I finally learn python, I beleive
? it would be a nice addition to my language collection.
? And that's about it for my rant. I hope I reach 0.50 soon and the
? people like it :)))))
?
? Peace, Beelo


!----------AFTER 0.40-----------
! For update 0.50 (hoping to be final), I'm just gonna add whatever comes to me mind.
! I want 10 new features and an update for each of them.
! Good Luck Ya 'hmar

TODOS THAT I HAVE TO COMPLETE :))) 

TODO: FOR UNTIL 0.50:
TODO:
TODO: 1) DELTATIME* []
TODO: 2) PROPER BOSSFIGHT*** []
TODO: 3) NEW COOL FONT + MAKE SELECTED INDEX BIGGER [0.41]
TODO: 4) MENU SCREEN BACKGROUND [IMPOSSIBLE ON LINUX, FIND ANOTHER WAY]
TODO: 5) MOUSE SUPPORT []
TODO: 6) COVER ART [0.42]
TODO: 7) LEVEL SELECT SCREEN []
TODO: 8) 10 MORE LEVELS + BIOMES** []
TODO: 9) MUSIC []
TODO: 10) LOOOOOOOOOOOOORE FOR NICK []

! * Definition: float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
! USE LogicalPosition TO MOVE!! (Just Position but it sounds cooler)
! Afterwards add LogPos into Drect and update as usual. Prob make that updating thing into a method and add thing in first line,
! where in Sprite.cs you call Drect = new Rectangle(LogicalPosition.X, LogicalPosition.Y, Drect.Width, Drect.Height)
! Then you can call whenever, or just set up an update loop withing Sprite.cs that other classes inherit from.
! All values should be floats up until the update method, as if you multiply anything by deltaTime and it's
! rounded to an integer, It's going to be 0, so the player won't be able to move
!
! ** Levels 0-10 are in the Plains biome, make levels 11-20 be in like the Crypts or something. Also add [THE PLAINS/CRYPTS]
! popup when entering the biome.
!
! *** Should be a new class and should summon small circles and triangles. Also make a better map for it.

*/

