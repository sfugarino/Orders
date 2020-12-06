using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
    /// <summary>
    /// Cooking stations
    /// </summary>
    /// <see cref="https://kamikoto.com/blogs/fundamentals/cooking-stations-explained-understanding-a-gourmet-kitchen"/>
    public enum Station
    {
        Saucier, // sauce chef
        Poissonnier, // fish seafood
        Rotisseur, // meat chef
        Entremetier, // vegetable ched
        Pâtissier // pastry chef
    }
}
