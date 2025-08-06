
using Microsoft.EntityFrameworkCore;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{
    public class BagService : IBagService
    {
        private readonly MyDbContext _dbContext;
        private readonly TiktokConfigService tiktokConfigService;
        public BagService(MyDbContext dbContext, TiktokConfigService tiktokConfigService)
        {
            _dbContext = dbContext;
            this.tiktokConfigService = tiktokConfigService ?? throw new ArgumentNullException(nameof(tiktokConfigService));
        }

        public Task<List<BagSlot>> GetAllBagSlotsAsync(int playerId)
        {
            // Fetch all bags for the player from the database
            return _dbContext.BagSlots
                .Where(b => b.PlayerId == playerId)
                .Include(b => b.BagItem) // Include related Item entity
                .ToListAsync();
        }

        public Task<BagSlot> AddBagSlotAsync(int playerId)
        {
            // Create a new bag for the player and save it to the database
            var newBag = new BagSlot
            {
                PlayerId = playerId,
            };
            _dbContext.BagSlots.Add(newBag);
            return _dbContext.SaveChangesAsync().ContinueWith(_ => newBag);

        }


        public Task<List<BagSlot>> AddBagSlotsAsync(int playerId, int count)
        {
            // Create multiple new bags for the player and save them to the database
            var newBags = new List<BagSlot>();
            for (int i = 0; i < count; i++)
            {
                var newBag = new BagSlot
                {
                    PlayerId = playerId,
                };
                newBags.Add(newBag);
            }
            _dbContext.BagSlots.AddRange(newBags);
            return _dbContext.SaveChangesAsync().ContinueWith(_ => newBags);

        }

        public Task<BagSlot?> GetEmptyBagSlotAsync(int playerId)
        {
            // Fetch an empty bag slot for the player from the database
            return _dbContext.BagSlots
                .Where(b => b.PlayerId == playerId && b.ItemId == null)
                .FirstOrDefaultAsync();

        }

        public Task<List<BagItem>> GetBagItemsAsync(int playerId, string itemBusinessId)
        {
            // Fetch all bag items for the player with the specified item business ID
            return _dbContext.BagItems
                .Where(bi => bi.PlayerId == playerId && bi.ItemBusinessId == itemBusinessId)
                .ToListAsync();
        }

        // Implementation of the IBagService methods would go here
        public async Task<BagItem> AddItemToBagSlotAsync(int playerId, string itemBusinessId, int count)
        {
            //如果找到相同itemBusinessId的道具，且没有满，则该道具数量+count，如果已经满了，则查看空的格子，如果存在则放入空的BagSlot
            var items = await GetBagItemsAsync(playerId, itemBusinessId);
            if(items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item.Count + count < tiktokConfigService.GetItemMaxCount(item.ItemBusinessId))
                    {
                        item.Count += count;
                        //更新道具
                        _dbContext.BagItems.Update(item);
                        await _dbContext.SaveChangesAsync();
                        return item;
                    }
                }
            }

            //如果没有找到相同itemBusinessId的道具，或者所有的道具都满了，则查看空的BagSlot
            var emptyBagSlot = await GetEmptyBagSlotAsync(playerId);
            if (emptyBagSlot != null)
            {
                //如果存在空的BagSlot，则创建新的BagItem并放入空的BagSlot
                var newItem = new BagItem
                {
                    PlayerId = playerId,
                    ItemBusinessId = itemBusinessId,
                    Count = count,
                    BagSlotId = emptyBagSlot.Id
                };
                //将新道具添加到数据库
                _dbContext.BagItems.Add(newItem);
                emptyBagSlot.ItemId = newItem.Id; // 更新BagSlot的ItemId
                _dbContext.BagSlots.Update(emptyBagSlot); // 更新BagSlot
                await _dbContext.SaveChangesAsync();
                return newItem;
            }

            //如果没有空的BagSlot，则返回背包已满的错误
            throw new InvalidOperationException("Bag is full, cannot add new item.");
        }



        public Task<BagSlot> RemoveItemFromBagSlotAsync(int playerId, int itemId, int count)
        {
            throw new NotImplementedException();
        }


    }
}
