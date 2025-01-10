using System;
using System.Collections.Generic;
using System.Linq;
using LenaSolutions.Classes;

public class EventScheduler
{
    private List<Event> _events;
    private List<DurationBetweenLocations> _locationTransitions;

    public EventScheduler(List<Event> events, List<DurationBetweenLocations> locationTransitions)
    {
        _events = events;
        _locationTransitions = locationTransitions;
    }

    public (List<int> EventIds, int TotalPriority) GetMaxPrioritySchedule()
    {
        int highestPriority = 0;
        List<int> optimalEventIds = new List<int>();

        for (int i = 1; i <= _events.Count; i++)
        {
            // Bütün kombinasyonları aldıktan sonra teker teker en fazla max prio veren kombinasyonu buluyorum
            var eventCombinations = GetEventCombinations(_events, i); 
            foreach (var combination in eventCombinations)
            {
                combination.Sort((event1, event2) => event1.StartTime.CompareTo(event2.StartTime));

                bool isValidSchedule = true;
                int currentPriority = 0;

                for (int index = 0; index < combination.Count - 1; index++)
                {
                    // Kombinasyon icindeki eventlerin valid olup olmadiklarina bakiyorum
                    var currentEvent = combination[index];
                    var nextEvent = combination[index + 1];
                    int transitionTime = _locationTransitions
                        .FirstOrDefault(transition => transition.From == currentEvent.Location && transition.To == nextEvent.Location)?.DurationMinutes ?? 0;

                    if (currentEvent.EndTime.Add(TimeSpan.FromMinutes(transitionTime)) > nextEvent.StartTime)
                    {
                        isValidSchedule = false;
                        break;
                    }
                }

                if (isValidSchedule) // Eger valid bir schedule ise
                {
                    // Buradan da diger priolarla karsilastirip highest olani deklare ediyorum
                    currentPriority = combination.Sum(eventItem => eventItem.Priority);
                    if (currentPriority > highestPriority)
                    {
                        highestPriority = currentPriority;
                        optimalEventIds = combination.Select(eventItem => eventItem.Id).ToList();
                    }
                }
            }
        }

        return (optimalEventIds, highestPriority);
    }

    private List<List<Event>> GetEventCombinations(List<Event> events, int combinationLength)
    {

        // Bütün kombinasyonların döndüğü bir fonksiyon
        List<List<Event>> allCombinations = new List<List<Event>>();
        void GenerateCombinations(int startIndex, List<Event> currentCombination)
        {
            if (currentCombination.Count == combinationLength)
            {
                allCombinations.Add(new List<Event>(currentCombination));
                return;
            }

            for (int index = startIndex; index < events.Count; index++)
            {
                currentCombination.Add(events[index]);
                GenerateCombinations(index + 1, currentCombination);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }

        GenerateCombinations(0, new List<Event>());
        return allCombinations;
    }
}

class Program
{
    static void Main()
    {
        var events = Event.InitializeEvents();
        var locationTransitions = DurationBetweenLocations.InitializeDurations();

        var scheduler = new EventScheduler(events, locationTransitions);
        var result = scheduler.GetMaxPrioritySchedule();

        Console.WriteLine($"Max Events: {result.EventIds.Count}");
        Console.WriteLine($"Event IDs: {string.Join(", ", result.EventIds)}");
        Console.WriteLine($"Total Priority: {result.TotalPriority}");
    }
}