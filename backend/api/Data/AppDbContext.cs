using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<Resource>().HasData(
                new Resource { ResourceId = 1, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 2, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 3, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 4, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 5, ResourceType = Enums.ResourceType.Skrivbord },

                new Resource { ResourceId = 6, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 7, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 8, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 9, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 10, ResourceType = Enums.ResourceType.Skrivbord },

                new Resource { ResourceId = 11, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 12, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 13, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 14, ResourceType = Enums.ResourceType.Skrivbord },
                new Resource { ResourceId = 15, ResourceType = Enums.ResourceType.Skrivbord },

                new Resource { ResourceId = 20, ResourceType = Enums.ResourceType.Mötesrum },
                new Resource { ResourceId = 21, ResourceType = Enums.ResourceType.Mötesrum },
                new Resource { ResourceId = 22, ResourceType = Enums.ResourceType.Mötesrum },
                new Resource { ResourceId = 23, ResourceType = Enums.ResourceType.Mötesrum },

                new Resource { ResourceId = 30, ResourceType = Enums.ResourceType.AIServer },

                new Resource { ResourceId = 40, ResourceType = Enums.ResourceType.VRHeadset },
                new Resource { ResourceId = 41, ResourceType = Enums.ResourceType.VRHeadset },
                new Resource { ResourceId = 42, ResourceType = Enums.ResourceType.VRHeadset },
                new Resource { ResourceId = 43, ResourceType = Enums.ResourceType.VRHeadset }
            );
        }
    }
}
