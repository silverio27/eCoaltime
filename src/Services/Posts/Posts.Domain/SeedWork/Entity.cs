using Newtonsoft.Json;
using System;

namespace Posts.Domain.SeedWork
{
    public abstract class Entity
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }
        public Entity() => Id = Guid.NewGuid();

        public override abstract string ToString();
    }
}