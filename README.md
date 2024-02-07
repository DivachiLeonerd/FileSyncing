This is a one-way file Syncronization tool where it updates 'Target' from 'Source'

WARNING: ANY KIND OF FILES MOVED TO 'TARGET' CAN BE SUBJECT TO PERMANENT DELETION!
    Also, be careful about the path of each folder. My app is very dumb and will delete stuff if it has permission to do so.

- Copy the repo and open it with Visual Studio
- Go into

        Project -> Project Settings -> Debug
  and click the first hyperlink to where you can input the command-line arguments.
- Input arguments in the following fashion: [SOURCE] [PATH] [SYNCINTERVAL] [LOG]
- 'Source' is the source folder. The "main" folder you want to be copied over to 'Target'.
- 'Target' is the folder which will suffer the updates.
- 'SyncInterval' is the time (in ticks) it takes to relaunch the update procedure. Feel free to play with/adjust this.
- 'Log' is the path of the Log. Default location is in the same folder as executable.
