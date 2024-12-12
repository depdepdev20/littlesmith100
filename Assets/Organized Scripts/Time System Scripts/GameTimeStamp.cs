using System;

public class GameTimeStamp
{
    public enum Season { Spring, Summer, Fall, Winter }

    public int day; // Current day number
    public Season season; // Current season
    public int hour; // Current hour
    public int minute; // Current minute
    public int second; // Current second (optional if needed)

    public GameTimeStamp(int day, Season season, int hour, int minute, int second = 0)
    {
        this.day = day;
        this.season = season;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }

    // Update the clock by incrementing time and managing overflows
    public void UpdateClock()
    {
        second++;
        if (second >= 60)
        {
            second = 0;
            minute++;
        }

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
    }

    // Check if a new day has started
    public bool IsNewDay()
    {
        return hour == 0 && minute == 0 && second == 0;
    }

    // Get the day of the week as a string
    public string GetDayOfTheWeek()
    {
        string[] daysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        return daysOfWeek[day % 7]; // Modulo to cycle through days of the week
    }

    // Get a formatted time string (e.g., "AM 07:45:00")
    // Get a formatted time string (e.g., "07:45 PM")
    public string GetFormattedTime()
    {
        int displayHour = hour;
        string period = "AM";

        if (hour >= 12)
        {
            period = "PM";
            if (hour > 12)
            {
                displayHour -= 12;
            }
        }
        else if (hour == 0)
        {
            displayHour = 12; // Display 12 for midnight
        }

        return $"{displayHour:D2}:{minute:D2} {period}";
    }


    // Get a formatted date string (e.g., "Spring 12 (Mon)")
    public string GetFormattedDate()
    {
        string dayOfTheWeek = GetDayOfTheWeek().Substring(0, 3); // First 3 letters (e.g., "Mon")
        return $"{season} {day} ({dayOfTheWeek})";
    }

    // Advance to the next season if needed
    public void AdvanceSeason()
    {
        if (day > 30) // Assuming each season has 30 days
        {
            day = 1;
            season = (Season)(((int)season + 1) % Enum.GetValues(typeof(Season)).Length);
        }
    }
}
