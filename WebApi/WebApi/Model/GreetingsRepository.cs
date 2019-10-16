using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Model {
    public class GreetingsRepository {
        private readonly SortedDictionary<long, Greeting> Greetings = new SortedDictionary<long, Greeting>();
        private long NextId;

        public IEnumerable<Greeting> All
        {
            get
            {
                lock (Greetings) {
                    return new List<Greeting>(Greetings.Values);
                }
            }
        }

        public GreetingsRepository() {
            Add(new Greeting { Text = "HelloWorld" });
        }

        public IQueryable<Greeting> AsQuerable() {
            return All.AsQueryable();
        }

        public Greeting Get(long id) {
            lock (Greetings) {
                Greeting greeting;
                return Greetings.TryGetValue(id, out greeting) ? greeting : null;
            }
        }

        public void Add(Greeting greeting) {
            lock (Greetings) {
                greeting.Id = ++NextId;
                greeting.Version = 1;
                Greetings.Add(greeting.Id, greeting);
            }
        }

        public bool TryUpdate(Greeting greeting) {
            lock (Greetings) {
                Greeting old;
                if (!Greetings.TryGetValue(greeting.Id, out old)) {
                    return false;
                }
                if (old.Version != greeting.Version) {
                    return false;
                }
                greeting.Version += 1;
                Greetings[greeting.Id] = greeting;
                return true;
            }
        }

        public bool TryRemove(Greeting greeting) {
            lock (Greetings) {
                Greeting old;
                if (!Greetings.TryGetValue(greeting.Id, out old)) {
                    return false;
                }
                if (old.Version != greeting.Version) {
                    return false;
                }
                Greetings.Remove(greeting.Id);
                return true;
            }
        }
    }
}
