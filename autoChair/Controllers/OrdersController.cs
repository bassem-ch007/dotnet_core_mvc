using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using autoChair.Data;
using autoChair.Models;

namespace autoChair.Controllers
{
    [Authorize]  // ✅ TOUT LE CONTROLLER NÉCESSITE LOGIN
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetSessionId()
        {
            return HttpContext.Session.GetString("CartSessionId") ?? string.Empty;
        }

        // GET: /Orders/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var sessionId = GetSessionId();
            var items = await _context.CartItems
                .Where(c => c.SessionId == sessionId)
                .Include(c => c.Product)
                .ToListAsync();

            if (!items.Any())
                return RedirectToAction("Index", "Cart");

            // ✅ CONTRÔLE 2 : Vérification de sécurité avant d'afficher le formulaire
            foreach (var item in items)
            {
                if (item.Product == null || item.Product.Stock < item.Quantity)
                {
                    TempData["Error"] = $"Stock insuffisant pour {item.Product?.Name}. Retour au panier.";
                    return RedirectToAction("Index", "Cart");
                }
            }

            decimal total = items.Sum(i => i.Product!.Price * i.Quantity);
            ViewBag.Total = total;

            return View();
        }

        // POST: /Orders/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(
            string shippingName,
            string shippingAddress,
            string shippingCity,
            string shippingPostalCode,
            string shippingCountry)
        {
            var sessionId = GetSessionId();
            var cartItems = await _context.CartItems
                .Where(c => c.SessionId == sessionId)
                .Include(c => c.Product)
                .ToListAsync();

            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // ✅ CONTRÔLE 3 : Vérifier le stock UNE DERNIÈRE FOIS avant de créer la commande
            var stockErrors = new List<string>();
            foreach (var item in cartItems)
            {
                if (item.Product == null || item.Product.Stock < item.Quantity)
                {
                    stockErrors.Add($"{item.Product?.Name} - Stock: {item.Product?.Stock}, Demandé: {item.Quantity}");
                }
            }

            // ⚠️ SI ERREUR : Rediriger vers le panier avec message
            if (stockErrors.Count > 0)
            {
                TempData["Error"] = "Stock insuffisant pour : " + string.Join(", ", stockErrors);
                return RedirectToAction("Index", "Cart");
            }

            // ✅ CRÉER LA COMMANDE
            decimal total = cartItems.Sum(i => i.Product!.Price * i.Quantity);
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = total,
                ShippingName = shippingName,
                ShippingAddress = shippingAddress,
                ShippingCity = shippingCity,
                ShippingPostalCode = shippingPostalCode,
                ShippingCountry = shippingCountry,
                OrderNumber = $"CMD-{DateTime.UtcNow.Ticks}",
                Status = "Confirmée"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // ✅ CRÉER LES ITEMS ET DÉCRÉMENTER LE STOCK
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product!.Price
                };
                _context.OrderItems.Add(orderItem);

                // 🔻 DÉCRÉMENTER LE STOCK
                item.Product.Stock -= item.Quantity;
            }

            // ✅ VIDER LE PANIER
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Commande confirmée avec succès !";
            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        // GET: /Orders/Confirmation/1
        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)!
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: /Orders/MyOrders (voir ses commandes)
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .Include(o => o.OrderItems)!
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}
