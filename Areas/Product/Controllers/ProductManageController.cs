using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Areas.Product.Models;
using appmvcnet.Data;
using appmvcnet.DatabaseContext;
using appmvcnet.Models;
using appmvcnet.Models.Product;
using appmvcnet.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace appmvcnet.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/productmanage/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class ProductManageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductManageController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }
        // GET: Blog/Post
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage, int pagesize)
        {
            var posts = _context.Products
                        .Include(p => p.Author)
                        .OrderByDescending(p => p.DateUpdated);

            int totalPosts = await posts.CountAsync();
            if (pagesize <= 0) pagesize = 10;
            int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;

            ViewBag.postIndex = (currentPage - 1) * pagesize;

            var postsInPage = await posts.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .Include(p => p.ProductCategoryProducts)
                             .ThenInclude(pc => pc.Category)
                             .ToListAsync();

            return View(postsInPage);
        }

        // GET: Blog/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Blog/Post/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _context.CategoryProducts.ToListAsync();

            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            return View();
        }

        // POST: Blog/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs,Price")] CreateProductModel product)
        {
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            if (product.Slug == null)
            {
                product.Slug = AppUtilities.GenerateSlug(product.Title);
            }

            if (await _context.Products.AnyAsync(p => p.Slug == product.Slug))
            {
                ModelState.AddModelError("Slug", "Input another slug");
                return View(product);
            }



            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                product.DateCreated = product.DateUpdated = DateTime.Now;
                product.AuthorId = user.Id;
                _context.Add(product);

                if (product.CategoryIDs != null)
                {
                    foreach (var CateId in product.CategoryIDs)
                    {
                        _context.Add(new ProductCategoryProduct()
                        {
                            CategoryID = CateId,
                            Product = product
                        });
                    }
                }


                await _context.SaveChangesAsync();
                StatusMessage = "A new product has been created";
                return RedirectToAction(nameof(Index));
            }


            return View(product);
        }

        // GET: Blog/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.Posts.FindAsync(id);
            var product = await _context.Products.Include(p => p.ProductCategoryProducts).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            var postEdit = new CreateProductModel()
            {
                ProductID = product.ProductID,
                Title = product.Title,
                Content = product.Content,
                Description = product.Description,
                Slug = product.Slug,
                Published = product.Published,
                CategoryIDs = product.ProductCategoryProducts.Select(pc => pc.CategoryID).ToArray(),
                Price = product.Price
            };

            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            return View(postEdit);
        }

        // POST: Blog/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,Title,Description,Slug,Content,Published,CategoryIDs,Price")] CreateProductModel product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");


            if (product.Slug == null)
            {
                product.Slug = AppUtilities.GenerateSlug(product.Title);
            }

            if (await _context.Products.AnyAsync(p => p.Slug == product.Slug && p.ProductID != id))
            {
                ModelState.AddModelError("Slug", "Input another slug");
                return View(product);
            }


            if (ModelState.IsValid)
            {
                try
                {

                    var productUpdate = await _context.Products.Include(p => p.ProductCategoryProducts).FirstOrDefaultAsync(p => p.ProductID == id);
                    if (productUpdate == null)
                    {
                        return NotFound();
                    }

                    productUpdate.Title = product.Title;
                    productUpdate.Description = product.Description;
                    productUpdate.Content = product.Content;
                    productUpdate.Published = product.Published;
                    productUpdate.Slug = product.Slug;
                    productUpdate.DateUpdated = DateTime.Now;
                    productUpdate.Price = product.Price;


                    // Update PostCategory
                    if (product.CategoryIDs == null) product.CategoryIDs = new int[] { };

                    var oldCateIds = productUpdate.ProductCategoryProducts.Select(c => c.CategoryID).ToArray();
                    var newCateIds = product.CategoryIDs;

                    var removeCatePosts = from productCate in productUpdate.ProductCategoryProducts
                                          where (!newCateIds.Contains(productCate.CategoryID))
                                          select productCate;
                    _context.ProductCategoryProducts.RemoveRange(removeCatePosts);

                    var addCateIds = from CateId in newCateIds
                                     where !oldCateIds.Contains(CateId)
                                     select CateId;

                    foreach (var CateId in addCateIds)
                    {
                        _context.ProductCategoryProducts.Add(new ProductCategoryProduct()
                        {
                            ProductID = id,
                            CategoryID = CateId
                        });
                    }

                    _context.Update(productUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "A product has been updated";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", product.AuthorId);
            return View(product);
        }

        // GET: Blog/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Blog/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Products.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Products.Remove(post);
            await _context.SaveChangesAsync();

            StatusMessage = "A product has been deleted: " + post.Title;

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }

        public class UploadOneFile
        {
            [Required]
            [DataType(DataType.Upload)]
            [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
            public IFormFile FileUpload { get; set; }
        }

        [HttpGet]
        public IActionResult UploadPhoto(int id)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                            .Include(p => p.Photos)
                            .FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            ViewData["product"] = product;
            return View(new UploadOneFile());
        }

        [HttpPost, ActionName("UploadPhoto")]
        public async Task<IActionResult> UploadPhotoAsync(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.Photos)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }
            ViewData["product"] = product;

            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);

                var file = Path.Combine("Uploads", "Products", file1);

                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream);
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductID,
                    FileName = file1
                });

                await _context.SaveChangesAsync();
            }


            return View(f);
        }

        [HttpPost]
        public IActionResult ListPhotos(int id)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.Photos)
                .FirstOrDefault();

            if (product == null)
            {
                return Json(
                    new
                    {
                        success = 0,
                        message = "Product not found",
                    }
                );
            }

            var listphotos = product.Photos.Select(photo => new
            {
                id = photo.Id,
                path = "/contents/Products/" + photo.FileName
            });

            return Json(
                new
                {
                    success = 1,
                    photos = listphotos
                }
            );


        }

        [HttpPost]
        public IActionResult DeletePhoto(int id)
        {
            var photo = _context.ProductPhotos.Where(p => p.Id == id).FirstOrDefault();
            if (photo != null)
            {
                _context.Remove(photo);
                _context.SaveChanges();

                var filename = "Uploads/Products/" + photo.FileName;
                System.IO.File.Delete(filename);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotoApi(int id, [Bind("FileUpload")] UploadOneFile f)
        {
            var product = _context.Products.Where(e => e.ProductID == id)
                .Include(p => p.Photos)
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }


            if (f != null)
            {
                var file1 = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                            + Path.GetExtension(f.FileUpload.FileName);

                var file = Path.Combine("Uploads", "Products", file1);

                using (var filestream = new FileStream(file, FileMode.Create))
                {
                    await f.FileUpload.CopyToAsync(filestream);
                }

                _context.Add(new ProductPhoto()
                {
                    ProductID = product.ProductID,
                    FileName = file1
                });

                await _context.SaveChangesAsync();
            }


            return Ok();
        }

    }
}