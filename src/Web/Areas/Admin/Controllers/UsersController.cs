using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.App_Start;
using Web.Areas.Admin.Models.Users;
using Web.Data;
using Web.Data.Identity;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class UsersController : BaseController
    {

        private readonly DataContext _context;
        private ApplicationUserManager _userManager;


        private static readonly IMapper _mapper = new Mapper(new MapperConfiguration(x =>
        {
            x.CreateMap<ApplicationUser, UserListItemViewModel>();
            x.CreateMap<ApplicationUser, UserChangePasswordViewModel>();
            x.CreateMap<ApplicationUser, DeleteUserViewModel>();
            x.CreateMap<ApplicationUser, ActivateUserViewModel>();
        }));


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public UsersController()
        {
            _context = new DataContext();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            var mappedUsers = _mapper.Map<List<ApplicationUser>, List<UserListItemViewModel>>(users);
            return View(mappedUsers);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Email == model.Email);
            if (user != null)
            {
                ModelState.AddModelError("EmailIsBusy", "Данный адрес электронной почты уже занят другим пользователем!");
                return View(model);
            }
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await UserManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                await UserManager.AddToRoleAsync(newUser.Id, "Admin");
            }
            ShowAlert(Enums.AlertTypes.Success, "Пользователь был успешно добавлен!");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> ChangePassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id пользователя");
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            var mappedUser = _mapper.Map<ApplicationUser, UserChangePasswordViewModel>(user);
            return View(mappedUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(UserChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            if (await _context.Roles.Where(f => f.Name == "Supervisor").AnyAsync(f => f.Users.Any(s => s.UserId == user.Id)))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Невозможно сменить пароль пользователю с ролью Supervisor");
            }
            user.PasswordHash = new PasswordHasher().HashPassword(model.Password);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, "Обновление пароля прошло успешно!");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id пользователя");
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            if (await _context.Roles.Where(f => f.Name == "Supervisor").AnyAsync(f => f.Users.Any(s => s.UserId == user.Id)))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Невозможно удалить пользователя с ролью Supervisor");
            }
            var mappedUser = _mapper.Map<ApplicationUser, DeleteUserViewModel>(user);
            return View(mappedUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Delete", new { userId = model.Id });
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            if (await _context.Roles.Where(f => f.Name == "Supervisor").AnyAsync(f => f.Users.Any(s => s.UserId == user.Id)))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Невозможно удалить пользователя с ролью Supervisor");
            }
            _context.Entry(user).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, "Удаление пользователя прошло успешно!");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<ActionResult> ChangeActivation(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Необходим id пользователя");
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            if (await _context.Roles.Where(f => f.Name == "Supervisor").AnyAsync(f => f.Users.Any(s => s.UserId == user.Id)))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Невозможно активировать/деактивировать пользователя с ролью Supervisor");
            }
            var mappedUser = _mapper.Map<ApplicationUser, ActivateUserViewModel>(user);
            return View(mappedUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeActivation(ActivateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == model.Id);
            if (user == null)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "Пользователь с таким id не найден");
            }
            if (await _context.Roles.Where(f => f.Name == "Supervisor").AnyAsync(f => f.Users.Any(s => s.UserId == user.Id)))
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Невозможно активировать/деактивировать пользователя с ролью Supervisor");
            }
            user.IsActive = model.IsActive;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            ShowAlert(Enums.AlertTypes.Success, $"{(user.IsActive ? "Активация" : "Деактивация")} пользователя прошло успешно!");
            return RedirectToAction("Index");
        }
    }
}