using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            //Объявляем модель типа List
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                //Инициализируем модель данными
                categoryVMList = db.Categories
                                 .ToArray()
                                 .OrderBy(x => x.Sorting)
                                 .Select(x => new CategoryVM(x))
                                 .ToList();
            }
                //Возвращаем List в представление
                return View(categoryVMList);
        }

        
        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Объявляем строковую переменную ID
            string id;

            using (Db db = new Db())
            {
                //Проверяем имя категории на уникальность
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                //Инициализируем модель DTO
                CategoryDTO dto = new CategoryDTO();

                //Добавляем данные в модель
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                //Сохранить
                db.Categories.Add(dto);
                db.SaveChanges();

                //Получаем ID для возврата в представление
                id = dto.Id.ToString();

            }

            //Возвращаем ID в представление
            return id;
        }

        
        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //Установить начальный счётчик
                int count = 1;

                //Объявить PageDTO
                CategoryDTO dto;

                //Установить сортировку для каждой страницы
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }

        
        
    }
}