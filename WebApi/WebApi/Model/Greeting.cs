using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Model {
    public class Greeting {
        [Key]
        public long Id { get; set; }

        public string Text { get; set; }

        [ConcurrencyCheck]
        public int Version { get; set; }
    }
}
