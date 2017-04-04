using DEA.Resources;
using System;
using System.Globalization;

public static class Config
{
    public static readonly CultureInfo CI = CultureInfo.CreateSpecificCulture("en-CA");

    public static readonly TimeSpan DEFAULT_MUTE_TIME, WHORE_COOLDOWN = TimeSpan.FromHours(2), JUMP_COOLDOWN = TimeSpan.FromHours(4),
        STEAL_COOLDOWN = TimeSpan.FromHours(6), ROB_COOLDOWN = TimeSpan.FromHours(8), WITHDRAW_COOLDOWN = TimeSpan.FromHours(4),
        LINE_COOLDOWN = TimeSpan.FromSeconds(30);

    public static readonly int MIN_CHAR_LENGTH = 7, LEADERBOARD_CAP = 20, RATELB_CAP = 20, WHORE_ODDS = 90, JUMP_ODDS = 85, STEAL_ODDS = 80, 
        MIN_CHILL = 5, MAX_CHILL = (int)TimeSpan.FromHours(1).TotalSeconds, MIN_CLEAR = 2, MAX_CLEAR = 1000, GANG_NAME_CHAR_LIMIT = 24,
        GANGSLB_CAP = 20, MIN_ROB_ODDS = 50, MAX_ROB_ODDS = 75, DEA_CUT = 10;

    public static readonly double LINE_COST = 250.0, POUND_COST = 1000.0, KILO_COST = 2500.0, POUND_MULTIPLIER = 2.0, KILO_MULTIPLIER = 4.0, 
        RESET_REWARD = 10000.0, MAX_WHORE = 100.0, MIN_WHORE = 50.0, WHORE_FINE = 200.0, MAX_JUMP = 250.0, JUMP_FINE = 500.0, MIN_JUMP = 100.0, 
        MAX_STEAL = 500.0, MIN_STEAL = 250.0, STEAL_FINE = 1000.0, MAX_RESOURCES = 1000.0, MIN_RESOURCES = 25.0, TEMP_MULTIPLIER_RATE = 0.1, 
        DONATE_MIN = 5.0, BET_MIN = 5.0, GANG_CREATION_COST = 2500.0, GANG_NAME_CHANGE_COST = 500.0, WITHDRAW_CAP = 0.20, MIN_WITHDRAW = 50.0, 
        MIN_DEPOSIT = 25.0;

    public static readonly string[] BANKS = { "Bank of America", "Wells Fargo Bank", "JPMorgan Chase Bank", "Capital One Bank",
        "RBC Bank", "USAA Bank", "Union Bank", "Morgan Stanley Bank" }, STORES = { "7-Eleven", "Speedway", "Couche-Tard", "QuikTrip",
        "Kroger", "Circle K" };
}
