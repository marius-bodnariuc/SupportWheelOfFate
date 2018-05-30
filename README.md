# SupportWheelOfFate
Random work schedule generator for 10 employees and 2 daily shifts, weekends excluded

## Setup
* Clone the repository
* `cd` into `SupportWheelOfFate.API` and restore the database:

`dotnet ef database update`

Running the above command should produce an output similar to the one below:

```
Applying migration '20180527152328_Initial'.
Done.
```

## Test the app in Debug mode

* Open the solution in VisualStudio 2017
* Launch the `SupportWheelOfFate.API` project
* Wait for the schedules to be generated **
* Launch the `SupportWheelOfFate.Web` project ***

** This is done from background jobs that get invoked shortly after the API project has been launched

To get feedback on the background schedule generation process, select `Show output from: ASP.NET Core Web Server` in the Output window in VS, once the project has been launched in Debug mode.

During the first run, the output should be similar to the one below:

```
SupportWheelOfFate.API> GetSchedulesBetween 5/1/2018 12:00:00 AM and 5/2/2018 12:00:00 AM: 0 found
SupportWheelOfFate.API> New schedule added to DB. New total: 1
SupportWheelOfFate.API> New schedule added to DB. New total: 2
[...]
SupportWheelOfFate.API> New schedule added to DB. New total: 43
SupportWheelOfFate.API> New schedule added to DB. New total: 44
SupportWheelOfFate.API> New schedule added to DB. New total: 45
SupportWheelOfFate.API> New schedule added to DB. New total: 46
SupportWheelOfFate.API> Generated schedules for current month
```

```
SupportWheelOfFate.API> GetSchedulesBetween 6/1/2018 12:00:00 AM and 6/2/2018 12:00:00 AM: 0 found
SupportWheelOfFate.API> New schedule added to DB. New total: 47
SupportWheelOfFate.API> New schedule added to DB. New total: 48
[...]

SupportWheelOfFate.API> New schedule added to DB. New total: 86
SupportWheelOfFate.API> New schedule added to DB. New total: 86
SupportWheelOfFate.API> New schedule added to DB. New total: 87
SupportWheelOfFate.API> New schedule added to DB. New total: 88
SupportWheelOfFate.API> Generated schedules for next month
```

On subsequent runs, the background jobs should detect the existing schedules and skip the generation of new ones:

```
SupportWheelOfFate.API> GetSchedulesBetween 5/1/2018 12:00:00 AM and 5/2/2018 12:00:00 AM: 2 found
SupportWheelOfFate.API> Schedules for current month already in place
SupportWheelOfFate.API> GetSchedulesBetween 6/1/2018 12:00:00 AM and 6/2/2018 12:00:00 AM: 2 found
SupportWheelOfFate.API> Schedules for next month already in place
```

*** Doing so should cause a simple calendar to be loaded in the browser, displaying the schedules for the current month, e.g.:

_TODO link screenshot here_

