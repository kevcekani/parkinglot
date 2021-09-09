using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Parking.Models;
using System.Net;
using System.Data.Entity;
using ZXing;
using System.IO;
using System.Web.UI;
using Newtonsoft.Json;
using System.Data;

namespace Parking.Controllers
{
    public class CarController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Car
        public ActionResult Index(string searchString)
        {
            var cars = from c in db.Cars
                       select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.CarPlate.Contains(searchString) || s.OwnerID.Contains(searchString) ||
                           s.EnterDate.ToString().Contains(searchString));

            }

            cars = cars.Where(s => s.CheckInOut == true);
            return View(cars);
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            return View(car);
            
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Car/Create
        [HttpPost]
        public ActionResult Create(CarInput car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            db.Cars.Add(new Car { ID = car.ID, CarPlate = car.CarPlate, OwnerID = car.OwnerID, EnterDate = DateTime.Now, CheckInOut = true });
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Car/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,CarPlate,OwnerId,EnterDate")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Car car = db.Cars.Find(id);
            car.CheckInOut = false;
            //db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Function to generate QR Code; saves it on the downloads folder
        public ActionResult GenerateCode(int id)
        {
            
            Car car = db.Cars.Find(id);
            string jsonstr = JsonConvert.SerializeObject(car, Formatting.Indented);

            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 200,
                    Width = 200
                }
            };

            barcodeWriter.Write(jsonstr)
                         .Save(String.Format(@"C:\Users\Kev\Downloads\{0}.png", car.CarPlate));

            return RedirectToAction("Index");
        }
        
        public class CarInput
        {
            public int ID { get; set; }
            public string CarPlate { get; set; }
            public string OwnerID { get; set; }
            public DateTime EnterDate { get; set; }
            public string Hour { get; set; }
            public bool CheckInOut { get; set; }
        }
    }
}
