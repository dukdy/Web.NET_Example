using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ECommerceMVC.Helpers;

namespace ECommerceMVC.Controllers
{
	public class CartController : Controller
	{
        private readonly Hshop2023Context db;

        public CartController(Hshop2023Context context) {
			db = context;
		}
		
		public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
		public IActionResult Index()
		{
			return View(Cart);

		}public IActionResult AddToCart(int id, int quantity = 1)
		{
			var giohang = Cart;
			var item = giohang.SingleOrDefault(p => p.MaHh == id);
			if (item == null)
			{
				var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
				if(hangHoa == null)
				{
					TempData["Message"] = $"Không tìm thấy hàng hoá có mã {id}";
					return Redirect("/404");
				}
				item = new CartItem
				{
					MaHh = hangHoa.MaHh,
					TenHh = hangHoa.TenHh,
					DonGia = hangHoa.DonGia ?? 0,
					Hinh = hangHoa.Hinh ?? string.Empty,
					SoLuong = quantity
				};
				giohang.Add(item);
			}
			else
			{
				item.SoLuong += quantity;
			}

			HttpContext.Session.Set(MySetting.CART_KEY, giohang);
			return RedirectToAction("Index");
		}

		public IActionResult RemoveCart(int id)
		{
			var giohang = Cart;
            var item = giohang.SingleOrDefault(p => p.MaHh == id);
			if (item != null)
			{
				giohang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, giohang);
            }
            return RedirectToAction("Index");
        }
	}
}
