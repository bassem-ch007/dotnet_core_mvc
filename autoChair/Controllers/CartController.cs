using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using autoChair.Data;
using autoChair.Models;

namespace autoChair.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetSessionId()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sessionId);
            }
            return sessionId;
        }

        // GET: /Cart/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionId = GetSessionId();
            var items = await _context.CartItems
                .Where(c => c.SessionId == sessionId)
                .Include(c => c.Product)
                .ToListAsync();

            // ✅ TOUJOURS calculer (même panier vide) - SAFE contre null
            decimal total = items.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity);
            ViewBag.Total = total;
            ViewBag.ItemCount = items.Count;

            if (!items.Any())
                return View(items);

            // ✅ Vérifier le stock DANS LE PANIER
            var stockErrors = new List<string>();
            foreach (var item in items)
            {
                if (item.Product == null || item.Product.Stock < item.Quantity)
                {
                    stockErrors.Add($"{item.Product?.Name ?? "Produit"} - Stock disponible: {item.Product?.Stock ?? 0}, Demandé: {item.Quantity}");
                }
            }

            // Passer les erreurs à la view
            ViewBag.HasStockError = stockErrors.Count > 0;
            ViewBag.StockErrors = stockErrors;

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var sessionId = GetSessionId();
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                return NotFound();

            // ✅ Vérifier stock avant ajout
            if (quantity > product.Stock)
            {
                TempData["Error"] = $"Stock insuffisant pour {product.Name}. Disponible: {product.Stock}";
                return RedirectToAction("Index");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    SessionId = sessionId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                var newTotalQuantity = cartItem.Quantity + quantity;
                if (newTotalQuantity > product.Stock)
                {
                    TempData["Error"] = $"Stock insuffisant pour {product.Name}. Disponible: {product.Stock}";
                    return RedirectToAction("Index");
                }
                cartItem.Quantity = newTotalQuantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"{product.Name} ajouté au panier !";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var sessionId = GetSessionId();
            var item = await _context.CartItems
                .Include(i => i.Product)
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.SessionId == sessionId);

            if (item == null)
                return NotFound();

            if (quantity <= 0)
            {
                _context.CartItems.Remove(item);
            }
            else
            {
                //  Vérifier stock
                if (item.Product != null && quantity > item.Product.Stock)
                {
                    TempData["Error"] = $"Stock insuffisant pour {item.Product.Name}. Disponible: {item.Product.Stock}";
                    return RedirectToAction("Index");
                }
                item.Quantity = quantity;
                _context.CartItems.Update(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var sessionId = GetSessionId();
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.SessionId == sessionId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Article supprimé du panier.";
            }

            return RedirectToAction("Index");
        }
    }
}
