using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TYG_Medical_System_Core.Models;

namespace TYG_Medical_System_Core.DAL
{
    public class DBInitializer
    {
            public static void Initialize(DBContext context)
            {
                context.Database.EnsureCreated();

                // Look for any students.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }

                var users = new User[]
                {
            new User{Email="jonathan@torres.com",Password="123123",Name="Jonathan", LastName="Torres", SecondLastName="Escarcega", Telephone=4111111},
                };
                foreach (User u in users)
                {
                    context.User.Add(u);
                }
                context.SaveChanges();

            }
        }

    
}
