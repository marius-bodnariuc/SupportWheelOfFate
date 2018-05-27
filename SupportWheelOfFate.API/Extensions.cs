using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API
{
    public static class Extensions
    {
        public static ScheduleDTO ToDTO(this Schedule schedule)
        {
            return new ScheduleDTO
            {
                Employee = schedule.Employee,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime
            };
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> collection)
        {
            var randomStartIndex = new Random()
                .Next(1, collection.Count() - 1);

            var shuffledCollection = collection
                .Skip(randomStartIndex)
                .Take(collection.Count() - randomStartIndex)
                .Concat(collection.Take(randomStartIndex));

            return shuffledCollection;
        }

        public static IEnumerable<(T, T)> ToRandomPairs<T>(this IEnumerable<T> collection)
        {
            // NOTE if the collection has an odd number of elements, the last one will be left out
            var pairs = collection.FirstHalf().Randomize()
                .Zip(collection.SecondHalf().Randomize(),
                    (firstPerson, secondPerson) => (firstPerson, secondPerson));

            return pairs;
        }

        public static void Times(this int num, Action action)
        {
            for (var i=0; i<num; i++)
            {
                action.Invoke();
            }
        }

        private static IEnumerable<T> SecondHalf<T>(this IEnumerable<T> collection)
        {
            return collection.Skip(collection.Count() / 2).Take(collection.Count() / 2);
        }

        private static IEnumerable<T> FirstHalf<T>(this IEnumerable<T> collection)
        {
            return collection.Take(collection.Count() / 2);
        }
    }
}
