using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiredBrain.Models;

namespace WiredBrain.Helpers
{
    public class OrderChecker
    {
        private readonly Random random;
        private int index;

        private readonly string[] Status =
            {"Grinding beans", "Steaming milk", "Taking a sip (quality control)", "On transit to counter", "Picked up"};

        public OrderChecker(Random random)
        {
            this.random = random;
        }

        public CheckResult GetUpdate(Guid orderNo)
        {
            if (random.Next(1, 5) == 4)
            {
                if (Status.Length - 1 >= index)
                {
                    var result = new CheckResult
                    {
                        New = true,
                        Update = Status[index],
                        Finished = Status.Length - 1 == index
                    };
                    index++;
                    if (result.Finished)
                        index = 0;
                    return result;
                }
            }

            return new CheckResult { New = false };
        }
    }
}
