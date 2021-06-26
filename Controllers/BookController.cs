using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVCCrudWithoutEF.Data;
using MVCCrudWithoutEF.Models;

namespace MVCCrudWithoutEF.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Book
        public  IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                conn.Open();

                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll", conn);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }



        // GET: Book/AddOrEdit/5
        public IActionResult AddOrEdit(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            if(id > 0)
            {
                bookViewModel = fetchBookById(id);
            }
            return View(bookViewModel);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult AddOrEdit(int id, [Bind("BookId,Title,Author,Price")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {    
                using(SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    conn.Open();
                    SqlCommand sqlCommand = new SqlCommand("BookAddOrEdit", conn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("BookId", bookViewModel.BookId);
                    sqlCommand.Parameters.AddWithValue("Title", bookViewModel.Title);
                    sqlCommand.Parameters.AddWithValue("Author", bookViewModel.Author);
                    sqlCommand.Parameters.AddWithValue("Price", bookViewModel.Price);
                    sqlCommand.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
            BookViewModel bookViewModel = fetchBookById(id);
            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand("BookDeleteById", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("BookId", id);
                
                sqlCommand.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookViewModel fetchBookById(int? id)
        {
            BookViewModel bookViewModel = new BookViewModel();
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                conn.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewById", conn);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("BookId", id);
                sqlDa.Fill(dtbl);

                if(dtbl.Rows.Count == 1)
                {
                    bookViewModel.BookId = (int)dtbl.Rows[0]["BookId"];
                    bookViewModel.Title = dtbl.Rows[0]["Title"].ToString();
                    bookViewModel.Author = dtbl.Rows[0]["Author"].ToString();
                    bookViewModel.Price = (int)dtbl.Rows[0]["Price"];
                }
                return bookViewModel;
            }
        }
    }
}
