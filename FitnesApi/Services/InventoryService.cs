using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class InventoryService
    {
        private readonly ContextDb _context;

        public InventoryService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAll()
        {
            var items = await _context.InventoryItems
                .OrderBy(x => x.name)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = items
            });
        }

        public async Task<ActionResult> Create(CreateInventoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.name))
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Введите название инвентаря"
                });
            }

            if (request.quantity < 0)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Количество не может быть отрицательным"
                });
            }

            var item = new InventoryItem
            {
                name = request.name,
                category = request.category,
                quantity = request.quantity,
                status = request.status,
                need_repair = request.need_repair,
                purchase_required = request.purchase_required,
                comment = request.comment
            };

            await _context.InventoryItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Инвентарь добавлен",
                item
            });
        }

        public async Task<ActionResult> UpdateStatus(UpdateInventoryStatusRequest request)
        {
            var item = await _context.InventoryItems
                .FirstOrDefaultAsync(x => x.id_inventory == request.id_inventory);

            if (item == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Инвентарь не найден"
                });
            }

            item.status = request.status;
            item.need_repair = request.need_repair;
            item.purchase_required = request.purchase_required;
            item.comment = request.comment;

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Состояние инвентаря обновлено",
                item
            });
        }

        public async Task<ActionResult> Delete(int id_inventory)
        {
            var item = await _context.InventoryItems
                .FirstOrDefaultAsync(x => x.id_inventory == id_inventory);

            if (item == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Инвентарь не найден"
                });
            }

            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Инвентарь удалён"
            });
        }
    }
}