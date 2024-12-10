using Game.Models;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { Email = "admin@game.pl", HashedPassword = "$2a$11$vxt2SMhddg9FXkEmeRhFkOGg7hiHh2GZLqINSnpRSNqZnWW5KffLi", Role = "Admin" },
                    new User { Email = "user@game.pl", HashedPassword = "$2a$11$fq5YkEf.Jbc8ssUJFwrPteWPZQtDMsZdvGCPDEn4j0k7jOizxHmQG", Role = "User" }
                );
            }

            if (!_context.Items.Any())
            {
                _context.Items.AddRange(new List<Item>
                {
                    new Item { Name = "Zbroja", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 15, BonusHealth = 10, Price = 150, IsForSale = true, IsEquipped = true },
                    new Item { Name = "Miecz Żołnierza", Type = "Broń", BonusStrength = 10, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 100, IsForSale = true, IsEquipped = true },
                    new Item { Name = "Topór Wikinga", Type = "Broń", BonusStrength = 12, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 120, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Rapier", Type = "Broń", BonusStrength = 6, BonusDexterity = 8, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 95, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Kusza", Type = "Broń", BonusStrength = 10, BonusDexterity = 5, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 130, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Broń Chaosu", Type = "Broń", BonusStrength = 12, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 10, BonusHealth = 0, Price = 170, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Sierp Krwi", Type = "Broń", BonusStrength = 8, BonusDexterity = 10, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 140, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Rycerza", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 20, BonusHealth = 0, Price = 200, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Pancerz Gladiatora", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 25, BonusHealth = 0, Price = 250, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Magi", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 15, BonusDefense = 10, BonusHealth = 0, Price = 220, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Łowcy", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 18, BonusHealth = 0, Price = 210, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Pancerz Wikinga", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 30, BonusHealth = 0, Price = 300, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Skórzana Zbroja", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 5, BonusIntelligence = 0, BonusDefense = 10, BonusHealth = 0, Price = 150, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Pancerz Smoków", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 10, BonusDefense = 40, BonusHealth = 0, Price = 350, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Złodzieja", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 10, BonusIntelligence = 0, BonusDefense = 15, BonusHealth = 0, Price = 180, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Królewska", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 50, BonusHealth = 0, Price = 500, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Zbroja Lodu", Type = "Zbroja", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 20, BonusDefense = 35, BonusHealth = 0, Price = 400, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Siły", Type = "Amulet", BonusStrength = 5, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 100, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Zwinności", Type = "Amulet", BonusStrength = 0, BonusDexterity = 10, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 110, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Mądrości", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 10, BonusDefense = 0, BonusHealth = 0, Price = 120, IsForSale = true, IsEquipped = true },
                    new Item { Name = "Amulet Ochrony", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 15, BonusHealth = 0, Price = 130, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Życia", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 50, Price = 150, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Szybkości", Type = "Amulet", BonusStrength = 0, BonusDexterity = 15, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 0, Price = 140, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Pancerza", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 20, BonusHealth = 0, Price = 160, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Ognia", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 15, BonusDefense = 0, BonusHealth = 0, Price = 170, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Amulet Magii", Type = "Amulet", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 20, BonusDefense = 0, BonusHealth = 0, Price = 180, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Złote Jabłko", Type = "Item", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 100, Price = 200, IsForSale = true, IsEquipped = false },
                    new Item { Name = "Eliksir Zdrowia", Type = "Item", BonusStrength = 0, BonusDexterity = 0, BonusIntelligence = 0, BonusDefense = 0, BonusHealth = 50, Price = 150, IsForSale = true, IsEquipped = false }
                });
            }

            if (!_context.Quests.Any())
            {
                _context.Quests.AddRange(new List<Quest>
                {
                    new Quest { Name = "Polowanie", Description = "Pokonaj 3 wilki.", Difficulty = "Łatwy", RewardExperience = 100, IsCompleted = false },
                    new Quest { Name = "Obrona wioski", Description = "Pokonaj 2 bandytów.", Difficulty = "Średni", RewardExperience = 150, IsCompleted = false },
                    new Quest { Name = "Zabij potwora", Description = "Pokonaj potwora z jaskini", Difficulty = "Trudny", RewardExperience = 200, IsCompleted = false },
                    new Quest { Name = "Poszukiwanie skarbu", Description = "Znajdź starożytny miecz.", Difficulty = "Trudny", RewardExperience = 500, IsCompleted = false },
                    new Quest { Name = "Obrona karawany", Description = "Obroń karawanę przed napadem.", Difficulty = "Średni", RewardExperience = 350, IsCompleted = false },
                    new Quest { Name = "Wielka bitwa", Description = "Pokonaj 10 orków.", Difficulty = "Trudny", RewardExperience = 300, IsCompleted = false },
                    new Quest { Name = "Odkrycie ruin", Description = "Znajdź zrujnowany zamek.", Difficulty = "Średni", RewardExperience = 200, IsCompleted = false },
                    new Quest { Name = "Pościg", Description = "Złap uciekiniera w lesie.", Difficulty = "Średni", RewardExperience = 250, IsCompleted = false },
                    new Quest { Name = "Szpiegowanie", Description = "Zbadaj wrogie obozowisko.", Difficulty = "Trudny", RewardExperience = 400, IsCompleted = false },
                    new Quest { Name = "Przywrócenie równowagi", Description = "Pokonaj złego maga.", Difficulty = "Trudny", RewardExperience = 600, IsCompleted = false },
                    new Quest { Name = "Tajemnicze zlecenie", Description = "Rozwiąż zagadkę starego maga.", Difficulty = "Średni", RewardExperience = 300, IsCompleted = false },
                    new Quest { Name = "Zdrada", Description = "Pokonaj zdrajcę w armii.", Difficulty = "Trudny", RewardExperience = 500, IsCompleted = false },
                    new Quest { Name = "Wyprawa na północ", Description = "Zdobądź rzadki minerał z gór.", Difficulty = "Średni", RewardExperience = 350, IsCompleted = false },
                    new Quest { Name = "Walka o wolność", Description = "Pokonaj tyrana.", Difficulty = "Trudny", RewardExperience = 700, IsCompleted = false },
                    new Quest { Name = "Poszukiwanie artefaktu", Description = "Zdobądź magiczny artefakt z lochów.", Difficulty = "Trudny", RewardExperience = 450, IsCompleted = false },
                    new Quest { Name = "Pomoc dla wioski", Description = "Pomóż w budowie wioski.", Difficulty = "Łatwy", RewardExperience = 100, IsCompleted = false },
                    new Quest { Name = "Pożar w mieście", Description = "Zatrzymaj pożar w mieście.", Difficulty = "Średni", RewardExperience = 150, IsCompleted = false },
                    new Quest { Name = "Ochrona władcy", Description = "Obroń króla przed zamachem.", Difficulty = "Trudny", RewardExperience = 600, IsCompleted = false },
                    new Quest { Name = "Zatrucie", Description = "Zatrzymaj zamach na ludność przez truciciela.", Difficulty = "Średni", RewardExperience = 250, IsCompleted = false },
                    new Quest { Name = "Wielka ucieczka", Description = "Pomóż więźniom uciec z lochów.", Difficulty = "Średni", RewardExperience = 300, IsCompleted = false },
                    new Quest { Name = "Skok na bank", Description = "Wykonaj skok na bank w mieście.", Difficulty = "Trudny", RewardExperience = 500, IsCompleted = false },
                    new Quest { Name = "Zabójca wśród nas", Description = "Złap mordercę w mieście.", Difficulty = "Średni", RewardExperience = 250, IsCompleted = false },
                    new Quest { Name = "Obrona twierdzy", Description = "Obroń twierdzę przed atakiem.", Difficulty = "Trudny", RewardExperience = 350, IsCompleted = false },
                    new Quest { Name = "Zatrzymanie przemytników", Description = "Pokonaj przemytników na granicy.", Difficulty = "Średni", RewardExperience = 400, IsCompleted = false },
                    new Quest { Name = "Wykopaliska", Description = "Znajdź starożytne ruiny w lesie.", Difficulty = "Średni", RewardExperience = 200, IsCompleted = false },
                    new Quest { Name = "Zło w podziemiach", Description = "Pokonaj potwory w lochach.", Difficulty = "Trudny", RewardExperience = 450, IsCompleted = false },
                    new Quest { Name = "Oczyszczenie wioski", Description = "Pokonaj demony w wiosce.", Difficulty = "Trudny", RewardExperience = 500, IsCompleted = false }
                });

                _context.SaveChanges();
            }
        }
    }
}