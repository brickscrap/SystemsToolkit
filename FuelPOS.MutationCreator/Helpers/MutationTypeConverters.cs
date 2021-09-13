using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelPOS.MutationCreator.Helpers
{
    public static class MutationTypeConverters
    {
        public static string ToMutation(this bool input)
        {
            return (input ? "YES" : "NO");
        }
    }
}
