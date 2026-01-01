using Core.Entities;
using Core.Utilities.Security.Hashing;
using DataAccess; // AppDbContext'in olduğu namespace
using Model.Entities; // User entity'sinin olduğu namespace (Senin projende Core'da da olabilir, kontrol et)

namespace DataAccess.Seed
{
    public static class DataSeeder
    {
        public static void SeedAdminUser(AppDbContext context)
        {

            if (context.Users.Any())
            {
                return;
            }

            byte[] passwordHash, passwordSalt;
            PasswordHasher.CreatePasswordHash("Admin123!", out passwordHash, out passwordSalt);

            // 3. Admin Kullanıcısını Hazırla
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@devopswizard.com",
                Role = "Admin",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,

            };


            context.Users.Add(adminUser);
            context.SaveChanges();
            
            Console.WriteLine("--> Admin kullanıcısı başarıyla oluşturuldu! (User: admin, Pass: Admin123!)");
        }
    }
}