﻿
using static HipAndClavicle.ViewModels.ShoppingCartViewModel;

namespace HipAndClavicle.Controllers;

public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepo _shoppingCartRepo;
    private readonly ICustRepo _custRepo;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly string _shoppingCartCookieName = "Cart";

    public ShoppingCartController(IServiceProvider services, IHttpContextAccessor accessor)
    {
        _shoppingCartRepo = services.GetRequiredService<IShoppingCartRepo>();
        _custRepo = services.GetRequiredService<ICustRepo>();
        _contextAccessor = accessor;
        _signInManager = services.GetRequiredService<SignInManager<AppUser>>();
        _userManager = _signInManager.UserManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var httpContext = _contextAccessor.HttpContext;
        ShoppingCartViewModel viewModel;

        // Determine if the user is logged in and retrieve the shopping cart accordingly
        if (User.Identity!.IsAuthenticated)
        {
            var owner = await _userManager.FindByNameAsync(_signInManager.Context.User.Identity!.Name!);
            ShoppingCart shoppingCart;

            // TODO something's missing here, fell asleep and forgot what it was.
            shoppingCart = await _shoppingCartRepo.GetShoppingCartByOwnerId(owner!.Id);
            shoppingCart.Owner = owner;
            viewModel = new ShoppingCartViewModel
            {
                ShoppingCart = shoppingCart,
            };
        }
        else
        {
            //SimpleShoppingCart simpleShoppingCart = GetShoppingCartFromCookie();

            viewModel = new ShoppingCartViewModel
            {
                ShoppingCart = new()
            };
        }

        return View(viewModel);
    }

    // This action method adds a listing to the cart with the specified quantity
    // TODO: For testing Will be changed to add items from catalog.
    [HttpPost]
    public async Task<IActionResult> AddToCart(int listingId, int quantity = 1)
    {

        if (_signInManager.Context.User.Identity is not null)
        {
            // Handle logged-in users

            var owner = await _userManager.FindByNameAsync(_signInManager.Context.User.Identity!.Name!);

            // Get the shopping cart using the cart ID
            var shoppingCart = await _shoppingCartRepo.GetShoppingCartByOwnerId(owner!.Id);

            // Find the listing with the given listingId
            var listing = await _custRepo.GetListingByIdAsync(listingId);

            if (listing == null)
            {
                return NotFound();
            }

            // Create a new ShoppingCartItem with the shoppingCartId, listing, and quantity
            var shoppingCartItem = new ShoppingCartItem
            {
               
            };
            shoppingCart.Items.Add(shoppingCartItem);
            await _shoppingCartRepo.UpdateShoppingCartAsync(shoppingCart);
        }
        else
        {
            // Handle non-logged-in users

            var shoppingCart = GetShoppingCartFromCookie();
            var listing = await _custRepo.GetListingByIdAsync(listingId);

            // Check if the listing already exists in the shopping cart
            var simpleCartItem = shoppingCart.Items.FirstOrDefault(item => item.ListingId == listingId);
            if (simpleCartItem != null)
            {
                simpleCartItem.Qty += quantity;
            }
            else
            {
                simpleCartItem = new SimpleCartItem
                {
                    ListingId = listing.ListingId,
                    Name = listing.ListingTitle,
                    Desc = listing.ListingDescription,
                    Qty = quantity,
                    ItemPrice = listing.Price
                };
                shoppingCart.Items.Add(simpleCartItem);
            }
            // Save the updated shopping cart in the cookie
            SetShoppingCartToCookie(shoppingCart);
        }

        return RedirectToAction("Index", "ShoppingCart");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart([Bind("itemId, qty")] int itemId, int qty)
    {
        if (User.Identity.IsAuthenticated)
        {
            // Handle logged-in users

            var item = await _shoppingCartRepo.GetOrderItemByIdAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }

            item.AmountOrdered = qty;
            await _shoppingCartRepo.UpdateItemAsync(item);
        }
        else
        {
            // Handle non-logged-in users
            var simpleShoppingCart = GetShoppingCartFromCookie();
            var simpleCartItem = simpleShoppingCart.Items.FirstOrDefault(item => item.Id == itemId);
            if (simpleCartItem != null)
            {
                simpleCartItem.Qty = qty;
            }
            else
            {
                return NotFound();
            }
            SetShoppingCartToCookie(simpleShoppingCart);
        }

        return RedirectToAction("Index", "ShoppingCart");
    }

    // Removes single item from cart
    public async Task<IActionResult> RemoveFromCart(int itemId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var owner = await _userManager.FindByNameAsync(_signInManager.Context.User.Identity!.Name!);

            // Handle logged-in users
            var item = await _shoppingCartRepo.GetShoppingCartItemByIdAsync(itemId);
            if (item == null)
            {
                return NotFound();
            }
            await _shoppingCartRepo.RemoveItemAsync(item);
        }
        else
        {
            // Handle non-logged-in users
            var simpleShoppingCart = GetShoppingCartFromCookie();
            var simpleCartItem = simpleShoppingCart.Items.FirstOrDefault(item => item.Id == itemId);
            if (simpleCartItem != null)
            {
                simpleShoppingCart.Items.Remove(simpleCartItem);
            }
            else
            {
                return NotFound();
            }
            SetShoppingCartToCookie(simpleShoppingCart);
        }
        return RedirectToAction("Index", "ShoppingCart");
    }

    // Removes all items from cart
    [HttpPost]
    public async Task<IActionResult> ClearCart(string cartId)
    {
        var httpContext = _contextAccessor.HttpContext;

        if (User.Identity.IsAuthenticated)
        {
            string ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _shoppingCartRepo.ClearShoppingCartAsync(cartId, ownerId);
        }
        else
        {
            ClearShoppingCartCookie();
        }
        return RedirectToAction("Index", "ShoppingCart");
    }

    // Helper method to get the shopping cart from the cookie
    private SimpleShoppingCart GetShoppingCartFromCookie()
    {
        var cartCookie = _contextAccessor.HttpContext.Request.Cookies[_shoppingCartCookieName];
        if (cartCookie == null)
        {
            // If the cart cookie doesn't exist, create an empty SimpleShoppingCart
            return new SimpleShoppingCart { Items = new List<SimpleCartItem>() };
        }
        else
        {
            // Deserialize the SimpleShoppingCart from the cart cookie
            return JsonConvert.DeserializeObject<SimpleShoppingCart>(cartCookie);
        }
    }

    // Helper method to save the shopping cart in the cookie
    private void SetShoppingCartToCookie(SimpleShoppingCart shoppingCart)
    {
        // Serialize the shopping cart and save it in the cookie
        var cartJson = JsonConvert.SerializeObject(shoppingCart);
        _contextAccessor.HttpContext.Response.Cookies.Append(_shoppingCartCookieName, cartJson, new CookieOptions()); // Cookie will expire once browser is closed
    }

    // Helper method to empty the cart
    private void ClearShoppingCartCookie()
    {
        var emptyCart = new SimpleShoppingCart { Items = new List<SimpleCartItem>() };
        var json = STJ.JsonSerializer.Serialize(emptyCart);
        _contextAccessor.HttpContext.Response.Cookies.Append(_shoppingCartCookieName, json, new CookieOptions()); // Cookie will expire once browser is closed
    }
}

